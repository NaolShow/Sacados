#if IS_SERVER

using Mirror;
using Sacados.Containers;
using Sacados.Items;
using Sacados.Slots;
using UnityEngine;

namespace Sacados.Examples.FlexibleContainer {

    public partial class FlexibleContainer : GUIContainer {

        // Initialize the container with 0 slots
        [ServerCallback]
        private void Awake() => Initialize(0);

        [ServerCallback]
        private void ServerUpdate() {

            // If the user wants to give an Item
            if (Input.GetKeyDown(KeyCode.A)) {

                // Gives 64 Items
                Give(new ItemStack(Item.Get("test_item"), 64));

            } else if (Input.GetKeyDown(KeyCode.Q)) {

                // Takes 48 Items
                Take(new ItemStack(Item.Get("test_item"), 48));

            }

        }

        // Called when we reach the slots limit
        [Server]
        protected override ItemStack OnOverflow(ItemStack itemStack) {

            // While the ItemStack is not empty
            while (!itemStack.IsEmpty) {

                // Create a new slot and add an empty itemstack
                Slots.Add(new Slot(this));
                ItemStacks.Add(ItemStack.Empty);

                // Give the ItemStack to the new slot and save the remaining
                itemStack = Slots[SlotsCount - 1].Give(itemStack);

            }

            // Here the itemstack is always empty
            return itemStack;

        }

        [Server]
        public override void OnSlotEmpty(Slot slot) {

            // Remove the itemstack from the item stacks list
            ItemStacks.RemoveAt(slot.Index);
            // Remove the slot from the slots list
            Slots.Remove(slot);

        }

    }

}

#endif