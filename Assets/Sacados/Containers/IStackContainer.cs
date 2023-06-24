namespace Sacados {

    /// <inheritdoc cref="IStackContainer"/>
    /// <typeparam name="T">Type of <see cref="ItemStack"/></typeparam>
    public interface IStackContainer<T> where T : ItemStack {

        /// <inheritdoc cref="IStackContainer.Give(ItemStack)"/>
        void Give(T itemStack);
        /// <inheritdoc cref="IStackContainer.Take(ItemStack)"/>
        void Take(T itemStack);

        /// <inheritdoc cref="IStackContainer.CanBeGiven(ItemStack)"/>
        bool CanBeGiven(T itemStack);
        /// <inheritdoc cref="IStackContainer.CanBeTaken(ItemStack)"/>
        bool CanBeTaken(T itemStack);

    }

    /// <summary>
    /// Represents a <see cref="IStackContainer"/> that contains one or multiple <see cref="ItemStack"/>
    /// </summary>
    public interface IStackContainer {

        /// <summary>
        /// Gives the specified <see cref="ItemStack"/> to the <see cref="IStackContainer"/> and substracts the given amount from the <see cref="ItemStack.StackSize"/>
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be givento the <see cref="IStackContainer"/></param>
        void Give(ItemStack itemStack);
        /// <summary>
        /// Takes the specified <see cref="ItemStack"/> from the <see cref="IStackContainer"/> and substracts the taken amount from the <see cref="ItemStack.StackSize"/>
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be taken</param>
        void Take(ItemStack itemStack);

        /// <summary>
        /// Determines if the <see cref="ItemStack"/> is allowed by the <see cref="IStackContainer"/> to be given to it<br/>
        /// This methods only acts as a filter and doesn't check if there is enough space for the <see cref="ItemStack"/> to be given
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that might be given</param>
        /// <returns>True if the <see cref="ItemStack"/> can be given, false otherwise</returns>
        bool CanBeGiven(ItemStack itemStack);
        /// <summary>
        /// Determines if the <see cref="ItemStack"/> is allowed by the <see cref="IStackContainer"/> to be taken from it<br/>
        /// This methods only acts as a filter and doesn't check if there is enough <see cref="ItemStack"/> to be taken
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that might be taken</param>
        /// <returns>True if the <see cref="ItemStack"/> can be taken, false otherwise</returns>
        bool CanBeTaken(ItemStack itemStack);

        /// <summary>
        /// Clears the <see cref="IStackContainer"/> contained <see cref="ItemStack"/>
        /// </summary>
        void Clear();

    }

}