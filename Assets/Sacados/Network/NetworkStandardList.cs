using System;
using System.Collections;
using System.Collections.Generic;

namespace Unity.Netcode {

    /// <summary>
    /// Represents a <see cref="List{T}"/> that is synchronized as a <see cref="List{T}"/><br/>
    /// This allows you to synchronize managed types objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NetworkStandardList<T> : NetworkStandardList<T, T> where T : new() { }
    /// <summary>
    /// Represents a <see cref="List{TOriginal}"/> that is synchronized as a <see cref="List{TCast}"/><br/>
    /// This allows you to:<br/>
    /// * Synchronize managed types objects<br/>
    /// * Synchronize those objects with their <see cref="{TCast}"/> types (so it's serializer)
    /// </summary>
    /// <typeparam name="TOriginal">Original type of the objet (will be store with this one)</typeparam>
    /// <typeparam name="TCast">Cast type of the object (will be serialized with this one)</typeparam>
    public class NetworkStandardList<TOriginal, TCast> : NetworkVariableBase, IList<TOriginal>, ICollection<TOriginal>, IEnumerable<TOriginal>, IReadOnlyCollection<TOriginal>, IReadOnlyList<TOriginal>
        where TOriginal : TCast, new() {

        private List<TOriginal> list;
        // TODO: More optimized way use a dictionnary
        // => Overwrite changes to that index to not send multiple events
        private List<NetworkListEvent<TOriginal>> dirtyEvents;

        /// <summary>
        /// Delegate type for list changed event
        /// </summary>
        /// <param name="changeEvent">Struct containing information about the change event</param>
        public delegate void OnListChangedDelegate(NetworkListEvent<TOriginal> changeEvent);

        /// <summary>
        /// The callback to be invoked when the list gets changed
        /// </summary>
        public event OnListChangedDelegate OnListChanged;

        public NetworkStandardList() : this(64) { }

        public NetworkStandardList(int capacity) {
            list = new List<TOriginal>(capacity);
            dirtyEvents = new List<NetworkListEvent<TOriginal>>(capacity);
        }

        /// <inheritdoc/>
        /// <param name="values"></param>
        /// <param name="readPerm"></param>
        /// <param name="writePerm"></param>
        public NetworkStandardList(IEnumerable<TOriginal> values = default, NetworkVariableReadPermission readPerm = DefaultReadPerm, NetworkVariableWritePermission writePerm = DefaultWritePerm) : base(readPerm, writePerm) {
            // allow null IEnumerable<T> to mean "no values"
            if (values != null) {
                foreach (var value in values) {
                    list.Add(value);
                }
            }
        }

        #region Dirty

        public override void ResetDirty() {
            base.ResetDirty();
            dirtyEvents.Clear();
        }

        public override bool IsDirty() => base.IsDirty() || dirtyEvents.Count > 0;

        #endregion

        public override void WriteDelta(FastBufferWriter writer) {

            if (base.IsDirty()) {
                writer.WriteValueSafe((ushort)1);
                writer.WriteValueSafe(NetworkListEvent<TOriginal>.EventType.Full);
                WriteField(writer);
                return;
            }

            // Write all the events that the list have
            writer.WriteValueSafe((ushort)dirtyEvents.Count);
            for (int i = 0; i < dirtyEvents.Count; i++) {
                var element = dirtyEvents[i];
                writer.WriteValueSafe(in element.Type);

                // If the list is getting cleared there is no need to send extra informations
                if (element.Type == NetworkListEvent<TOriginal>.EventType.Clear) continue;

                // If the list have a value that is getting inserted, removed or modified then send the element index
                if (element.Type == NetworkListEvent<TOriginal>.EventType.Insert || element.Type == NetworkListEvent<TOriginal>.EventType.RemoveAt || element.Type == NetworkListEvent<TOriginal>.EventType.Value)
                    writer.WriteValueSafe(in element.Index);

                // If the list is inserting or modifying a value then send the element data
                if (element.Type != NetworkListEvent<TOriginal>.EventType.RemoveAt) {
                    TCast tElement = element.Value;
                    NetworkVariableSerialization<TCast>.Write(writer, ref tElement);
                }

            }
        }

        public override void WriteField(FastBufferWriter writer) {
            writer.WriteValueSafe((ushort)list.Count);
            for (int i = 0; i < list.Count; i++) {
                TCast value = list[i];
                NetworkVariableSerialization<TCast>.Write(writer, ref value);
            }
        }

        public override void ReadField(FastBufferReader reader) {
            list.Clear();
            reader.ReadValueSafe(out ushort count);
            for (int i = 0; i < count; i++) {
                TCast value = new TOriginal();
                NetworkVariableSerialization<TCast>.Read(reader, ref value);
                list.Add((TOriginal)value);
            }
            OnListChanged?.Invoke(new NetworkListEvent<TOriginal>() {
                Type = NetworkListEvent<TOriginal>.EventType.Full
            });
        }

        public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta) {
            reader.ReadValueSafe(out ushort deltaCount);
            for (int i = 0; i < deltaCount; i++) {
                reader.ReadValueSafe(out NetworkListEvent<TOriginal>.EventType eventType);

                NetworkListEvent<TOriginal> listEvent = new NetworkListEvent<TOriginal>();

                switch (eventType) {
                    case NetworkListEvent<TOriginal>.EventType.Add: {
                            TCast value = new TOriginal();
                            NetworkVariableSerialization<TCast>.Read(reader, ref value);
                            list.Add((TOriginal)value);

                            listEvent = new NetworkListEvent<TOriginal>() {
                                Type = eventType,
                                Index = list.Count - 1,
                                Value = list[list.Count - 1]
                            };

                        }
                        break;
                    case NetworkListEvent<TOriginal>.EventType.Insert: {
                            reader.ReadValueSafe(out int index);
                            TCast value = new TOriginal();
                            NetworkVariableSerialization<TCast>.Read(reader, ref value);

                            if (index < list.Count) {
                                list.Insert(index, (TOriginal)value);
                            } else {
                                list.Add((TOriginal)value);
                            }

                            listEvent = new NetworkListEvent<TOriginal>() {
                                Type = eventType,
                                Index = index,
                                Value = list[index]
                            };
                        }
                        break;
                    case NetworkListEvent<TOriginal>.EventType.Remove: {

                            TCast value = new TOriginal();
                            NetworkVariableSerialization<TCast>.Read(reader, ref value);
                            int index = list.IndexOf((TOriginal)value);
                            if (index == -1) {
                                break;
                            }

                            list.RemoveAt(index);

                            listEvent = new NetworkListEvent<TOriginal>() {
                                Type = eventType,
                                Index = index,
                                Value = (TOriginal)value
                            };
                        }
                        break;
                    case NetworkListEvent<TOriginal>.EventType.RemoveAt: {
                            reader.ReadValueSafe(out int index);
                            TOriginal value = list[index];
                            list.RemoveAt(index);

                            listEvent = new NetworkListEvent<TOriginal>() {
                                Type = eventType,
                                Index = index,
                                Value = value
                            };
                        }
                        break;
                    case NetworkListEvent<TOriginal>.EventType.Value: {
                            reader.ReadValueSafe(out int index);
                            TCast value = new TOriginal();
                            NetworkVariableSerialization<TCast>.Read(reader, ref value);
                            if (index >= list.Count) {
                                throw new Exception("Shouldn't be here, index is higher than list length");
                            }

                            var previousValue = list[index];
                            list[index] = (TOriginal)value;

                            listEvent = new NetworkListEvent<TOriginal>() {
                                Type = eventType,
                                Index = index,
                                Value = (TOriginal)value,
                                PreviousValue = previousValue
                            };
                        }
                        break;
                    case NetworkListEvent<TCast>.EventType.Clear: {
                            //Read nothing
                            list.Clear();

                            listEvent = new NetworkListEvent<TOriginal> {
                                Type = eventType,
                            };
                        }
                        break;
                    case NetworkListEvent<TOriginal>.EventType.Full: {
                            ReadField(reader);
                            ResetDirty();
                        }
                        break;
                }

                if (eventType != NetworkListEvent<TOriginal>.EventType.Full) {

                    OnListChanged?.Invoke(listEvent);
                    if (keepDirtyDelta)
                        dirtyEvents.Add(listEvent);

                }

            }
        }

