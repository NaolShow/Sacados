using Mirror;

namespace Sacados.Items {

    public struct ItemStack {

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
        public Item Item;

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
            Item = item;
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
        public bool IsSameAs(ItemStack itemStack) {
            return itemStack.Item == Item;
        }

        /// <summary>
        /// Determines if the ItemStacks are the same or if the specified item stack is empty
        /// </summary>
        public bool IsSameOrEmpty(ItemStack itemStack) {
            return itemStack.IsEmpty || IsSameAs(itemStack);
        }

        #endregion

    }

    public static class NetworkItemStackSerializer {

        /// <summary>
        /// Writes an ItemStack to the Network Writer
        /// </summary>
        public static void WriteItemStack(this NetworkWriter writer, ItemStack itemStack) {

            // If the ItemStack is empty
            if (itemStack.IsEmpty) {

                // Write an empty Item
                writer.WriteString(null);
                return;

            }

            // Write the item
            writer.WriteItem(itemStack.Item);

            // Write the stack size
            writer.WriteUInt32(itemStack.StackSize);

        }

        /// <summary>
        /// Reads an ItemStack from the Network Reader
        /// </summary>
        public static ItemStack ReadItemStack(this NetworkReader reader) {

            // Read the item
            Item item = reader.ReadItem();

            // If the item is null (it means that the itemstack is empty)
            if (item == null) return ItemStack.Empty;

            // Create and return the itemstack
            return new ItemStack(item, reader.ReadUInt32());

        }

    }

}