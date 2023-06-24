using System;
using Unity.Netcode;

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
        public static ContainerEventType ToContainerEventType<T>(this NetworkListEvent<T> e) => e.Type switch {
            NetworkListEvent<T>.EventType.Add or NetworkListEvent<T>.EventType.Insert => ContainerEventType.Add,
            NetworkListEvent<T>.EventType.Remove or NetworkListEvent<T>.EventType.RemoveAt => ContainerEventType.Remove,
            NetworkListEvent<T>.EventType.Value => ContainerEventType.Value,
            NetworkListEvent<T>.EventType.Clear => ContainerEventType.Clear,
            NetworkListEvent<T>.EventType.Full => ContainerEventType.Full,
            _ => throw new ArgumentException($"{nameof(NetworkListEvent<T>)} cannot be converted to {nameof(ContainerEventType)} because it's {nameof(NetworkListEvent<T>.EventType)} is unknown"),
        };

    }

}
