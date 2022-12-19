using Sacados.Items;

namespace Sacados {

    /// <summary>
    /// Extensions methods for the <see cref="ISlot{T}"/> interface
    /// </summary>
    public static class ISlotExtensions {

        /// <summary>
        /// Determines the total number of <see cref="ItemStack"/> that could be taken from the <see cref="ISlot{T}"/>
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
        /// <param name="slot">The <see cref="ISlot{T}"/> that contains the <see cref="ItemStack"/></param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be counted</param>
        /// <returns>The total number of <see cref="ItemStack"/> that is in the <see cref="ISlot{T}"/></returns>
        public static ulong GetCount<T>(this ISlot<T> slot, T itemStack) where T : ItemStack
            => (slot.ItemStack.IsEmpty() || !slot.ItemStack.IsSameAs(itemStack)) ? 0 : slot.ItemStack.StackSize;

        /// <summary>
        /// Determines the remaining space for <see cref="ItemStack"/> that could be given to the <see cref="ISlot{T}"/>
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
        /// <param name="slot">The <see cref="ISlot{T}"/> that contains the <see cref="ItemStack"/></param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will get it's remaining space counted</param>
        /// <returns>The remaining space for <see cref="ItemStack"/> in the <see cref="ISlot{T}"/></returns>
        public static ulong GetSpace<T>(this ISlot<T> slot, T itemStack) where T : ItemStack
            => slot.ItemStack.IsEmpty() ? slot.GetMaxStackSize(itemStack) : slot.ItemStack.IsSameAs(itemStack) ? slot.GetMaxStackSize(itemStack) - slot.ItemStack.StackSize : 0;

        /// <summary>
        /// Determines if the <see cref="ISlot{T}"/> has enough space to be given the <see cref="ItemStack"/>
        /// </summary>
        /// <param name="slot">The <see cref="ISlot{T}"/> to which the <see cref="ItemStack"/> might be given</param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that could be given to the <see cref="ISlot{T}"/></param>
        /// <returns>True if there is enough space for the <see cref="ItemStack"/> in the <see cref="ISlot{T}"/> to be given, false otherwise</returns>
        public static bool HasEnoughSpace<T>(this ISlot<T> slot, T itemStack) where T : ItemStack => slot.GetSpace(itemStack) >= itemStack.StackSize;

        /// <summary>
        /// Determines if the <see cref="ISlot{T}"/> has enough <see cref="ItemStack"/> to be taken
        /// </summary>
        /// <param name="slot">The <see cref="ISlot{T}"/> from which the <see cref="ItemStack"/> might be taken</param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that could be taken from the <see cref="ISlot{T}"/></param>
        /// <returns>True if there is enough <see cref="ItemStack"/> in the <see cref="ISlot{T}"/> to be taken, false otherwise</returns>
        public static bool HasEnoughCount<T>(this ISlot<T> slot, T itemStack) where T : ItemStack => slot.GetCount(itemStack) >= itemStack.StackSize;

        /// <summary>
        /// Swaps <see cref="ItemStack"/> of the two specified <see cref="ISlot{T}"/>
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
        /// <param name="slot">The first <see cref="ISlot{T}"/></param>
        /// <param name="otherSlot">The second <see cref="ISlot{T}"/></param>
        /// <returns>True if both <see cref="ISlot{T}"/> could have their <see cref="ItemStack"/> taken and are both compatible with them, otherwise false</returns>
        public static bool Swap<T>(this ISlot<T> slot, ISlot<T> otherSlot) where T : ItemStack {

            T itemStack = slot.ItemStack;
            T otherItemStack = otherSlot.ItemStack;

            // If both slot can have their itemstack taken and both slots accepts each other's itemstack then swap
            if (slot.CanBeTaken && otherSlot.CanBeTaken && slot.CanBeGiven(otherItemStack) && otherSlot.CanBeGiven(itemStack)) {
                otherSlot.ItemStack = itemStack;
                slot.ItemStack = otherItemStack;
                return true;
            }
            return false;

        }

    }

}
