using Sacados.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

namespace Sacados {

    /// <summary>
    /// Provides a basic implementation of <see cref="IContainer{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
    public abstract class Container<T> : NetworkBehaviour, IContainer<T> where T : ItemStack, new() {

        public T this[int i] { get => itemStacks[i]; set => itemStacks[i] = value; }
        private NetworkStandardList<T, ItemStack> itemStacks = new NetworkStandardList<T, ItemStack>();

        public IReadOnlyList<ISlot<T>> Slots => slots;
        private List<ISlot<T>> slots = new List<ISlot<T>>();

        public event Action<NetworkListEvent<T>> OnChanged;

        // Subscribe to the ItemStack's list changed event
        // => Doesn't need to unsubscribe since the list lifetime = container's lifetime
        public override void OnNetworkSpawn() => itemStacks.OnListChanged += e => OnChanged?.Invoke(e);

        /// <summary>
        /// Adds the specified <see cref="ISlot{T}"/> to the <see cref="Container{T}"/>
        /// </summary>
        /// <param name="slot">The <see cref="ISlot{T}"/> that will be added</param>
        protected void AddSlot(ISlot<T> slot) {

            // Insert the slot and if on the server the ItemStack too
            slots.Insert(slot.Index, slot);
            if (IsServer)
                itemStacks.Insert(slot.Index, null);

            // Reorder the slots indexes only if the added slot isn't at the end
            for (int i = slot.Index; i < slots.Count; i++)
                slots[i].Index = i;

        }

        /// <summary>
        /// Removes the <see cref="ISlot{T}"/> at the specified index from the <see cref="Container{T}"/>
        /// </summary>
        /// <param name="index">The index of the <see cref="ISlot{T}"/></param>
        protected void RemoveSlot(int index) {

            // Remove the slot and if on the server the ItemStack too
            slots.RemoveAt(index);
            if (IsServer) itemStacks.RemoveAt(index);

            // Reorder the slots indexes only if the removed slot isn't at the end
            for (int i = index; i < slots.Count; i++)
                slots[i].Index = i;

        }

        // User implementation
        public abstract void Clear();
        public abstract void Give(T itemStack);
        public abstract void Take(T itemStack);

        // IEnumerable implementation
        public IEnumerator<T> GetEnumerator() => itemStacks.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}