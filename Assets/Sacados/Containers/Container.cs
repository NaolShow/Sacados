using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections.Generic;
using static Sacados.IContainer;

namespace Sacados {

    /// <summary>
    /// Basic implementation of <see cref="IContainer"/>
    /// </summary>
    public abstract class Container : NetworkBehaviour, IContainer {

        private bool done;

        public int Size => itemStacks.Count;
        private readonly List<ISlot> slots = new List<ISlot>();
        public ISlot GetSlot(int index) => slots[index];

        public ItemStack this[int i] {
            get => itemStacks[i];
            set => itemStacks[i] = value;
        }
        private readonly SyncList<ItemStack> itemStacks = new SyncList<ItemStack>();

        public event OnContainerUpdateDelegate OnUpdate;
        public event Action OnStarted;
        public event Action OnStopped;

        #region Container Update

        /// <inheritdoc cref="IContainer.OnUpdate"/>
        protected virtual void OnContainerUpdate(ContainerEventType type, ItemStack oldItemStack, int index) => OnUpdate?.Invoke(type, oldItemStack, index);
        private void InternalOnItemStacksChanged(SyncListOperation operation, int index, ItemStack oldItemStack, ItemStack newItemStack, bool asServer) {

            // If the container is now done initializing
            if (!done && operation == SyncListOperation.Complete) done = true;
            // If the container is not done creating
            if (!done) return;

            // Only call for the server side FIRST or for the client
            if (IsClientOnlyInitialized || asServer)
                OnContainerUpdate(operation.ToContainerEventType(), oldItemStack, index);
        }

        #endregion

        public override void OnStartServer() => OnCommonStart();
        public override void OnStartClient() {
            // If we are not the server
            if (!IsServerInitialized)
                OnCommonStart();
        }

        public override void OnStopServer() => OnCommonStop();
        public override void OnStopClient() {
            if (!IsServerInitialized)
                OnCommonStop();
        }

        protected virtual void OnCommonStart() {
            OnStarted?.Invoke();
            itemStacks.OnChange += InternalOnItemStacksChanged;
        }

        protected virtual void OnCommonStop() {
            itemStacks.OnChange -= InternalOnItemStacksChanged;
            OnStopped?.Invoke();
            OnContainerUpdate(ContainerEventType.Clear, null, -1);
            done = false;
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
            if (IsSpawned && IsServerInitialized) itemStacks.Insert(slot.Index, itemStack?.Clone());

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
            if (IsSpawned && IsServerInitialized) itemStacks.RemoveAt(index);

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
            if (IsSpawned && IsServerInitialized) itemStacks.Clear();

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