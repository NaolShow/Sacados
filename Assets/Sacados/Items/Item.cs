using Hashing;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Sacados {

    /// <summary>
    /// Represents an <see cref="Item"/> data that have common and constant values
    /// </summary>
#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New Item", menuName = "Sacados/Item")]
#endif
    public class Item : ScriptableObject {

        // This should get called automatically when registering any Item
        static Item() {

            // Register the serialization methods for the items and itemstacks
            UserNetworkVariableSerialization<Item>.WriteValue = ItemNetworkExtensions.WriteValueSafe;
            UserNetworkVariableSerialization<Item>.ReadValue = ItemNetworkExtensions.ReadValueSafe;
            UserNetworkVariableSerialization<Item>.DuplicateValue = (in Item item, ref Item copyItem) => copyItem = item;

            UserNetworkVariableSerialization<ItemStack>.WriteValue = ItemStackNetworkExtensions.WriteValueSafe;
            UserNetworkVariableSerialization<ItemStack>.ReadValue = ItemStackNetworkExtensions.ReadValueSafe;
            UserNetworkVariableSerialization<ItemStack>.DuplicateValue = (in ItemStack itemStack, ref ItemStack copyItemStack) => copyItemStack = itemStack;

        }

        #region Registry

        /// <summary>
        /// Registry containing every registered <see cref="Item"/> associated with their hashed <see cref="Item.ID"/>
        /// </summary>
        public static IReadOnlyDictionary<ulong, Item> Registry => registry;
        private readonly static Dictionary<ulong, Item> registry = new Dictionary<ulong, Item>();

        /// <summary>
        /// Tries to register the specified <see cref="Item"/> into the <see cref="Registry"/>
        /// </summary>
        /// <param name="item">The <see cref="Item"/> that will be registered</param>
        /// <returns>True if the <see cref="Item"/> wasn't already registered and got registered successfully</returns>
        public static bool Register(Item item) => registry.TryAdd(item.HashedID, item);
        /// <summary>
        /// Tries to unregister the specified <see cref="Item"/> from the <see cref="Registry"/>
        /// </summary>
        /// <param name="item">The <see cref="Item"/> that will be registered</param>
        /// <returns>True if the <see cref="Item"/> was registered and got unregistered successfully</returns>
        public static bool UnRegister(Item item) => registry.Remove(item.HashedID);
        /// <summary>
        /// Unregisters every registered <see cref="Item"/>
        /// </summary>
        public static void UnregisterAll() => registry.Clear();

        /// <summary>
        /// Tries to get a registered <see cref="Item"/> corresponding with the specified <see cref="Item.ID"/>
        /// </summary>
        /// <inheritdoc cref="Get(ulong)"/>
        public static Item Get(string itemID) => Get(XXHash.Hash64(itemID));
        /// <summary>
        /// Tries to get a registered <see cref="Item"/> corresponding with the specified <see cref="Item.HashedID"/>
        /// </summary>
        /// <param name="hashedItemID">The <see cref="Item.HashedID"/> of the <see cref="Item"/> that you try to get</param>
        /// <returns>Null if the <see cref="Item"/> cannot be found or the <see cref="Item"/>'s reference if it has been found</returns>
        public static Item Get(ulong hashedItemID) => registry.TryGetValue(hashedItemID, out Item item) ? item : null;

        #endregion

        /// <summary>
        /// Determines the ID of the <see cref="Item"/> (can be used to identify the <see cref="Item"/> in-game)
        /// </summary>
        [field: SerializeField] public string ID { get; set; }
        /// <summary>
        /// Determines the hash of the <see cref="Item.ID"/> (used to synchronize the <see cref="Item"/> accross the network)
        /// </summary>
        public ulong HashedID => XXHash.Hash64(ID);

        /// <summary>
        /// Determines the max stack size of the <see cref="Item"/> (maximum amount that can be stored in one single slot)
        /// </summary>
        [field: SerializeField] public uint MaxStackSize { get; set; }

        /// <summary>
        /// Determines the <see cref="Sprite"/> of the <see cref="Item"/> (will be displayed in user interfaces)
        /// </summary>
        [field: SerializeField] public Sprite Sprite { get; set; }

        #region Create ItemStack

        /// <inheritdoc cref="ItemStack(Item)"/>
        /// <returns>The new <see cref="ItemStack"/> that contains the <see cref="Item"/></returns>
        public virtual ItemStack CreateItemStack() => new ItemStack(this);

        /// <summary>
        /// Creates an <see cref="ItemStack"/> for the <see cref="Item"/> with the specified stack size
        /// </summary>
        /// <param name="stackSize">The stack size of the <see cref="ItemStack"/></param>
        /// <returns>The new <see cref="ItemStack"/> that contains the <see cref="Item"/></returns>
        public ItemStack CreateItemStack(uint stackSize) {
            ItemStack itemStack = CreateItemStack();
            itemStack.StackSize = stackSize;
            return itemStack;
        }

        #endregion

        // Two Items are the same if their IDs matches
        public override bool Equals(object other) => other is Item item && item.HashedID == HashedID;
        public override int GetHashCode() => HashedID.GetHashCode();

        /// <summary>
        /// Returns the <see cref="Item.ID"/>
        /// </summary>
        /// <returns>The <see cref="Item.ID"/></returns>
        public override string ToString() => ID;

    }

}