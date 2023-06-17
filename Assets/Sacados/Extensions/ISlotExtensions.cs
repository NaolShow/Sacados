namespace Sacados {

    /// <summary>
    /// Extensions methods for the <see cref="ISlot"/> interface
    /// </summary>
    public static class ISlotExtensions {

        /// <summary>
        /// Determines the total number of <see cref="ItemStack"/> that could be taken from the <see cref="ISlot"/>
        /// </summary>
        /// <param name="slot">The <see cref="ISlot"/> that contains the <see cref="ItemStack"/></param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be counted</param>
        /// <returns>The total number of <see cref="ItemStack"/> that is in the <see cref="ISlot"/></returns>
        public static uint GetCount(this ISlot slot, ItemStack itemStack) {

            uint count = 0;

            // If the slot's ItemStack is not empty and are the same then set the StackSize
            if (slot.CanBeTaken(itemStack)) {

                ItemStack slotItemStack = slot.ItemStack;
                if (!slotItemStack.IsEmpty() && slotItemStack.IsSameAs(itemStack))
                    count = slotItemStack.StackSize;

            }
            return count;

        }

        /// <summary>
        /// Determines the remaining space for <see cref="ItemStack"/> that could be given to the <see cref="ISlot"/>
        /// </summary>
        /// <param name="slot">The <see cref="ISlot"/> that contains the <see cref="ItemStack"/></param>
        /// <param name="itemStack">The <see cref="ItemStack"/> used to calculate the remaining space</param>
        /// <returns>The remaining space for <see cref="ItemStack"/> in the <see cref="ISlot"/></returns>
        public static uint GetSpace(this ISlot slot, ItemStack itemStack) {

            uint space = 0;

            // If the ItemStack can be given to the slot
            if (slot.CanBeGiven(itemStack)) {
                ItemStack slotItemStack = slot.ItemStack;

                // If the slot's ItemStack is empty then it's the max stack size
                if (slotItemStack.IsEmpty())
                    space = slot.GetMaxStackSize(itemStack);
                // If the ItemStacks are the same then it's the max stack size decreased by the current stack size
                else if (slotItemStack.IsSameAs(itemStack))
                    space = slot.GetMaxStackSize(itemStack) - slotItemStack.StackSize;

            }
            return space;

        }

        /// <summary>
        /// Swaps <see cref="ItemStack"/> of the two specified <see cref="ISlot"/>
        /// </summary>
        /// <param name="slot">The first <see cref="ISlot"/></param>
        /// <param name="otherSlot">The second <see cref="ISlot"/></param>
        /// <returns>True if both <see cref="ISlot"/> could have their <see cref="ItemStack"/> taken and given, otherwise false</returns>
        public static bool Swap(this ISlot slot, ISlot otherSlot) {

            ItemStack itemStack = slot.ItemStack;
            ItemStack otherItemStack = otherSlot.ItemStack;

            // If both slot can have their itemstack taken and both slots accepts each other's itemstack then swap
            if (slot.CanBeTaken(itemStack) && otherSlot.CanBeTaken(otherItemStack) && slot.CanBeGiven(otherItemStack) && otherSlot.CanBeGiven(itemStack)) {
                otherSlot.ItemStack = itemStack;
                slot.ItemStack = otherItemStack;
                return true;
            }
            return false;

        }

    }

}
