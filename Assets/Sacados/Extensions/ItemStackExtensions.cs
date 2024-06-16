using FishNet.Serializing;

namespace Sacados {

    /// <summary>
    /// Extensions of the <see cref="ItemStack"/> class
    /// </summary>
    public static class ItemStackExtensions {

        /// <summary>
        /// Determines if the <see cref="ItemStack"/> is empty (either the <see cref="ItemStack"/>, it's <see cref="Item"/> or <see cref="ItemStack.Size"/> is null)
        /// </summary>
        /// <param name="itemStack">The <see cref="ItemStack"/> that might be empty</param>
        /// <returns>True if the <see cref="ItemStack"/> is empty</returns>
        public static bool IsEmpty(this ItemStack itemStack) => itemStack == null || ReferenceEquals(itemStack.Item, null) || itemStack.StackSize == 0;

    }

    /// <summary>
    /// Network extensions of the <see cref="ItemStack"/> class
    /// </summary>
    public static class ItemStackNetworkExtensions {

        /// <summary>
        /// Writes the <see cref="ItemStack"/> inside the <see cref="FastBufferWriter"/>
        /// </summary>
        /// <param name="writer">The <see cref="FastBufferWriter"/> that will contain the <see cref="ItemStack"/></param>
        /// <param name="value">The <see cref="ItemStack"/> that will get written</param>
        public static void WriteItemStack(this Writer writer, ItemStack itemStack) {

            // If the ItemStack is empty do not write the Item's data
            bool isEmpty = itemStack.IsEmpty();
            writer.WriteItem(isEmpty ? null : itemStack.Item);

            // Otherwise write the ItemStack's data
            if (!isEmpty)
                itemStack.Serialize(writer);

        }

        /// <summary>
        /// Reads the <see cref="ItemStack"/> from the <see cref="FastBufferReader"/>
        /// </summary>
        /// <param name="reader">The <see cref="FastBufferReader"/> that contains the <see cref="ItemStack"/></param>
        /// <param name="value">The <see cref="ItemStack"/> that got read</param>
        public static ItemStack ReadItemStack(this Reader reader) {

            // If the ItemStack is empty then return null
            Item item = reader.ReadItem();
            if (item == null) return null;

            // Otherwise create the corresponding ItemStack and read its data
            ItemStack itemStack = item.CreateItemStack();
            itemStack.Deserialize(reader);
            return itemStack;

        }

    }

}