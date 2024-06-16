namespace Sacados.Samples {

    /// <summary>
    /// Flexible size <see cref="IContainer"/> that can have <see cref="ISlot"/> added or removed at runtime
    /// </summary>
    public class FlexibleSizeContainer : Container {

        protected override void OnContainerUpdate(ContainerEventType type, ItemStack oldItemStack, int index) {

            // If we are not the server then we must sync the added/removed slots
            if (!IsServerInitialized) {

                switch (type) {
                    case ContainerEventType.Add: AddSlot(new Slot(this, index)); break;
                    case ContainerEventType.Remove: RemoveSlot(index); break;
                    case ContainerEventType.Clear: ClearSlots(); break;
                    // If we joined the server then create all the slots directly
                    case ContainerEventType.Full:
                        for (int i = 0; i < Size; i++)
                            AddSlot(new Slot(this, i));
                        break;
                }

            }

            // Execute the on update event at the end
            base.OnContainerUpdate(type, oldItemStack, index);

            // If the slot is now empty then remove it
            if (IsServerInitialized && type == ContainerEventType.Value && this[index].IsEmpty())
                RemoveSlot(index);

        }

        public override void Give(ItemStack itemStack) {
            int i = 0;
            while (itemStack.StackSize > 0) {

                // If there is no more room for this ItemStack then create a new slot
                if (Size == i) AddSlot(new Slot(this, i));
                GetSlot(i++).Give(itemStack);

            }
        }

        public override void Take(ItemStack itemStack) {
            int i = Size; // or (int i = 0)
            while (itemStack.StackSize > 0 && --i >= 0) // or (itemStack.StackSize > 0 && i < SlotsCount)
                GetSlot(i).Take(itemStack);
        }

        public override void Clear() => ClearSlots();

    }

}
