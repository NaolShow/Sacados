using System;

namespace Sacados {

    /// <inheritdoc cref="Slot"/>
    /// <typeparam name="T">Type of <see cref="Sacados.ItemStack"/></typeparam>
    public class Slot<T> : Slot, ISlot<T> where T : ItemStack {

        /// <inheritdoc cref="Slot(IContainer, int, uint)"/>
        public Slot(IContainer<T> container, int index, uint maxStackSize = 0) : base(container, index, maxStackSize) {

        }

        public virtual new T ItemStack { get => (T)base.Get(); set => base.Set(value); }

        // User implementation
        public virtual uint GetMaximumSpace(T itemStack) => base.GetMaximumSpace(itemStack);

        public virtual void Give(T itemStack) => base.Give(itemStack);
        public virtual void Take(T itemStack) => base.Take(itemStack);

        // By default accept to give and take any ItemStack
        public virtual bool CanBeGiven(T itemStack) => true;
        public virtual bool CanBeTaken(T itemStack) => true;

        // Converts the ItemStack to T and call the new methods
        protected sealed override ItemStack Get() => base.Get();
        protected sealed override void Set(ItemStack itemStack) {

            // If the value is null then set it or if it's a T then set it
            if (itemStack == null) ItemStack = null;
            else if (itemStack is T otherItemStack) ItemStack = otherItemStack;

        }
        public sealed override uint GetMaximumSpace(ItemStack itemStack)
            => itemStack == null ? GetMaximumSpace(null) : itemStack is T otherItemStack ? GetMaximumSpace(otherItemStack) : 0;

        public sealed override void Give(ItemStack itemStack) {
            if (itemStack is T otherItemStack)
                Give(otherItemStack);
        }
        public sealed override void Take(ItemStack itemStack) {
            if (itemStack is T otherItemStack)
                Take(otherItemStack);
        }

        public sealed override bool CanBeGiven(ItemStack itemStack)
            => itemStack == null ? CanBeGiven(null) : itemStack is T otherItemStack && CanBeGiven(otherItemStack);
        public sealed override bool CanBeTaken(ItemStack itemStack)
            => itemStack == null ? CanBeTaken(null) : itemStack is T otherItemStack && CanBeTaken(otherItemStack);

    }

    /// <summary>
    /// Basic implementation of <see cref="ISlot"/>
    /// </summary>
    public class Slot : ISlot {

        public int Index { get; set; }

        private readonly IContainer container;
        private readonly uint maxStackSize;

        /// <summary>
        /// Initializes a <see cref="Slot"/> for the specified <see cref="IContainer"/> with the specified <see cref="Index"/> and max stack size
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> that the <see cref="Slot"/> belongs to</param>
        /// <param name="index">The <see cref="Index"/> of the <see cref="Slot"/></param>
        /// <param name="maxStackSize">The max stack size of the <see cref="Slot"/> (set it to 0 to not override the <see cref="Item.MaxStackSize"/>)</param>
        public Slot(IContainer container, int index, uint maxStackSize = 0) {
            this.container = container;
            Index = index;
            this.maxStackSize = maxStackSize;
        }

        public virtual uint GetMaximumSpace(ItemStack itemStack) => maxStackSize == 0 && !itemStack.IsEmpty() ? itemStack.Item.MaxStackSize : maxStackSize;

        // Can't override this and have at the same time new T ItemStack
        // => To go around this issue, just use two methods for the property
        public virtual ItemStack ItemStack { get => Get(); set => Set(value); }

        /// <inheritdoc cref="ItemStack"/>
        protected virtual ItemStack Get() => container[Index];
        /// <inheritdoc cref="ItemStack"/>
        protected virtual void Set(ItemStack itemStack) {

            // If the ItemStack can't be set
            if (!CanBeGiven(itemStack)) return;

            // If the ItemStack is empty then just set it
            if (itemStack.IsEmpty()) container[Index] = null;
            else {

                // Transfer the maximum stack size (don't forget to clone the original stack)
                uint transfer = Math.Min(GetMaximumSpace(itemStack), itemStack.StackSize);
                ItemStack stack = itemStack.Clone();
                stack.StackSize = transfer;
                itemStack.StackSize -= transfer;
                container[Index] = stack;

            }

        }

        // By default accept to give and take any ItemStack
        public virtual bool CanBeGiven(ItemStack itemStack) => true;
        public virtual bool CanBeTaken(ItemStack itemStack) => true;

        public virtual void Give(ItemStack itemStack) {

            // Get the slot's ItemStack
            ItemStack slotItemStack = ItemStack;

            // If the slot's ItemStack is empty then just set it
            if (slotItemStack.IsEmpty()) ItemStack = itemStack;
            else {

                // If there is no space for the ItemStack in the slot
                uint space = this.GetSpace(itemStack);
                if (space == 0) return;

                // Transfer the maximum stack size
                uint toTransfer = Math.Min(itemStack.StackSize, space);
                itemStack.StackSize -= toTransfer;
                slotItemStack.StackSize += toTransfer;
                MarkDirty();

            }

        }

        public virtual void Take(ItemStack itemStack) {

            // If the ItemStack can't be taken from the slot
            if (!CanBeTaken(itemStack)) return;

            // If there is nothing to take from the slot
            uint count = this.GetCount(itemStack);
            if (count == 0) return;

            // Transfer the maximum stacksize
            uint toTransfer = Math.Min(itemStack.StackSize, ItemStack.StackSize);
            itemStack.StackSize -= toTransfer;
            ItemStack.StackSize -= toTransfer;
            MarkDirty();

        }

        public virtual void Clear() => ItemStack = null;

        // Mark the slot as dirty by just setting back it's itemstack to the same value
        // TODO: Maybe put this in the ISlot interface so that we can update it from outside?
        protected void MarkDirty() => container[Index] = ItemStack;

    }

}