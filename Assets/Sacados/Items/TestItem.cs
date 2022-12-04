using Sacados.Items;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Sacados.Items {

    [CreateAssetMenu(fileName = "New Test Item", menuName = "Sacados/Test Item")]
    public class TestItem : Item {

        public override ItemStack CreateItemStack() => new TestItemStack(this);

    }

    public class TestItemStack : ItemStack {

        public ulong Value;

        public override bool IsSameAs(ItemStack itemStack) => itemStack is TestItemStack && base.IsSameAs(itemStack);

        public TestItemStack() { }
        public TestItemStack(Item item) : base(item) { }
        public TestItemStack(TestItemStack itemStack) : base(itemStack) {
            Value = itemStack.Value;
        }

        public override void Serialize(FastBufferWriter writer) {
            base.Serialize(writer);
            writer.WriteValueSafe(in Value);
        }

        public override void Deserialize(FastBufferReader reader) {
            base.Deserialize(reader);
            reader.ReadValueSafe(out Value);
        }

        public override ItemStack Clone() => new TestItemStack(this);

    }

}
