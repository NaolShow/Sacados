using System;
using System.Collections.Generic;
using Unity.Netcode;
using static Sacados.IContainer;

namespace Sacados {

    /// <summary>
    /// Basic implementation of <see cref="IContainer"/>
    /// </summary>
    public abstract class Container : NetworkBehaviour, IContainer {

        public int SlotsCount => itemStacks.Count;
        private readonly List<ISlot> slots = new List<ISlot>();
        public ISlot Get(int index) => slots[index];

        public ItemStack this[int i] { get => itemStacks[i]; set => itemStacks[i] = value; }
        private readonly NetworkStandardList<ItemStack> itemStacks = new NetworkStandardList<ItemStack>();

        public event OnContainerUpdateDelegate OnUpdate;
        public event Action OnStarted;
        public event Action OnStopped;

        #region Container Update

        // Subscribe to the ItemStack's list changed event
        // => Doesn't need to unsubscribe since the list lifetime = container's lifetime
        protected virtual void Awake() => itemStacks.OnListChanged += InternalOnListChanged;

        /// <inheritdoc cref="IContainer.OnUpdate"/>
        protected virtual void OnContainerUpdate(ContainerEventType type, ItemStack oldItemStack, int index) => OnUpdate?.Invoke(type, oldItemStack, index);
        private void InternalOnListChanged(NetworkListEvent<ItemStack> e) => OnContainerUpdate(e.ToContainerEventType(), e.PreviousValue, e.Index);

        #endregion

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();

            // If we are the server and there is a different amount of slots than ItemStacks then add empty ItemStacks
            if (IsServer && slots.Count != itemStacks.Count)
                for (int i = 0; i < slots.Count; i++)
                    itemStacks.Add(null);

            // Call the on started event
            OnStarted?.Invoke();

        }

        public override void OnNetworkDespawn() {
            base.OnNetworkDespawn();

            // Clear the slots and the ItemStacks list (netcode doesn't clear it automatically)
            ClearSlots();
            itemStacks.Clear();

            // Call the onstopped event
            OnStopped?.Invoke();

        }

        #region Slots Management

        /// <summary>
        /// Adds the specified <see cref="ISlot"/> to the <see cref="Container"/>
        /// </summary>
        /// <param name="slot">The <see cref="ISlot"/> that will be added</param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be added</param>
        protected void AddSlot(ISlot slot, ItemStack itemStack = null) {

            // Insert the slot and also it's ItemStack if we are the server and spawned
            slots.Insert(slot.Index, slot);
            if (IsSpawned && IsServer) itemStacks.Insert(slot.Index, itemStack?.Clone());

            // Reorder the slots indexes only if the added slot isn't at the end
            for (int i = slot.Index; i < slots.Count; i++)
                slots[i].Index = i;

        }

        /// <summary>
        /// Removes the <see cref="ISlot"/> at the specified index from the <see cref="Container"/>
        /// </summary>
        /// <param name="index">The index of the <see cref="ISlot"/> that will be removed</param>
        protected void RemoveSlot(int index) {

            // Remove the slot and also it's ItemStack if we are the server and spawned
            slots.RemoveAt(index);
            if (IsSpawned && IsServer) itemStacks.RemoveAt(index);

            // Reorder the slots indexes only if the removed slot isn't at the end
            for (int i = index; i < slots.Count; i++)
                slots[i].Index = i;

        }

        /// <summary>
        /// Clears all the <see cref="ISlot"/> from the <see cref="Container"/>
        /// </summary>
        protected void ClearSlots() {

            // Clear the slots and also the ItemStacks if we are the server and spawned
            slots.Clear();
            if (IsSpawned && IsServer) itemStacks.Clear();

        }

        #endregion

        // User implementation
        public abstract void Give(ItemStack itemStack);
        public abstract void Take(ItemStack itemStack);

        // By default accept to give and take any ItemStack
        public virtual bool CanBeGiven(ItemStack itemStack) => true;
        public virtual bool CanBeTaken(ItemStack itemStack) => true;

        public abstract void Clear();

    }

}