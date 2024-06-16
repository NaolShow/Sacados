using FishNet.Serializing;
using UnityEngine;

namespace Sacados {

    /// <summary>
    /// Network extensions of the <see cref="Item"/> class
    /// </summary>
    public static class ItemNetworkExtensions {

        /// <summary>
        /// Writes the <see cref="Item"/> inside the <see cref="FastBufferWriter"/> by simply writing its <see cref="Item.HashedID"/>
        /// </summary>
        /// <param name="writer">The <see cref="FastBufferWriter"/> that will contain the <see cref="Item"/></param>
        /// <param name="value">The <see cref="Item"/> that will get written</param>
        public static void WriteItem(this Writer writer, Item value) {

            // Write a boolean indicating if the Item is empty or not
            writer.WriteBoolean(value == null);

            // If the Item is not empty then write it's hashed id
            if (value != null) writer.WriteUInt64(value.HashedID);

        }

        /// <summary>
        /// Reads the <see cref="Item"/> from the <see cref="FastBufferReader"/> by simply getting it using its <see cref="Item.HashedID"/>
        /// </summary>
        /// <param name="reader">The <see cref="FastBufferReader"/> that contains the <see cref="Item"/></param>
        /// <param name="value">The <see cref="Item"/> that got read</param>
        public static Item ReadItem(this Reader reader) {

            // If there is no Item
            if (reader.ReadBoolean()) return null;

            // If the item couldn't not be found
            ulong hashedID = reader.ReadUInt64();
            if (!Item.TryGet(hashedID, out Item item))
                Debug.LogWarning($"Trying to deserialize an {nameof(Item)} with hashedID='{hashedID}' and could not be found. Is there a registry mismatch between peers?");
            return item;

        }

    }

}
