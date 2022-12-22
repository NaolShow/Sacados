﻿using System;
using Unity.Netcode;

namespace Sacados.Items {

    /// <summary>
    /// Represents individual data about <see cref="{T}"/>
    /// </summary>
    /// <typeparam name="T"><see cref="Sacados.Items.Item"/> that the <see cref="ItemStack"/> stores</typeparam>
    public abstract class ItemStack<T> : ItemStack where T : Item {

        /// <summary>
        /// Represents the <see cref="{T}"/> of the <see cref="ItemStack{T}"/>
        /// </summary>
        public new T Item { get => (T)base.Item; set => base.Item = value; }

        #region Constructors

        /// <summary>
        /// Creates an empty <see cref="ItemStack"/> (prefer using <see cref="null"/> instead)
        /// </summary>
        public ItemStack() { }

        /// <summary>
        /// Creates an <see cref="ItemStack"/> that copies all the values of the specified one
        /// </summary>
        /// <param name="original">The <see cref="ItemStack"/> that will be copied</param>
        public ItemStack(ItemStack original) : base(original) { }

        /// <summary>
        /// Creates an <see cref="ItemStack"/> with the specified <see cref="Item"/> and <see cref="StackSize"/>
        /// </summary>
        public ItemStack(Item item, uint stackSize) : base(item, stackSize) { }

        /// <summary>
        /// Creates an <see cref="ItemStack"/> with the specified <see cref="Item"/> and it's <see cref="Item.MaxStackSize"/> as the <see cref="ItemStack.StackSize"/>
        /// </summary>
        public ItemStack(Item item) : base(item) { }

        #endregion

    }

    /// <summary>
    /// Represents an <see cref="Item"/> instance that have a <see cref="StackSize"/> and other personal values
    /// </summary>
    public class ItemStack {

        // Register the serialization methods for the itemstacks
        static ItemStack() {
            UserNetworkVariableSerialization<ItemStack>.WriteValue = ItemStackNetworkExtensions.WriteValueSafe;
            UserNetworkVariableSerialization<ItemStack>.ReadValue = ItemStackNetworkExtensions.ReadValueSafe;
        }

        /// <summary>
        /// <see cref="Item"/> of the <see cref="ItemStack"/>
        /// </summary>
        public Item Item {
            get => item; set {

#if UNITY_EDITOR || DEVELOPMENT_BUILD

                // If the item is not null
                if (value != null) {

                    // If the item shouldn't be assigned to this type of ItemStack
                    Type assignedType = value.CreateItemStack().GetType();
                    Type requiredType = GetType();
                    if (requiredType != assignedType)
                        throw new ArgumentException($"Trying to assign the {nameof(Item)} '{value.ID}' to {assignedType.Name} instead of {requiredType.Name} (this could cause synchronization errors!)");

                }
#endif

                item = value;

            }
        }
        private Item item;

        /// <summary>
        /// <see cref="StackSize"/> of the <see cref="ItemStack"/>
        /// </summary>
        public uint StackSize { get => stackSize; set => stackSize = value; }
        private uint stackSize;

        #region Constructors

        /// <summary>
        /// Creates an empty <see cref="ItemStack"/> (prefer using <see cref="null"/> instead)
        /// </summary>
        public ItemStack() { }

        /// <summary>
        /// Creates an <see cref="ItemStack"/> that copies all the values of the specified one
        /// </summary>
        /// <param name="original">The <see cref="ItemStack"/> that will be copied</param>
        public ItemStack(ItemStack original) {
            Item = original.Item;
            StackSize = original.StackSize;
        }

        /// <summary>
        /// Creates an <see cref="ItemStack"/> with the specified <see cref="Item"/> and <see cref="StackSize"/>
        /// </summary>
        public ItemStack(Item item, uint stackSize) {

            // Save the Item and it's Stack Size
            this.item = item;
            this.stackSize = stackSize;

        }

        /// <summary>
        /// Creates an <see cref="ItemStack"/> with the specified <see cref="Item"/> and it's <see cref="Item.MaxStackSize"/> as the <see cref="ItemStack.StackSize"/>
        /// </summary>
        public ItemStack(Item item) : this(item, (item == null) ? 0 : item.MaxStackSize) { }

        #endregion

        /// <summary>
        /// Determines if both the <see cref="ItemStack"/> are the same (can be combined in a single <see cref="ItemStack"/>)
        /// </summary>
        public virtual bool IsSameAs(ItemStack itemStack) => itemStack.Item == Item;

        /// <summary>
        /// Serializes the <see cref="ItemStack"/> into the specified <see cref="FastBufferWriter"/>
        /// </summary>
        /// <param name="writer">The <see cref="FastBufferWriter"/> that will contain the serialized <see cref="ItemStack"/></param>
        public virtual void Serialize(FastBufferWriter writer) {
            writer.WriteValueSafe(in stackSize);
        }

        /// <summary>
        /// Deserializes an <see cref="ItemStack"/> from the specified <see cref="FastBufferReader"/>
        /// </summary>
        /// <param name="reader">The <see cref="FastBufferReader"/> that contains a serialized <see cref="ItemStack"/></param>
        public virtual void Deserialize(FastBufferReader reader) {
            reader.ReadValueSafe(out stackSize);
        }

        /// <summary>
        /// Clones the <see cref="ItemStack"/>
        /// </summary>
        /// <returns>The cloned <see cref="ItemStack"/></returns>
        public virtual ItemStack Clone() => new ItemStack(this);

    }

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

        /// <summary>
        /// Determines if the <see cref="ItemStack"/> is empty or if it the same as the other one
        /// </summary>
        /// <param name="possiblyEmpty">The <see cref="ItemStack"/> that might be empty</param>
        /// <param name="other">The other <see cref="ItemStack"/> that might the same of the other one</param>
        /// <returns>True if either the <see cref="ItemStack"/> is empty or if they are the same</returns>
        public static bool IsEmptyOrSame(this ItemStack possiblyEmpty, ItemStack other) => possiblyEmpty.IsEmpty() || possiblyEmpty.IsSameAs(other);

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