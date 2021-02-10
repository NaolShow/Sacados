using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sacados.Items {

#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New Item", menuName = "Sacados/Item")]
#endif
    public class Item : ScriptableObject {

        #region Registry

        /// <summary>
        /// HashSet containing the registered Items
        /// </summary>
        public static HashSet<Item> Items = new HashSet<Item>();

        /// <summary>
        /// Registers an Item to the Items HashSet
        /// </summary>
        public static bool Register(Item item) {
            return Items.Add(item);
        }

        /// <summary>
        /// Find and returns an Item with the specified Item ID<br/>
        /// If no item is found then it returns null
        /// </summary>
        public static Item Get(string itemID) {

            // If the item ID is null
            if (itemID == null) return null;

            // Find and return default item
            return Items.Where(item => string.Equals(item.ID, itemID)).FirstOrDefault();

        }

        #endregion

        /// <summary>
        /// ID of the Item
        /// </summary>
        public string ID;

        /// <summary>
        /// Max stack size of the Item
        /// </summary>
        public uint MaxStackSize;

        /// <summary>
        /// Sprite of the Item
        /// </summary>
        public Sprite Sprite;

    }

    public static class NetworkItemSerializer {

        /// <summary>
        /// Writes an Item to the Network Writer
        /// </summary>
        public static void WriteItem(this NetworkWriter writer, Item item) {
            writer.WriteString(item?.ID);
        }

        /// <summary>
        /// Reads an Item from the Network Reader
        /// </summary>
        public static Item ReadItem(this NetworkReader reader) {
            return Item.Get(reader.ReadString());
        }

    }

}