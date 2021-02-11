using Mirror;
using Sacados.Containers;
using Sacados.Items;
using System;

namespace Sacados.Slots {

    public class Slot {

        #region Container

        public readonly Container Container;
        public int Index => Container.Slots.IndexOf(this);

        #endregion

        /// <summary>
        /// The slot's ItemStack, when setting an ItemStack to this variable, no checks are done
        /// </summary>
        protected virtual ItemStack ItemStack {
            get { return Container.ItemStacks[Index]; }
            set { Container.ItemStacks[Index] = value; }
        }

        public Slot(Container container) {

            // Save the slot container
            Container = container;

        }

        /// <summary>
        /// Determines if the ItemStack is accepted (=filtered) in the slot
        /// </summary>
        [Server]
        public virtual bool IsFiltered(ItemStack itemStack) => true;

        #region Management

        /// <summary>
        /// Sets the ItemStack (and overwrites the current slot ItemStack) to the slot and returns the surplus (if there is)
        /// </summary>
        [Server]
        public virtual ItemStack Set(ItemStack itemStack) {

            // If the ItemStack is empty
            if (itemStack.IsEmpty) {

                // Set the slot ItemStack to an empty one
                ItemStack = ItemStack.Empty;

                // Call the on SlotEmpty method
                Container.OnSlotEmpty(this);

                return itemStack;
            }

            // If the itemstack is not filtered
            if (!IsFiltered(itemStack)) return itemStack;

            // Determines the transfer size
            uint transferSize = Math.Min(itemStack.StackSize, itemStack.Item.MaxStackSize);

            // Copy the ItemStack
            ItemStack copiedItemStack = itemStack;

            // Transfer the stacks
            copiedItemStack.StackSize = transferSize;
            itemStack.StackSize -= transferSize;

            // Set the slot ItemStack
            ItemStack = copiedItemStack;

            // If the slot is now full
            if (copiedItemStack.StackSize == copiedItemStack.Item.MaxStackSize) {

                // Call the on SlotOverflow method
                Container.OnSlotOverflow(this);

            }

            return itemStack;

        }

        /// <summary>
        /// Gives the ItemStack to the slot and returns the surplus (if there is)
        /// </summary>
        [Server]
        public virtual ItemStack Give(ItemStack itemStack) {

            #region Empty slot

            // Get the slot ItemStack
            ItemStack slotItemStack = ItemStack;

            // If the slot is empty
            if (slotItemStack.IsEmpty) {

                // Set the ItemStack
                return Set(itemStack);

            }

            #endregion

            // If the ItemStack is empty or isn't filtered or not the same as the slot Item
            if (itemStack.IsEmpty || !IsFiltered(itemStack) || !slotItemStack.IsSameAs(itemStack)) return itemStack;

            // Determines the transfer size
            uint transferSize = Math.Min(slotItemStack.Item.MaxStackSize - slotItemStack.StackSize, itemStack.StackSize);

            // Transfer the stacks
            slotItemStack.StackSize += transferSize;
            itemStack.StackSize -= transferSize;

            // Set the slot ItemStack
            ItemStack = slotItemStack;

            // If the slot is now full
            if (slotItemStack.StackSize == slotItemStack.Item.MaxStackSize) {

                // Call the on SlotOverflow method
                Container.OnSlotOverflow(this);

            }

            return itemStack;

        }

        /// <summary>
        /// Takes the ItemStack from the slot and returns the remaining (if there is)
        /// </summary>
        [Server]
        public virtual ItemStack Take(ItemStack itemStack) {

            // If the ItemStack is empty
            if (itemStack.IsEmpty) return itemStack;

            // Get the slot ItemStack
            ItemStack slotItemStack = ItemStack;

            // If the slot itemstack is empty or not the same as the itemstack item
            if (slotItemStack.IsEmpty || !slotItemStack.IsSameAs(itemStack)) return itemStack;

            // Determines the transfer size
            uint transferSize = Math.Min(slotItemStack.StackSize, itemStack.StackSize);

            // Transfer the stacks
            slotItemStack.StackSize -= transferSize;
            itemStack.StackSize -= transferSize;

            // Save the slot ItemStack
            ItemStack = slotItemStack;

            // If the slot is now empty
            if (slotItemStack.IsEmpty) {

                // Call the on SlotEmpty method
                Container.OnSlotEmpty(this);

            }

            return itemStack;

        }

        #endregion

    }

}