using System;

namespace Sacados {

    /// <inheritdoc cref="Slot"/>
    /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
    public class Slot<T> : Slot, ISlot<T> where T : ItemStack {

        /// <inheritdoc cref="Slot(IContainer, int, uint)"/>
        public Slot(IContainer<T> container, int index, uint maxStackSize = 0) : base(container, index, maxStackSize) {

        }

        public new T ItemStack { get => (T)base.ItemStack; set => base.ItemStack = value; }

        // User implementation
        public virtual uint GetMaxStackSize(T itemStack) => base.GetMaxStackSize(itemStack);

        public virtual void Give(T itemStack) => base.Give(itemStack);
        public virtual void Take(T itemStack) => base.Take(itemStack);

        // By default accept to give and take any ItemStack
        public virtual bool CanBeGiven(T itemStack) => true;
        public virtual bool CanBeTaken(T itemStack) => true;

        // Converts the ItemStack to T and call the new methods
        public override uint GetMaxStackSize(ItemStack itemStack)
            => itemStack == null ? GetMaxStackSize(null) : itemStack is T otherItemStack ? GetMaxStackSize(otherItemStack) : 0;

        public override void Give(ItemStack itemStack) {
            if (itemStack is T)
                base.Give(itemStack);
        }
        public override void Take(ItemStack itemStack) {
            if (itemStack is T)
                base.Take(itemStack);
        }

        public override bool CanBeGiven(ItemStack itemStack)
            => itemStack == null ? CanBeGiven(null) : itemStack is T otherItemStack ? CanBeGiven(otherItemStack) : false;
        public override bool CanBeTaken(ItemStack itemStack)
            => itemStack == null ? CanBeTaken(null) : itemStack is T otherItemStack ? CanBeTaken(otherItemStack) : false;

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

        public virtual uint GetMaxStackSize(ItemStack itemStack) => maxStackSize == 0 && !itemStack.IsEmpty() ? itemStack.Item.MaxStackSize : maxStackSize;

        public virtual ItemStack ItemStack {
            get => container[Index]; set {

                // If the ItemStack can't be set
                if (!CanBeGiven(value)) return;

                // If the ItemStack is empty then just set it
                if (value.IsEmpty()) container[Index] = null;
                else {

                    // Transfer the maximum stack size (don't forget to clone the original stack)
                    uint transfer = Math.Min(GetMaxStackSize(value), value.StackSize);
                    ItemStack stack = value.Clone();
                    stack.StackSize = transfer;
                    value.StackSize -= transfer;
                    container[Index] = stack;

                }

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