using Sacados.Items;

namespace Sacados {

    /// <inheritdoc cref="IStackContainer"/>
    /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
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
    /// Represents a <see cref="IStackContainer{T}"/> that contains one or multiple <see cref="ItemStack{T}"/>
    /// </summary>
    public interface IStackContainer {

        /// <summary>
        /// Gives the specified <see cref="ItemStack"/> to the <see cref="IStackContainer"/>
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be given</param>
        /// <returns>The remaining <see cref="ItemStack"/> that couldn't be given</returns>
        void Give(ItemStack itemStack);
        /// <summary>
        /// Takes the specified <see cref="ItemStack"/> from the <see cref="IStackContainer"/>
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be taken</param>
        /// <returns>The remaining <see cref="ItemStack"/> that couldn't be taken</returns>
        void Take(ItemStack itemStack);

        /// <summary>
        /// Determines if the <see cref="ItemStack"/> can be given to the <see cref="IStackContainer"/>
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that might be given</param>
        /// <returns>True if the <see cref="ItemStack"/> can be given, false otherwise</returns>
        bool CanBeGiven(ItemStack itemStack);
        /// <summary>
        /// Determines if the <see cref="ItemStack"/> can be taken from the <see cref="IStackContainer"/>
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that might be taken</param>
        /// <returns>True if the <see cref="ItemStack"/> can be taken, false otherwise</returns>
        bool CanBeTaken(ItemStack itemStack);

        /// <summary>
        /// Clears the <see cref="IStackContainer"/> content
        /// </summary>
        void Clear();

    }

}