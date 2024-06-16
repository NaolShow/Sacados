using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sacados.IContainerStorage;

namespace Sacados {

    /// <summary>
    /// Represents a storage for a <see cref="Container"/> that only resides locally
    /// </summary>
    public class LocalContainerStorage : MonoBehaviour, IContainerStorage {

        public int Count => itemStacks.Count;
        public bool IsReady => true;
        public bool IsReadOnly => false;

        public event OnContainerUpdateDelegate OnUpdate;
        public event Action OnStarted;
        public event Action OnStopped;

        private List<ItemStack> itemStacks = new List<ItemStack>();

        private bool wasUpdated = false;
        private void InvokeUpdate(ContainerEventType type, ItemStack oldItemStack, int index) {
            OnUpdate?.Invoke(type, oldItemStack, index);
            wasUpdated = true;
        }

        private void LateUpdate() {
            if (!wasUpdated) return;
            wasUpdated = false;
            OnUpdate?.Invoke(ContainerEventType.Full, null, -1);
        }

        public ItemStack this[int index] {
            get => itemStacks[index]; set {
                ItemStack oldItemStack = itemStacks[index];
                itemStacks[index] = value;
                InvokeUpdate(ContainerEventType.Value, oldItemStack, index);
            }
        }

        private void Start() {
            OnStarted?.Invoke();
        }

        private void OnDestroy() {
            OnStopped?.Invoke();
        }

        public void Add(ItemStack itemStack) {
            int index = itemStacks.Count;
            itemStacks.Add(itemStack);
            InvokeUpdate(ContainerEventType.Add, null, index);
        }

        public void Insert(int index, ItemStack itemStack) {
            itemStacks.Insert(index, itemStack);
            InvokeUpdate(ContainerEventType.Add, null, index);
        }

        public bool Remove(ItemStack itemStack) {
            int index = itemStacks.IndexOf(itemStack);
            if (index == -1) return false;
            itemStacks.Remove(itemStack);
            InvokeUpdate(ContainerEventType.Remove, itemStack, index);
            return true;

        }

        public void RemoveAt(int index) {
            ItemStack oldItemStack = itemStacks[index];
            itemStacks.RemoveAt(index);
            InvokeUpdate(ContainerEventType.Remove, oldItemStack, index);
        }

        public void Clear() {
            itemStacks.Clear();
            InvokeUpdate(ContainerEventType.Clear, null, -1);
        }

        public bool Contains(ItemStack itemStack) => itemStacks.Contains(itemStack);
        public int IndexOf(ItemStack itemStack) => itemStacks.IndexOf(itemStack);
        public void CopyTo(ItemStack[] array, int arrayIndex) => itemStacks.CopyTo(array, arrayIndex);

        public IEnumerator<ItemStack> GetEnumerator() => itemStacks.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => itemStacks.GetEnumerator();

    }
}