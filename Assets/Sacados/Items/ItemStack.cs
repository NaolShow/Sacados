using System;
using Unity.Netcode;

namespace Sacados.Items {

    public struct ItemStack : INetworkSerializable, IEquatable<ItemStack> {

        #region Static

        /// <summary>
        /// Empty ItemStack used to give a default value
        /// </summary>
        public static readonly ItemStack Empty = new ItemStack(null, 0);

        #endregion

        #region Is Properties

        /// <summary>
        /// Determines if the ItemStack is empty (if the Item is null or it's StackSize = 0)
        /// </summary>
        public bool IsEmpty => (Item == null) || (StackSize == 0);

        #endregion

        /// <summary>
        /// Item of the ItemStack
        /// </summary>
        public Item Item { get => null; set { } }

        /// <summary>
        /// Stack size of the ItemStack
        /// </summary>
        public uint StackSize;

        #region Constructors

        /// <summary>
        /// Creates an ItemStack with the specified Item and StackSize
        /// </summary>
        public ItemStack(Item item, uint stackSize) : this() {

            // Save the Item and the StackSize
            StackSize = stackSize;

        }

        /// <summary>
        /// Creates an ItemStack with the specified Item and it's MaxStackSize as the StackSize
        /// </summary>
        public ItemStack(Item item) : this(item, (item == null) ? 0 : item.MaxStackSize) { }

        #endregion

        #region Is Methods

        /// <summary>
        /// Determines if the ItemStacks are the same
        /// </summary>
        public bool IsSameAs(ItemStack itemStack) => itemStack.Item == Item;

        /// <summary>
        /// Determines if the ItemStacks are the same or if the specified item stack is empty
        /// </summary>
        public bool IsSameOrEmpty(ItemStack itemStack) => itemStack.IsEmpty || IsSameAs(itemStack);

        #endregion

        public bool Equals(ItemStack itemStack) => itemStack.StackSize == StackSize && IsSameAs(itemStack);

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {

        }

    }

}