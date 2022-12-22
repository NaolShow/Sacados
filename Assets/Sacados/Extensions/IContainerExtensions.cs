using Sacados.Items;

namespace Sacados {

    /// <summary>
    /// Extensions methods for the <see cref="IContainer{T}"/> interface
    /// </summary>
    public static class IContainerExtensions {

        /// <summary>
        /// Determines the total number of <see cref="ItemStack"/> that could be taken from the <see cref="IContainer{T}"/>
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
        /// <param name="container">The <see cref="IContainer{T}"/> that contains the <see cref="ItemStack"/></param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be counted</param>
        /// <returns>The total number of <see cref="ItemStack"/> that is in the <see cref="IContainer{T}"/></returns>
        public static ulong GetCount(this IContainer container, ItemStack itemStack) {

            ulong count = 0;
            for (int i = 0; i < container.SlotsCount; i++)
                count += container.Get(i).GetCount(itemStack);
            return count;

        }

        /// <summary>
        /// Determines the remaining space for <see cref="ItemStack"/> that could be given to the <see cref="IContainer{T}"/>
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
        /// <param name="container">The <see cref="IContainer{T}"/> that contains the <see cref="ItemStack"/></param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will get it's remaining space counted</param>
        /// <returns>The remaining space for <see cref="ItemStack"/> in the <see cref="IContainer{T}"/></returns>
        public static ulong GetSpace(this IContainer container, ItemStack itemStack) {

            ulong count = 0;
            for (int i = 0; i < container.SlotsCount; i++)
                count += container.Get(i).GetSpace(itemStack);
            return count;

        }

        /// <summary>
        /// Determines if the specified <see cref="ItemStack"/> can be given to the <see cref="IStackContainer{T}"/>
        /// </summary>
        /// <param name="container">The <see cref="IStackContainer{T}"/> to which the <see cref="ItemStack"/> might be given</param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that could be given to the <see cref="IStackContainer{T}"/></param>
        /// <returns>True if there is enough space for the <see cref="ItemStack"/> in the <see cref="IStackContainer{T}"/> to be given, false otherwise</returns>
        public static bool HasEnoughSpace<T>(this IContainer<T> container, T itemStack) where T : ItemStack => container.GetSpace(itemStack) >= itemStack.StackSize;

        /// <summary>
        /// Determines if the specified <see cref="ItemStack"/> can be taken from the <see cref="IStackContainer{T}"/>
        /// </summary>
        /// <param name="container">The <see cref="IStackContainer{T}"/> from which the <see cref="ItemStack"/> might be taken</param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that could be taken from the <see cref="IStackContainer{T}"/></param>
        /// <returns>True if there is enough <see cref="ItemStack"/> in the <see cref="IStackContainer{T}"/> to be taken, false otherwise</returns>
        public static bool HasEnoughCount<T>(this IContainer<T> container, T itemStack) where T : ItemStack => container.GetCount(itemStack) >= itemStack.StackSize;

    }

}
