using Sacados.Items;

namespace Sacados {

    /// <inheritdoc cref="ISlot"/>
    /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
    public interface ISlot<T> : ISlot, IStackContainer<T> where T : ItemStack {

        /// <inheritdoc cref="ISlot.ItemStack"/>
        new T ItemStack { get; set; }

        /// <inheritdoc cref="ISlot.GetMaxStackSize(ItemStack)"/>
        uint GetMaxStackSize(T itemStack);

    }

    /// <summary>
    /// Represents a <see cref="ISlot"/> that contains a single <see cref="ItemStack"/>
    /// </summary>
    public interface ISlot : IStackContainer {

        /// <summary>
        /// Index of the <see cref="ISlot"/> and it's <see cref="ItemStack"/>
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Determines the <see cref="ItemStack"/> that is contained in the <see cref="ISlot"/><br/>
        /// Setting the <see cref="ItemStack"/> through this property makes every validations with the <see cref="ISlot"/>
        /// </summary>
        public ItemStack ItemStack { get; set; }

        /// <summary>
        /// Determines the max stack size of the <see cref="ItemStack"/> in the <see cref="ISlot"/>
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that might be given</param>
        /// <returns>The max stack size of the <see cref="ItemStack"/> that the <see cref="ISlot"/> accepts</returns>
        uint GetMaxStackSize(ItemStack itemStack);

    }

}
