using Unity.Netcode;
using UnityEngine;

namespace Sacados {

    /// <summary>
    /// Network extensions of the <see cref="Item"/> class
    /// </summary>
    public static class ItemNetworkExtensions {

        /// <summary>
        /// Writes the <see cref="Item"/> inside the <see cref="FastBufferWriter"/> by simply writing it's <see cref="Item.HashedID"/>
        /// </summary>
        /// <param name="writer">The <see cref="FastBufferWriter"/> that will contain the <see cref="Item"/></param>
        /// <param name="value">The <see cref="Item"/> that will get written</param>
        public static void WriteValueSafe(this FastBufferWriter writer, in Item value) {

            // Write a boolean indicating if the Item is empty or not
            writer.WriteValueSafe(value == null);

            // If the Item is not empty then write it's hashed id
            if (value != null) writer.WriteValueSafe(value.HashedID);

        }

        /// <summary>
        /// Reads the <see cref="Item"/> from the <see cref="FastBufferReader"/> by simply getting it using it's <see cref="Item.HashedID"/>
        /// </summary>
        /// <param name="reader">The <see cref="FastBufferReader"/> that contains the <see cref="Item"/></param>
        /// <param name="value">The <see cref="Item"/> that got read</param>
        public static void ReadValueSafe(this FastBufferReader reader, out Item value) {

            // If the Item is empty then return null
            reader.ReadValueSafe(out bool isEmpty);
            if (isEmpty) value = null;
            // If the Item is not null then read it's hashed id and return it from the registry
            else {
                reader.ReadValueSafe(out ulong id);
                value = Item.Get(id);

                // If the Item isn't registered on this side then log an error
                if (value == null) Debug.LogError($"Trying to deserialize an {nameof(Item)} with the {nameof(Item.HashedID)} '{id}' while it is currently not registered");

            }

        }

    }

}
