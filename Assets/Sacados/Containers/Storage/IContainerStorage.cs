using System;
using System.Collections.Generic;

namespace Sacados {

    /// <summary>
    /// Represents a storage for a <see cref="Container"/> content
    /// </summary>
    public interface IContainerStorage : IList<ItemStack> {

        /// <summary>
        /// Determines if the <see cref="IContainerStorage"/> is ready or not
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// Called when any operations about <see cref="ItemStack"/> occurs in the <see cref="IContainer"/>
        /// </summary>
        event OnContainerUpdateDelegate OnUpdate;
        public delegate void OnContainerUpdateDelegate(ContainerEventType type, ItemStack oldItemStack, int index);

        /// <summary>
        /// Called when the <see cref="IContainer"/> started and is now ready to be used<br/>
        /// This is the right moment to load the previously saved <see cref="Container"/>'s content (if on server-side)
        /// </summary>
        event Action OnStarted;
        /// <summary>
        /// Called when the <see cref="IContainer"/> stopped and is no longer ready to be used
        /// </summary>
        event Action OnStopped;

    }

}