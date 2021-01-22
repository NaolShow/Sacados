using Mirror;

namespace Sacados.Core.Items {

    public class ItemStack {

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
        public ItemStack(Item item, uint stackSize) {

            // Save the Item and the StackSize
            Item = item;
            StackSize = stackSize;

        }

        /// <summary>
        /// Creates an ItemStack with the specified Item and it's MaxStackSize as the StackSize
        /// </summary>
        /// <param name="item"></param>
        public ItemStack(Item item) : this(item, item.MaxStackSize) { }

        #endregion

    }

    public static class NetworkItemStackSerializer {

        /// <summary>
        /// Writes an ItemStack to the Network Writer
        /// </summary>
        public static void WriteItemStack(this NetworkWriter writer, ItemStack itemStack) {

            // If the ItemStack is empty
            if (itemStack.IsEmpty()) {

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
            if (item == null) return null;

            // Create and return the itemstack
            return new ItemStack(item, reader.ReadUInt32());

        }

    }

}