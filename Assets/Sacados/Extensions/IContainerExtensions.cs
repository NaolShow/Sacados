namespace Sacados {

    /// <summary>
    /// Extensions methods for the <see cref="IContainer"/> interface
    /// </summary>
    public static class IContainerExtensions {

        /// <summary>
        /// Determines the total number of <see cref="ItemStack"/> that could be taken from the <see cref="IContainer"/>
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> that contains the <see cref="ItemStack"/></param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be counted</param>
        /// <returns>The total number of <see cref="ItemStack"/> that is in the <see cref="IContainer"/></returns>
        public static ulong GetCount(this IContainer container, ItemStack itemStack) {

            ulong count = 0;
            if (container.CanBeTaken(itemStack))
                for (int i = 0; i < container.SlotsCount; i++)
                    count += container.Get(i).GetCount(itemStack);
            return count;

        }

        /// <summary>
        /// Determines the remaining space for <see cref="ItemStack"/> that could be given to the <see cref="IContainer"/>
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> that contains the <see cref="ItemStack"/></param>
        /// <param name="itemStack">The <see cref="ItemStack"/> used to calculate the remaining space</param>
        /// <returns>The remaining space for <see cref="ItemStack"/> in the <see cref="IContainer"/></returns>
        public static ulong GetSpace(this IContainer container, ItemStack itemStack) {

            ulong count = 0;
            if (container.CanBeGiven(itemStack))
                for (int i = 0; i < container.SlotsCount; i++)
                    count += container.Get(i).GetSpace(itemStack);
            return count;

        }

    }

}
