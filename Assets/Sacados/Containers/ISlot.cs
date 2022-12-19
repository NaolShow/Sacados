using Sacados.Items;

namespace Sacados {

    /// <summary>
    /// Represents a <see cref="ISlot{T}"/> that contains a single <see cref="ItemStack"/>
    /// </summary>
    /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
    public interface ISlot<T> : IStackContainer<T> where T : ItemStack {

        /// <summary>
        /// Index of the <see cref="ISlot{T}"/> and it's <see cref="ItemStack"/>
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Determines the contained <see cref="ItemStack"/> of the <see cref="ISlot{T}"/><br/>
        /// <br/>
        /// Setting the <see cref="ItemStack"/> through this property will make every validation with the <see cref="Slot{T}"/>
        /// </summary>
        public T ItemStack { get; set; }

        /// <summary>
        /// Determines if the specified <see cref="ItemStack"/> can be contained in the <see cref="ISlot{T}"/>
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that might be contained in the <see cref="ISlot{T}"/></param>
        /// <returns>True if the <see cref="ItemStack"/> can be contained, otherwise false</returns>
        bool CanBeGiven(ItemStack itemStack);
        /// <summary>
        /// Determines if the <see cref="ISlot{T}"/> can have it's <see cref="ItemStack"/> taken
        /// </summary>
        bool CanBeTaken { get; set; }

        /// <summary>
        /// Determines the max stack size of the <see cref="ISlot{T}"/> for the specified <see cref="ItemStack"/>
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that might be given to the <see cref="ISlot{T}"/></param>
        /// <returns>The max stack size that the <see cref="ISlot{T}"/> accepts for the specified <see cref="ItemStack"/></returns>
        uint GetMaxStackSize(T itemStack);

    }

}
