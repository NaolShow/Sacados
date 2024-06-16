using Hashing;
using System.Collections.Generic;
using UnityEngine;

namespace Sacados {

    /// <summary>
    /// Represents common data about an <see cref="Item"/>
    /// </summary>
#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New Item", menuName = "Sacados/Item")]
#endif
    public class Item : ScriptableObject {

        #region Registry

        /// <summary>
        /// Registry containing every registered <see cref="Item"/> associated with their hashed <see cref="Item.ID"/>
        /// </summary>
        public static IReadOnlyDictionary<ulong, Item> Registry => registry;
        private readonly static Dictionary<ulong, Item> registry = new Dictionary<ulong, Item>();

        /// <summary>
        /// Registers the specified <see cref="Item"/> into the <see cref="Registry"/>
        /// </summary>
        /// <param name="item">The <see cref="Item"/> that will be registered</param>
        /// <returns>True if no other <see cref="Item"/> was registered with the same <see cref="Item.ID"/>, false otherwise</returns>
        public static bool Register(Item item) => registry.TryAdd(item.HashedID, item);
        /// <summary>
        /// Unregisters the specified <see cref="Item"/> from the <see cref="Registry"/>
        /// </summary>
        /// <param name="item">The <see cref="Item"/> that will be unregistered</param>
        /// <returns>True if an <see cref="Item"/> was registered with the same <see cref="Item.ID"/> and got unregistered, false otherwise</returns>
        public static bool Unregister(Item item) => registry.Remove(item.HashedID);
        /// <inheritdoc cref="Unregister(Item)"/>
        /// <param name="itemHashedID">The <see cref="Item.HashedID"/> that will be unregistered</param>
        public static bool Unregister(ulong itemHashedID) => registry.Remove(itemHashedID);
        /// <summary>
        /// Unregisters all the registered <see cref="Item"/> from the <see cref="Registry"/>
        /// </summary>
        public static void UnregisterAll() => registry.Clear();

        /// <summary>
        /// Tries to get an <see cref="Item"/> with the specified <see cref="Item.ID"/> from the <see cref="Registry"/>
        /// </summary>
        /// <param name="itemID">The <see cref="Item.ID"/> that we are trying to get</param>
        /// <param name="item">The <see cref="Item"/> or null if the not found</param>
        /// <returns>True if an <see cref="Item"/> with the specified <see cref="Item.ID"/> was found, false otherwise</returns>
        public static bool TryGet(string itemID, out Item item) => registry.TryGetValue(XXHash.Hash64(itemID), out item);
        /// <inheritdoc cref="TryGet(string, out Item)"/>
        /// <param name="itemHashedID">The <see cref="Item.HashedID"/> that we are trying to get</param>
        public static bool TryGet(ulong itemHashedID, out Item item) => registry.TryGetValue(itemHashedID, out item);

        /// <summary>
        /// Tries to get an <see cref="Item"/> with the specified <see cref="Item.ID"/> from the <see cref="Registry"/>
        /// </summary>
        /// <param name="itemID">The <see cref="Item.ID"/> that we are trying to get</param>
        /// <returns>The <see cref="Item"/> reference if found, null otherwise</returns>
        public static Item Get(string itemID) => Get(XXHash.Hash64(itemID));
        /// <summary>
        /// Tries to get a registered <see cref="Item"/> corresponding with the specified <see cref="Item.HashedID"/>
        /// </summary>
        /// <param name="hashedItemID">The <see cref="Item.HashedID"/> that we are trying to get</param>
        /// <returns>The <see cref="Item"/> reference if found, null otherwise</returns>
        public static Item Get(ulong hashedItemID) => registry.TryGetValue(hashedItemID, out Item item) ? item : null;

        #endregion

        /// <summary>
        /// Determines the ID of the <see cref="Item"/> (can be used to identify the <see cref="Item"/> in-game)
        /// </summary>
        [field: SerializeField] public string ID { get; private set; }
        /// <summary>
        /// Determines the hash of the <see cref="Item.ID"/> (used to synchronize the <see cref="Item"/> accross the network)
        /// </summary>
        public ulong HashedID => hashedID ??= XXHash.Hash64(ID);
        private ulong? hashedID = null;

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