using System;

namespace Sacados {

    /// <inheritdoc cref="IContainer"/>
    /// <typeparam name="T">Type of <see cref="ItemStack"/></typeparam>
    public interface IContainer<T> : IContainer, IStackContainer<T> where T : ItemStack {

        /// <inheritdoc cref="IContainer.this"/>
        new T this[int i] { get; set; }

        /// <inheritdoc cref="IContainer.Get(int)"/>
        new ISlot<T> Get(int index);

    }

    /// <summary>
    /// Represents a <see cref="IContainer"/> that contains multiple <see cref="ISlot"/> and <see cref="ItemStack"/>
    /// </summary>
    public interface IContainer : IStackContainer {

        /// <summary>
        /// Determines the amount of <see cref="ISlot"/> present in the <see cref="IContainer"/>
        /// </summary>
        int SlotsCount { get; }

        /// <summary>
        /// Accessor to the <see cref="ItemStack"/> at the specified index in the <see cref="IContainer"/><br/>
        /// Setting the <see cref="ItemStack"/> through this property doesn't make any validation with its <see cref="ISlot"/>
        /// </summary>
        /// <param name="i">The i-th position of the <see cref="ItemStack"/></param>
        /// <returns>The <see cref="ItemStack"/> at the specified index</returns>
        ItemStack this[int i] { get; set; }

        /// <summary>
        /// Gets the <see cref="ISlot"/> at the specified index in the <see cref="IContainer"/>
        /// </summary>
        /// <param name="index">The index of the specified <see cref="ISlot"/></param>
        /// <returns>The <see cref="ISlot"/> at the specified index</returns>
        ISlot Get(int index);

        /// <summary>
        /// Called when any operations about <see cref="ItemStack"/> occurs in the <see cref="IContainer"/>
        /// </summary>
        event OnContainerUpdateDelegate OnUpdate;
        delegate void OnContainerUpdateDelegate(ContainerEventType type, int index);

        /// <summary>
        /// Called when the <see cref="IContainer"/> started and is now ready to be used
        /// </summary>
        event Action OnStarted;
        /// <summary>
        /// Called when the <see cref="IContainer"/> stopped and is no longer ready to be used
        /// </summary>
        event Action OnStopped;


    }

}