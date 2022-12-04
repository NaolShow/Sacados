using Hashing;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Netcode;
using UnityEngine;

namespace Sacados.Items {

    /// <summary>
    /// Represents an <see cref="Item"/> data that have common and constant values
    /// </summary>
#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New Item", menuName = "Sacados/Item")]
#endif
    public class Item : ScriptableObject {

        // Register the serialization methods for the items
        static Item() {
            UserNetworkVariableSerialization<Item>.WriteValue = ItemNetworkExtensions.WriteValueSafe;
            UserNetworkVariableSerialization<Item>.ReadValue = ItemNetworkExtensions.ReadValueSafe;
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
        /// <param name="itemID">The <see cref="Item.ID"/> of the <see cref="Item"/> that you try to get</param>
        /// <returns>Null if the <see cref="Item"/> cannot be found or the <see cref="Item"/>'s reference if it has been found</returns>
        public static Item Get(ulong hashedItemID) => registry.TryGetValue(hashedItemID, out Item item) ? item : null;

        #endregion

        /// <summary>
        /// Determines the ID of the <see cref="Item"/> (can be used to identify the <see cref="Item"/> in-game)
        /// </summary>
        [field: SerializeField] public string ID { get; private set; }
        /// <summary>
        /// Determines the hash of the <see cref="Item.ID"/> (used to synchronize the <see cref="Item"/> accross the network)
        /// </summary>
        public ulong HashedID => XXHash.Hash64(ID);

        /// <summary>
        /// Determines the max stack size of the <see cref="Item"/> (maximum amount that can be stored in one single slot)
        /// </summary>
        [field: SerializeField] public uint MaxStackSize { get; private set; }

        /// <summary>
        /// Determines the <see cref="Sprite"/> of the <see cref="Item"/> (will be displayed in user interfaces)
        /// </summary>
        [field: SerializeField] public Sprite Sprite { get; private set; }

        #region Callbacks

        /// <summary>
        /// Called when the <see cref="Item"/> is getting registered inside of the <see cref="Registry"/>
        /// </summary>
        public virtual void OnRegister() {

            // Create the item's itemstack constructor delegate
            itemStackConstructor = Expression.Lambda<Func<ItemStack>>(Expression.New(ItemStack.GetConstructor(Type.EmptyTypes))).Compile();

        }

        #endregion

        #region Custom Item Stacks

        /// <summary>
        /// Determines in which <see cref="ItemStack"/> type the <see cref="Item"/> should be stored
        /// </summary>
        public virtual Type ItemStack => typeof(ItemStack);

        /// <summary>
        /// Creates an <see cref="ItemStack"/> for the <see cref="Item"/>
        /// </summary>
        /// <returns>The <see cref="ItemStack"/> reference</returns>
        /// <exception cref="InvalidOperationException">Thrown when the <see cref="Item"/> isn't registered</exception>
        public ItemStack CreateItemStack() {

            // If the item stack constructor isn't set
            if (itemStackConstructor == null)
                throw new InvalidOperationException($"Trying to create an {nameof(ItemStack)} for the Item '{nameof(ID)}' while it is currently not registered");
            return itemStackConstructor.Invoke();

        }
        private Func<ItemStack> itemStackConstructor = null;

        #endregion

    }

    /// <summary>
    /// Network extensions of the <see cref="Item"/> class
    /// </summary>
    public static class ItemNetworkExtensions {

        /// <summary>
        /// Writes the <see cref="Item"/> inside the <see cref="FastBufferWriter"/> by simply writing it's <see cref="Item.HashedID"/>
        /// </summary>
        /// <param name="writer">The <see cref="FastBufferWriter"/> that will contain the <see cref="Item"/></param>
        /// <param name="value">The <see cref="Item"/> that will get written</param>
        public static void WriteValueSafe(this FastBufferWriter writer, in Item value) => writer.WriteValueSafe(value.HashedID);

        /// <summary>
        /// Reads the <see cref="Item"/> from the <see cref="FastBufferReader"/> by simply getting it using it's <see cref="Item.HashedID"/>
        /// </summary>
        /// <param name="reader">The <see cref="FastBufferReader"/> that contains the <see cref="Item"/></param>
        /// <param name="value">The <see cref="Item"/> that got read</param>
        public static void ReadValueSafe(this FastBufferReader reader, out Item value) {
            reader.ReadValueSafe(out ulong id);

            // If the item isn't registered on this side
            value = Item.Get(id);
            if (value == null)
                Debug.LogError($"Trying to deserialize an {nameof(Item)} with the {nameof(Item.HashedID)} '{id}' while it is currently not registered");

        }

    }

}