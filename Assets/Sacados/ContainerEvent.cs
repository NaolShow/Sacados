using FishNet.Object.Synchronizing;
using System;

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
        /// When the <see cref="IContainer"/> is getting updated completely
        /// </summary>
        Full

    }

    /// <summary>
    /// Extensions methods for the <see cref="ContainerEventType"/>
    /// </summary>
    public static class ContainerEventExtensions {

        /// <summary>
        /// Converts a <see cref="NetworkListEvent{T}"/> into a <see cref="ContainerEventType"/>
        /// </summary>
        public static ContainerEventType ToContainerEventType(this SyncListOperation e) => e switch {
            SyncListOperation.Add or SyncListOperation.Insert => ContainerEventType.Add,
            SyncListOperation.RemoveAt => ContainerEventType.Remove,
            SyncListOperation.Set => ContainerEventType.Value,
            SyncListOperation.Clear => ContainerEventType.Clear,
            SyncListOperation.Complete => ContainerEventType.Full,
            _ => throw new ArgumentException($"{nameof(SyncListOperation)} cannot be converted to {nameof(ContainerEventType)} because it's value is unknown"),
        };

    }

}
