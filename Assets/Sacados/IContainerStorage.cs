using System;
using System.Collections.Generic;

namespace Sacados {

    public interface IContainerStorage : IList<ItemStack> {

        bool IsReady { get; }

        /// <summary>
        /// Called when any operations about <see cref="ItemStack"/> occurs in the <see cref="IContainer"/>
        /// </summary>
        event OnContainerUpdateDelegate OnUpdate;
        public delegate void OnContainerUpdateDelegate(ContainerEventType type, ItemStack oldItemStack, int index);

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