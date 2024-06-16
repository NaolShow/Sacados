using FishNet.Object.Synchronizing;

namespace Sacados {

    /// <summary>
    /// Event type that occurs on the <see cref="IContainer"/>
    /// </summary>
    public enum ContainerEventType : byte {

        /// <summary>
        /// When the <see cref="IContainer"/> has a new <see cref="ISlot"/>
        /// </summary>
        Add,
        /// <summary>
        /// When the <see cref="IContainer"/> has lost a <see cref="ISlot"/>
        /// </summary>
        Remove,
        /// <summary>
        /// When the <see cref="IContainer"/> updated an <see cref="ISlot.ItemStack"/>
        /// </summary>
        Value,
        /// <summary>
        /// When the <see cref="IContainer"/> cleared all it's <see cref="ISlot"/>
        /// </summary>
        Clear,
        /// <summary>
        /// When the <see cref="IContainer"/> has been updated completely (after being updated by a batch of updates)
        /// </summary>
        Full

    }

    /// <summary>
    /// Extensions methods for the <see cref="ContainerEventType"/>
    /// </summary>
    public static class ContainerEventExtensions {

        /// <summary>
        /// Converts a <see cref="SyncListOperation"/> into a <see cref="ContainerEventType"/>
        /// </summary>
        public static ContainerEventType? ToContainerEventType(this SyncListOperation e) => e switch {
            SyncListOperation.Add or SyncListOperation.Insert => ContainerEventType.Add,
            SyncListOperation.RemoveAt => ContainerEventType.Remove,
            SyncListOperation.Set => ContainerEventType.Value,
            SyncListOperation.Clear => ContainerEventType.Clear,
            SyncListOperation.Complete => ContainerEventType.Full,
            _ => null
        };

    }

}
