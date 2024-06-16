using FishNet.Serializing;
using UnityEngine;

namespace Sacados.Sample {

    /// <summary>
    /// Represents a custom <see cref="ItemStack"/>
    /// </summary>
    public class CustomItemStack : ItemStack {

        /// <summary>
        /// Represents the <see cref="CustomItemStack"/> durability
        /// </summary>
        public uint Durability { get => durability; set => durability = value; }
        private uint durability;

        /// <inheritdoc cref="ItemStack(Item)"/>
        public CustomItemStack(CustomItem item) : base(item) { }
        /// <inheritdoc cref="ItemStack(ItemStack)"/>
        public CustomItemStack(CustomItemStack original) : base(original) {
            Durability = original.Durability;
        }

        public override void Serialize(Writer writer) {
            base.Serialize(writer);

            // Serialize the stack's durability
            writer.WriteUInt32(durability);

        }

        public override void Deserialize(Reader reader) {
            base.Deserialize(reader);

            // Deserialize the stack's durability
            durability = reader.ReadUInt32();

        }

        // If from the base they are the same and then check from this type
        public override bool IsSameAs(ItemStack itemStack)
            => base.IsSameAs(itemStack) && (itemStack is CustomItemStack customItemStack) && customItemStack.Durability == Durability;

        // Simply clone the ItemStack from it's constructor
        public override ItemStack Clone() => new CustomItemStack(this);

    }

#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New Custom Item", menuName = "Sacados/Custom Item")]
#endif
    public class CustomItem : Item {

        // Just an example, you can store anything here about items
        [field: SerializeField] public GameObject AnyPrefab { get; private set; }

        // Simply create an ItemStack from it's constructor
        public override ItemStack CreateItemStack() => new CustomItemStack(this);

    }

}