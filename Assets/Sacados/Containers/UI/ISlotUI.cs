namespace Sacados {

    /// <summary>
    /// Represents a <see cref="ISlotUI"/> that will display a specific <see cref="ItemStack"/>
    /// </summary>
    public interface ISlotUI {

        /// <summary>
        /// Configures the <see cref="ISlotUI"/> for the specified <see cref="IContainer"/> and index
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> from which the <see cref="ItemStack"/> will be gathered</param>
        /// <param name="index">The index of the <see cref="ISlot"/> that is binded with the <see cref="ISlotUI"/></param>
        void Configure(IContainer container, int index);

        /// <summary>
        /// Destroys the <see cref="ISlotUI"/>
        /// </summary>
        void Destroy();

        /// <summary>
        /// Refreshes the <see cref="ISlotUI"/> after a change of its <see cref="ItemStack"/>
        /// </summary>
        void Refresh();

    }

}
