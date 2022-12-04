using Sacados.Items;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Sacados.Items {

    [CreateAssetMenu(fileName = "New Test Item", menuName = "Sacados/Test Item")]
    public class TestItem : Item {

        public override Type ItemStack => typeof(TestItemStack);

    }

    public class TestItemStack : ItemStack {

        public ulong Value;

        public override bool IsSameAs(ItemStack itemStack) => itemStack is TestItemStack && base.IsSameAs(itemStack);

        public TestItemStack() { }
        public TestItemStack(TestItemStack itemStack) : base(itemStack) {
            Value = itemStack.Value;
        }

        public override bool Serialize(FastBufferWriter writer) {
            if (base.Serialize(writer))
                writer.WriteValueSafe(in Value);
            return true;
        }

        public override bool Deserialize(FastBufferReader reader) {
            if (base.Deserialize(reader))
                reader.ReadValueSafe(out Value);
            return true;
        }

        public override ItemStack Clone() => new TestItemStack(this);

    }

}
