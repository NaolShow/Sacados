using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using static Sacados.IContainerStorage;

namespace Sacados {

    // TODO: Rename to NetworkContainerStorage
    public class FishNetContainerStorage : NetworkBehaviour, IContainerStorage {

        public bool IsReady => IsSpawned;

        private readonly SyncList<ItemStack> itemStacks = new SyncList<ItemStack>();
        private bool done;

        public event OnContainerUpdateDelegate OnUpdate;
        public event Action OnStarted;
        public event Action OnStopped;

        public ItemStack this[int index] {
            get => itemStacks[index]; set => itemStacks[index] = value;
        }

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
            OnUpdate?.Invoke(ContainerEventType.Clear, null, -1);
            done = false;
        }

        private void InternalOnItemStacksChanged(SyncListOperation operation, int index, ItemStack oldItemStack, ItemStack newItemStack, bool asServer) {

            // If the container is now done initializing
            if (!done && operation == SyncListOperation.Complete) done = true;
            // If the container is not done creating
            if (!done) return;

            // Only call for the server side FIRST or for the client
            if (IsClientOnlyInitialized || asServer)
                OnUpdate(operation.ToContainerEventType(), oldItemStack, index);
        }

        public int Count => itemStacks.Count;
        public bool IsReadOnly => !IsSpawned || !IsServerInitialized;

        public void Add(ItemStack itemStack) => itemStacks.Add(itemStack);
        public void Insert(int index, ItemStack itemStack) => itemStacks.Insert(index, itemStack);
        public bool Remove(ItemStack itemStack) => itemStacks.Remove(itemStack);
        public void RemoveAt(int index) => itemStacks.RemoveAt(index);
        public void Clear() => itemStacks.Clear();

        public bool Contains(ItemStack itemStack) => itemStacks.Contains(itemStack);
        public int IndexOf(ItemStack itemStack) => itemStacks.IndexOf(itemStack);
        public void CopyTo(ItemStack[] array, int arrayIndex) => itemStacks.CopyTo(array, arrayIndex);

        public IEnumerator<ItemStack> GetEnumerator() => (IEnumerator<ItemStack>)itemStacks.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => itemStacks.GetEnumerator();

    }
}