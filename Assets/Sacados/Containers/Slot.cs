using Sacados.Items;
using System;

namespace Sacados {

    /// <summary>
    /// Provides a basic implementation of <see cref="ISlot{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
    public class Slot<T> : ISlot<T> where T : ItemStack {

        public int Index { get; set; }

        private readonly IContainer<T> container;
        private readonly uint maxStackSize;

        /// <summary>
        /// Initializes a <see cref="Slot{T}"/> for the specified <see cref="IContainer{T}"/> with the specified <see cref="Index"/> and max stack size
        /// </summary>
        /// <param name="container">The <see cref="IContainer{T}"/> that the <see cref="Slot{T}"/> belongs to</param>
        /// <param name="index">The <see cref="Index"/> of the <see cref="Slot{T}"/></param>
        /// <param name="maxStackSize">The max stack size of the <see cref="Slot{T}"/> (set it to 0 to not override <see cref="Item.MaxStackSize"/>)</param>
        public Slot(IContainer<T> container, int index, uint maxStackSize = 0) {
            this.container = container;
            Index = index;
            this.maxStackSize = maxStackSize;
        }

        public T ItemStack {
            get => container[Index]; set {

                // If we cannot contain the ItemStack
                if (!CanBeGiven(value)) return;

                // If the ItemStack is empty then just set it
                if (value.IsEmpty()) container[Index] = null;
                else {

                    uint transfer = Math.Min(GetMaxStackSize(value), value.StackSize);

                    T stack = (T)value.Clone();
                    stack.StackSize = transfer;
                    value.StackSize -= transfer;
                    container[Index] = stack;

                }

            }
        }

        // Accept to give and take any items
        public virtual bool CanBeGiven(ItemStack itemStack) => true;
        public virtual bool CanBeTaken { get; set; } = true;

        public void Give(T itemStack) {

            // If this ItemStack isn't authorized to be in this slot or there is no more space
            if (!CanBeGiven(itemStack) || this.GetSpace(itemStack) == 0) return;

            // If the slot's ItemStack is empty then just set it
            if (ItemStack.IsEmpty())
                ItemStack = itemStack;
            // If the slot's ItemStack is the same
            else if (ItemStack.IsSameAs(itemStack)) {

                uint toTransfer = Math.Min(itemStack.StackSize, GetMaxStackSize(itemStack) - ItemStack.StackSize);
                itemStack.StackSize -= toTransfer;
                ItemStack.StackSize += toTransfer;
                MarkDirty();

            }

        }

        public void Take(T itemStack) {

            // If the slot ItemStack cannot be taken or either the slot's or specified ItemStack is empty or not the same or there is no item stack
            if (!CanBeTaken || ItemStack.IsEmpty() || itemStack.IsEmpty() || !ItemStack.IsSameAs(itemStack) || this.GetCount(itemStack) == 0) return;

            uint toTransfer = Math.Min(itemStack.StackSize, ItemStack.StackSize);
            itemStack.StackSize -= toTransfer;
            ItemStack.StackSize -= toTransfer;
            MarkDirty();

        }

        public void Clear() => ItemStack = null;

        public uint GetMaxStackSize(T itemStack) => maxStackSize == 0 ? itemStack.Item.MaxStackSize : maxStackSize;

        // Mark the slot as dirty by just setting back it's itemstack to the same value
        // TODO: Maybe put this in the ISlot interface so that we can update it from outside?
        private void MarkDirty() => container[Index] = ItemStack;

    }

}