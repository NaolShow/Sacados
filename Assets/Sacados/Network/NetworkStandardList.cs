using System;
using System.Collections.Generic;

namespace Unity.Netcode {

    public sealed class NetworkStandardList<T> : NetworkVariableBase where T : new() {

        private List<T> list;
        private List<NetworkListEvent<T>> dirtyEvents;

        /// <summary>
        /// Delegate type for list changed event
        /// </summary>
        /// <param name="changeEvent">Struct containing information about the change event</param>
        public delegate void OnListChangedDelegate(NetworkListEvent<T> changeEvent);

        /// <summary>
        /// The callback to be invoked when the list gets changed
        /// </summary>
        public event OnListChangedDelegate OnListChanged;

        public NetworkStandardList() : this(64) { }

        public NetworkStandardList(int capacity) {
            list = new List<T>(capacity);
            dirtyEvents = new List<NetworkListEvent<T>>(capacity);
        }

        /// <inheritdoc/>
        /// <param name="values"></param>
        /// <param name="readPerm"></param>
        /// <param name="writePerm"></param>
        public NetworkStandardList(IEnumerable<T> values = default, NetworkVariableReadPermission readPerm = DefaultReadPerm, NetworkVariableWritePermission writePerm = DefaultWritePerm) : base(readPerm, writePerm) {
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

        #region Read & Write

        #endregion

        public override void WriteDelta(FastBufferWriter writer) {

            if (base.IsDirty()) {
                writer.WriteValueSafe((ushort)1);
                writer.WriteValueSafe(NetworkListEvent<T>.EventType.Full);
                WriteField(writer);
                return;
            }

            // Write all the events that the list have
            writer.WriteValueSafe((ushort)dirtyEvents.Count);
            for (int i = 0; i < dirtyEvents.Count; i++) {
                var element = dirtyEvents[i];
                writer.WriteValueSafe(in element.Type);

                // If the list is getting cleared there is no need to send extra informations
                if (element.Type == NetworkListEvent<T>.EventType.Clear) continue;

                // If the list have a value that is getting inserted, removed or modified then send the element index
                if (element.Type == NetworkListEvent<T>.EventType.Insert || element.Type == NetworkListEvent<T>.EventType.RemoveAt || element.Type == NetworkListEvent<T>.EventType.Value)
                    writer.WriteValueSafe(in element.Index);

                // If the list is inserting or modifying a value then send the element data
                if (element.Type != NetworkListEvent<T>.EventType.RemoveAt)
                    NetworkVariableSerialization<T>.Write(writer, ref element.Value);

            }
        }

        public override void WriteField(FastBufferWriter writer) {
            writer.WriteValueSafe((ushort)list.Count);
            for (int i = 0; i < list.Count; i++) {
                T value = list[i];
                NetworkVariableSerialization<T>.Write(writer, ref value);
            }
        }

        public override void ReadField(FastBufferReader reader) {
            list.Clear();
            reader.ReadValueSafe(out ushort count);
            for (int i = 0; i < count; i++) {
                var value = new T();
                NetworkVariableSerialization<T>.Read(reader, ref value);
                list.Add(value);
            }
        }

        public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta) {
            reader.ReadValueSafe(out ushort deltaCount);
            for (int i = 0; i < deltaCount; i++) {
                reader.ReadValueSafe(out NetworkListEvent<T>.EventType eventType);

                NetworkListEvent<T> listEvent = new NetworkListEvent<T>();

                switch (eventType) {
                    case NetworkListEvent<T>.EventType.Add: {
                            var value = new T();
                            NetworkVariableSerialization<T>.Read(reader, ref value);
                            list.Add(value);

                            listEvent = new NetworkListEvent<T>() {
                                Type = eventType,
                                Index = list.Count - 1,
                                Value = list[list.Count - 1]
                            };

                        }
                        break;
                    case NetworkListEvent<T>.EventType.Insert: {
                            reader.ReadValueSafe(out int index);
                            T value = new T();
                            NetworkVariableSerialization<T>.Read(reader, ref value);

                            if (index < list.Count) {
                                list.Insert(index, value);
                            } else {
                                list.Add(value);
                            }

                            listEvent = new NetworkListEvent<T>() {
                                Type = eventType,
                                Index = index,
                                Value = list[index]
                            };
                        }
                        break;
                    case NetworkListEvent<T>.EventType.Remove: {

                            T value = new T();
                            NetworkVariableSerialization<T>.Read(reader, ref value);
                            int index = list.IndexOf(value);
                            if (index == -1) {
                                break;
                            }

                            list.RemoveAt(index);

                            listEvent = new NetworkListEvent<T>() {
                                Type = eventType,
                                Index = index,
                                Value = value
                            };
                        }
                        break;
                    case NetworkListEvent<T>.EventType.RemoveAt: {
                            reader.ReadValueSafe(out int index);
                            T value = list[index];
                            list.RemoveAt(index);

                            listEvent = new NetworkListEvent<T>() {
                                Type = eventType,
                                Index = index,
                                Value = value
                            };
                        }
                        break;
                    case NetworkListEvent<T>.EventType.Value: {
                            reader.ReadValueSafe(out int index);
                            var value = new T();
                            NetworkVariableSerialization<T>.Read(reader, ref value);
                            if (index >= list.Count) {
                                throw new Exception("Shouldn't be here, index is higher than list length");
                            }

                            var previousValue = list[index];
                            list[index] = value;

                            listEvent = new NetworkListEvent<T>() {
                                Type = eventType,
                                Index = index,
                                Value = value,
                                PreviousValue = previousValue
                            };
                        }
                        break;
                    case NetworkListEvent<T>.EventType.Clear: {
                            //Read nothing
                            list.Clear();

                            listEvent = new NetworkListEvent<T> {
                                Type = eventType,
                            };
                        }
                        break;
                    case NetworkListEvent<T>.EventType.Full: {
                            ReadField(reader);
                            ResetDirty();
                        }
                        break;
                }

                if (eventType != NetworkListEvent<T>.EventType.Full) {

                    OnListChanged?.Invoke(listEvent);
                    if (keepDirtyDelta)
                        dirtyEvents.Add(listEvent);

                }

            }
        }

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        public void Add(T item) {

            list.Add(item);

            var listEvent = new NetworkListEvent<T>() {
                Type = NetworkListEvent<T>.EventType.Add,
                Value = item,
                Index = list.Count - 1
            };
            HandleAddListEvent(listEvent);
        }

