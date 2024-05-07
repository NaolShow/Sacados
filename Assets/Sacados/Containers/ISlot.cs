namespace Sacados {

    /// <summary>
    /// Represents a <see cref="ISlot"/> that contains a single <see cref="Sacados.ItemStack"/>
    /// </summary>
    public interface ISlot : IStackContainer {

        /// <summary>
        /// Index of the <see cref="ISlot"/> and it's <see cref="Sacados.ItemStack"/>
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Determines the <see cref="Sacados.ItemStack"/> that is contained in the <see cref="ISlot"/><br/>
        /// Setting the <see cref="Sacados.ItemStack"/> through this property makes every validations with the <see cref="ISlot"/>
        /// </summary>
        public ItemStack ItemStack { get; set; }

        /// <summary>
        /// Determines the maximum space available in the <see cref="ISlot"/> for the specified <see cref="Sacados.ItemStack"/><br/>
        /// This method doesn't act as a filter and can give a maximum space even if the <see cref="Sacados.ItemStack"/> cannot be given to the slot
        /// </summary>
        /// <param name="itemStack">The <see cref="Sacados.ItemStack"/> that could be given to the <see cref="ISlot"/></param>
        /// <returns>The max stack size of the <see cref="Sacados.ItemStack"/> that the <see cref="ISlot"/> accepts</returns>
        uint GetMaximumSpace(ItemStack itemStack);

        /// <summary>
        /// Synchronizes the <see cref="ISlot"/> content over the network (useful in case some <see cref="ItemStack"/>'s values have changed)
        /// </summary>
        void Synchronize();

    }

}
