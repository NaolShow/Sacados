using Sacados.Items;
using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace Sacados {

    /// <summary>
    /// Represents a <see cref="IContainer{T}"/> that contains multiple <see cref="ISlot{T}"/> and <see cref="ItemStack{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of <see cref="ItemStack{T}"/></typeparam>
    public interface IContainer<T> : IEnumerable<T>, IStackContainer<T> where T : ItemStack {

        /// <summary>
        /// Sets or gets the <see cref="ItemStack"/> at the i-th position in the <see cref="IContainer{T}"/><br/>
        /// <br/>
        /// Setting the <see cref="ItemStack"/> through this property doesn't makes any validation with it's <see cref="ISlot{T}"/><br/>
        /// If you want to set an <see cref="ItemStack"/> and do the <see cref="ISlot{T}"/> validation then set it through the <see cref="ISlot{T}.ItemStack"/>
        /// </summary>
        /// <param name="i">The position where we want the <see cref="ItemStack"/></param>
        /// <returns>Reference of the <see cref="ItemStack"/></returns>
        T this[int i] { get; set; }

        /// <summary>
        /// Determines all the <see cref="ISlot{T}"/> of the <see cref="IContainer{T}"/>
        /// </summary>
        IReadOnlyList<ISlot<T>> Slots { get; }

        /// <summary>
        /// Called when any operations about <see cref="ItemStack"/> occurs on the <see cref="IContainer{T}"/>
        /// </summary>
        event Action<NetworkListEvent<T>> OnChanged;

    }

}