using Unity.Netcode;

namespace Sacados {

    /// <summary>
    /// Represents individual data about <see cref="{T}"/>
    /// </summary>
    /// <typeparam name="T"><see cref="Sacados.Items.Item"/> that the <see cref="ItemStack"/> stores</typeparam>
    public abstract class ItemStack<T> : ItemStack where T : Item {

        /// <summary>
        /// Represents the <see cref="{T}"/> of the <see cref="ItemStack{T}"/>
        /// </summary>
        public new T Item { get => (T)GetItem(); set => SetItem(value); }

        protected override void SetItem(Item itemStack) {
            if (itemStack is T)
                base.SetItem(itemStack);
        }

        #region Constructors

        /// <inheritdoc cref="ItemStack(Item)"/>
        public ItemStack(T item) : base(item) { }

        /// <inheritdoc cref="ItemStack(ItemStack)"/>
        public ItemStack(ItemStack original) : base(original) { }

        #endregion

    }

    /// <summary>
    /// Represents an <see cref="Item"/> instance that have a <see cref="StackSize"/> and other personal values
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
            StackSize = original.StackSize;
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

    }

}