        public IEnumerator<TOriginal> GetEnumerator() => list.GetEnumerator();

        public void Add(TOriginal item) {

            list.Add(item);

            var listEvent = new NetworkListEvent<TOriginal>() {
                Type = NetworkListEvent<TOriginal>.EventType.Add,
                Value = item,
                Index = list.Count - 1
            };
            HandleEvent(listEvent);
        }

        public void Clear() {

            list.Clear();

            var listEvent = new NetworkListEvent<TOriginal>() {
                Type = NetworkListEvent<TOriginal>.EventType.Clear
            };

            HandleEvent(listEvent);
        }

        public bool Contains(TOriginal item) {
            int index = list.IndexOf(item);
            return index != -1;
        }

        public bool Remove(TOriginal item) {

            int index = list.IndexOf(item);
            if (index == -1) {
                return false;
            }

            list.RemoveAt(index);
            var listEvent = new NetworkListEvent<TOriginal>() {
                Type = NetworkListEvent<TOriginal>.EventType.Remove,
                Value = item
            };

            HandleEvent(listEvent);
            return true;
        }

        public int Count => list.Count;

        public int IndexOf(TOriginal item) {
            return list.IndexOf(item);
        }

        public void Insert(int index, TOriginal item) {

            if (index < list.Count) {
                list.Insert(index, item);
            } else {
                list.Add(item);
            }

            var listEvent = new NetworkListEvent<TOriginal>() {
                Type = NetworkListEvent<TOriginal>.EventType.Insert,
                Index = index,
                Value = item
            };

            HandleEvent(listEvent);
        }

        public void RemoveAt(int index) {

            list.RemoveAt(index);

            var listEvent = new NetworkListEvent<TOriginal>() {
                Type = NetworkListEvent<TOriginal>.EventType.RemoveAt,
                Index = index
            };

            HandleEvent(listEvent);
        }

        public TOriginal this[int index] {
            get => list[index];
            set {

                // Store the previous value
                var previousValue = list[index];
                list[index] = value;

                var listEvent = new NetworkListEvent<TOriginal>() {
                    Type = NetworkListEvent<TOriginal>.EventType.Value,
                    Index = index,
                    Value = value,
                    PreviousValue = previousValue
                };

                HandleEvent(listEvent);
            }
        }

        private void HandleEvent(NetworkListEvent<TOriginal> listEvent) {

            // Indicates to netcode that the behaviour is now dirty
            m_NetworkBehaviour.NetworkManager.MarkNetworkObjectDirty(m_NetworkBehaviour.NetworkObject);

            // Store the new event and trigger it
            dirtyEvents.Add(listEvent);
            OnListChanged?.Invoke(listEvent);

        }

        public void CopyTo(TOriginal[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

        public bool IsReadOnly => ((IList)list).IsReadOnly;
        public bool IsFixedSize => ((IList)list).IsFixedSize;
        public bool IsSynchronized => ((ICollection)list).IsSynchronized;

        public object SyncRoot => ((ICollection)list).SyncRoot;

    }

}