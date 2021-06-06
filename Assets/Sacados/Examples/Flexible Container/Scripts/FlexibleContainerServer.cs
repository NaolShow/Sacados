#if IS_SERVER

using Sacados.Containers;
using Sacados.Items;
using Sacados.Slots;
using UnityEngine;

namespace Sacados.Examples.FlexibleContainer {

    public partial class FlexibleContainer : GUIContainer {

        public override void NetworkStart() {

            // If it's not the server
            if (!NetworkManager.IsServer) return;

            // Initialize the container with 0 slots
            Initialize(0);

        }

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

        public override void OnSlotEmpty(Slot slot) {

            // Remove the itemstack from the item stacks list
            ItemStacks.RemoveAt(slot.Index);
            // Remove the slot from the slots list
            Slots.Remove(slot);

        }

    }

}

#endif