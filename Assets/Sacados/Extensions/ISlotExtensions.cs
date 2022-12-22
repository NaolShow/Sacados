using Sacados.Items;

namespace Sacados {

    /// <summary>
    /// Extensions methods for the <see cref="ISlot{T}"/> interface
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
            ItemStack slotItemStack = slot.ItemStack;
            if (!slotItemStack.IsEmpty() && slotItemStack.IsSameAs(itemStack))
                count = slotItemStack.StackSize;

            return count;

        }

        /// <summary>
        /// Determines the remaining space for <see cref="ItemStack"/> that could be given to the <see cref="ISlot"/>
        /// </summary>
        /// <param name="slot">The <see cref="ISlot"/> that contains the <see cref="ItemStack"/></param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will get it's remaining space counted</param>
        /// <returns>The remaining space for <see cref="ItemStack"/> in the <see cref="ISlot"/></returns>
        public static uint GetSpace(this ISlot slot, ItemStack itemStack) {

            uint space = 0;

            // If the ItemStack can be given in the slot
            if (slot.CanBeGiven(itemStack)) {

                ItemStack slotItemStack = slot.ItemStack;
                space = slot.GetMaxStackSize(itemStack);

                // If the slot ItemStack isn't empty then decrease the stack sze
                if (!slotItemStack.IsEmpty()) space -= slotItemStack.StackSize;

            }
            return space;

        }

        /// <summary>
        /// Determines if the <see cref="ISlot"/> has enough space to be given the <see cref="ItemStack"/>
        /// </summary>
        /// <param name="slot">The <see cref="ISlot"/> to which the <see cref="ItemStack"/> might be given</param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that could be given to the <see cref="ISlot"/></param>
        /// <returns>True if there is enough space for the <see cref="ItemStack"/> in the <see cref="ISlot"/> to be given, false otherwise</returns>
        public static bool HasEnoughSpace(this ISlot slot, ItemStack itemStack) => slot.GetSpace(itemStack) >= itemStack.StackSize;

        /// <summary>
        /// Determines if the <see cref="ISlot"/> has enough <see cref="ItemStack"/> to be taken
        /// </summary>
        /// <param name="slot">The <see cref="ISlot"/> from which the <see cref="ItemStack"/> might be taken</param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that could be taken from the <see cref="ISlot"/></param>
        /// <returns>True if there is enough <see cref="ItemStack"/> in the <see cref="ISlot"/> to be taken, false otherwise</returns>
        public static bool HasEnoughCount(this ISlot slot, ItemStack itemStack) => slot.GetCount(itemStack) >= itemStack.StackSize;

        /// <summary>
        /// Swaps <see cref="ItemStack"/> of the two specified <see cref="ISlot"/>
        /// </summary>
        /// <param name="slot">The first <see cref="ISlot"/></param>
        /// <param name="otherSlot">The second <see cref="ISlot"/></param>
        /// <returns>True if both <see cref="ISlot"/> could have their <see cref="ItemStack"/> taken and are both compatible with them, otherwise false</returns>
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