        public void Clear() {

            list.Clear();

            var listEvent = new NetworkListEvent<T>() {
                Type = NetworkListEvent<T>.EventType.Clear
            };

            HandleAddListEvent(listEvent);
        }

        public bool Contains(T item) {
            int index = list.IndexOf(item);
            return index != -1;
        }

        public bool Remove(T item) {

            int index = list.IndexOf(item);
            if (index == -1) {
                return false;
            }

            list.RemoveAt(index);
            var listEvent = new NetworkListEvent<T>() {
                Type = NetworkListEvent<T>.EventType.Remove,
                Value = item
            };

            HandleAddListEvent(listEvent);
            return true;
        }

        public int Count => list.Count;

        public int IndexOf(T item) {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item) {

            if (index < list.Count) {
                list.Insert(index, item);
            } else {
                list.Add(item);
            }

            var listEvent = new NetworkListEvent<T>() {
                Type = NetworkListEvent<T>.EventType.Insert,
                Index = index,
                Value = item
            };

            HandleAddListEvent(listEvent);
        }

        public void RemoveAt(int index) {

            list.RemoveAt(index);

            var listEvent = new NetworkListEvent<T>() {
                Type = NetworkListEvent<T>.EventType.RemoveAt,
                Index = index
            };

            HandleAddListEvent(listEvent);
        }

        public T this[int index] {
            get => list[index];
            set {

                var previousValue = list[index];
                list[index] = value;

                var listEvent = new NetworkListEvent<T>() {
                    Type = NetworkListEvent<T>.EventType.Value,
                    Index = index,
                    Value = value,
                    PreviousValue = previousValue
                };

                HandleAddListEvent(listEvent);
            }
        }

        private void MarkNetworkObjectDirty() => m_NetworkBehaviour.NetworkManager.MarkNetworkObjectDirty(m_NetworkBehaviour.NetworkObject);

        private void HandleAddListEvent(NetworkListEvent<T> listEvent) {
            dirtyEvents.Add(listEvent);
            MarkNetworkObjectDirty();
            OnListChanged?.Invoke(listEvent);
        }

    }

}