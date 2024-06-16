using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using static Sacados.IContainerStorage;

namespace Sacados {

    /// <summary>
    /// Represents a storage for a <see cref="Container"/> that synchronizes over the network
    /// </summary>
    public class NetworkContainerStorage : NetworkBehaviour, IContainerStorage {

        public int Count => itemStacks.Count;
        public bool IsReady => IsSpawned && (OnStartServerCalled || OnStartClientCalled);
        public bool IsReadOnly => !IsSpawned || !IsServerInitialized;

        public event OnContainerUpdateDelegate OnUpdate;
        public event Action OnStarted;
        public event Action OnStopped;

        private readonly SyncList<ItemStack> itemStacks = new SyncList<ItemStack>();

        public ItemStack this[int index] {
            get => itemStacks[index]; set => itemStacks[index] = value;
        }

        public override void OnStartNetwork() {
            OnStarted?.Invoke();
            itemStacks.OnChange += InternalOnItemStacksChanged;
        }

        public override void OnStopNetwork() {
            itemStacks.OnChange -= InternalOnItemStacksChanged;
            OnStopped?.Invoke();
        }

        private void InternalOnItemStacksChanged(SyncListOperation operation, int index, ItemStack oldItemStack, ItemStack newItemStack, bool asServer) {
            // Only call for the server side FIRST or for the client
            ContainerEventType? eventType = operation.ToContainerEventType();
            if (eventType != null && (IsClientOnlyInitialized || asServer))
                OnUpdate?.Invoke(eventType.Value, oldItemStack, index);
        }

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