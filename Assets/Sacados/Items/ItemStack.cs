﻿using Unity.Netcode;

namespace Sacados {

    /// <summary>
    /// Represents individual data about an <see cref="Sacados.Item"/>
    /// </summary>
    public class ItemStack {

        /// <summary>
        /// <see cref="Item"/> contained in the <see cref="ItemStack"/>
        /// </summary>
        public Item Item { get => GetItem(); set => SetItem(value); }
        private Item item;

        /// <summary>
        /// <see cref="StackSize"/> of the <see cref="ItemStack"/>
        /// </summary>
        public uint StackSize { get => stackSize; set => stackSize = value; }
        private uint stackSize;

        #region Constructors

        /// <summary>
        /// Creates an empty <see cref="ItemStack"/> (prefer using <see cref="null"/> instead for no allocation)
        /// </summary>
        public ItemStack() { }

        /// <summary>
        /// Creates an <see cref="ItemStack"/> with the specified <see cref="Item"/> and with it's <see cref="Item.MaxStackSize"/> as the <see cref="StackSize"/>
        /// </summary>
        public ItemStack(Item item) {
            Item = item;
            stackSize = item.MaxStackSize;
        }

        /// <summary>
        /// Creates an <see cref="ItemStack"/> that copies all the values of the specified one
        /// </summary>
        /// <param name="original">The <see cref="ItemStack"/> that will be copied</param>
        public ItemStack(ItemStack original) {
            Item = original.Item;
            stackSize = original.StackSize;
        }

        #endregion

        /// <summary>
        /// Gets the contained <see cref="Item"/> from the <see cref="ItemStack"/>
        /// Replaces the "get" override of the <see cref="Item"/> property, because overriding and hidding a property isn't possible at the same time
        /// </summary>
        /// <returns>The <see cref="ItemStack"/> contained <see cref="Item"/></returns>
        protected virtual Item GetItem() => item;
        /// <summary>
        /// Sets the contained <see cref="Item"/> of the <see cref="ItemStack"/>
        /// Replaces the "set" override of the <see cref="Item"/> property, because overriding and hidding a property isn't possible at the same time
        /// </summary>
        /// <returns>The <see cref="ItemStack"/> contained <see cref="Item"/></returns>
        protected virtual void SetItem(Item itemStack) => item = itemStack;

        /// <summary>
        /// Determines if both the <see cref="ItemStack"/> are the same (can be combined in a single <see cref="ItemStack"/>)
        /// </summary>
        public virtual bool IsSameAs(ItemStack itemStack) => ReferenceEquals(Item, itemStack.Item);

        /// <summary>
        /// Clones the <see cref="ItemStack"/>
        /// </summary>
        /// <returns>The cloned <see cref="ItemStack"/></returns>
        public virtual ItemStack Clone() => new ItemStack(this);

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

        private const string empty = "Empty";
        public override string ToString() => this.IsEmpty() ? empty : $"{Item.ID}x{StackSize}";

    }

}