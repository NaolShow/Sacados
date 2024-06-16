using UnityEngine;

namespace Sacados.Samples {

    /// <summary>
    /// Fixed size <see cref="IContainer"/> with fixed number of <see cref="ISlot"/>
    /// </summary>
    public class FixedSizeContainer : Container {

        [SerializeField] private int slotsCount;

        protected override void Awake() {
            base.Awake();
            Storage.OnStarted += Initialize;
        }

        private void Initialize() {
            // By default add slots count slots
            for (int i = 0; i < slotsCount; i++) AddSlot(new Slot(this, i));
        }

        public override void Give(ItemStack itemStack) {

            // Give to the slots until we reach the end of the container or we gave everything
            int i = 0;
            while (i < Size && itemStack.StackSize > 0)
                GetSlot(i++).Give(itemStack);

        }

        public override void Take(ItemStack itemStack) {

            // Take from the slots until we reach the end of the container or we gave everything
            int i = 0;
            while (i < Size && itemStack.StackSize > 0)
                GetSlot(i++).Take(itemStack);

        }

        public override void Clear() {
            for (int i = 0; i < Size; i++)
                GetSlot(i).Clear();
        }

    }

}
