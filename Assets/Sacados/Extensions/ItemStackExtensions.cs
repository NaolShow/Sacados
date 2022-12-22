using Sacados.Items;
using Unity.Netcode;

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
        public static void WriteValueSafe(this FastBufferWriter writer, in ItemStack value) {

            // If the ItemStack is empty then write an empty Item, else write the Item
            bool isEmpty = value.IsEmpty();
            writer.WriteValueSafe(isEmpty ? null : value.Item);

            // If the ItemStack is not empty then serialize it's data
            if (!isEmpty) value.Serialize(writer);

        }

        /// <summary>
        /// Reads the <see cref="ItemStack"/> from the <see cref="FastBufferReader"/>
        /// </summary>
        /// <param name="reader">The <see cref="FastBufferReader"/> that contains the <see cref="ItemStack"/></param>
        /// <param name="value">The <see cref="ItemStack"/> that got read</param>
        public static void ReadValueSafe(this FastBufferReader reader, out ItemStack value) {

            // TODO: Fix this since now we can't use Serialize or Deserialize of ItemStacks
            // => Wouldn't serialize Item

            // If the ItemStack is empty then return null
            reader.ReadValueSafe(out Item item);
            if (item == null) value = null;
            // If the ItemStack isn't empty then return it with it's deserialized data
            else {
                value = item.CreateItemStack();
                value.Deserialize(reader);
            }

        }

    }

}