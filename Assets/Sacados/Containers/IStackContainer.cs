using Sacados.Items;

namespace Sacados {

    /// <summary>
    /// Represents a <see cref="IStackContainer{T}"/> that contains one or multiple <see cref="ItemStack{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
    public interface IStackContainer<T> where T : ItemStack {

        /// <summary>
        /// Gives the specified <see cref="ItemStack"/> to the <see cref="IStackContainer{T}"/>
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be given</param>
        /// <returns>The remaining <see cref="ItemStack"/> that couldn't be given</returns>
        void Give(T itemStack);
        /// <summary>
        /// Takes the specified <see cref="ItemStack"/> from the <see cref="IStackContainer{T}"/>
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be taken</param>
        /// <returns>The remaining <see cref="ItemStack"/> that couldn't be taken</returns>
        void Take(T itemStack);

        /// <summary>
        /// Clears the <see cref="IStackContainer{T}"/> content
        /// </summary>
        void Clear();

    }

}