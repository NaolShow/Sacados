#if IS_SERVER

using Sacados.Containers;
using Sacados.Items;
using Sacados.Slots;
using UnityEngine;

namespace Sacados.Examples.FixedContainer {

    public partial class FixedContainer : GUIContainer {

        /**
          * 
          * Server code of the Fixed Container
          * 
          **/

        public override void OnNetworkSpawn() {

            // If it's not the server
            if (!NetworkManager.IsServer) return;

            // Initialize the container with 25 slots
            Initialize(25);

        }

        private void ServerUpdate() {

            // Quick and dirty tests

            // If the user wants to give an Item
            if (Input.GetKeyDown(KeyCode.A)) {

                // Gives 64 Items
                ItemStack stack = new ItemStack(Item.Get("test_item"), 64);
                Debug.Log($"Giving {stack.Item.ID}x{stack.StackSize} ({stack.GetType().Name})");
                Give(stack);

            } else if (Input.GetKeyDown(KeyCode.Q)) {

                // Takes 48 Items
                Take(new ItemStack(Item.Get("test_item"), 48));

            } else if (Input.GetKeyDown(KeyCode.Z)) {

                ItemStack stack = Item.Get("new_test_item").CreateItemStack();
                stack.StackSize = 64;
                Debug.Log($"Giving {stack.Item.ID}x{stack.StackSize} ({stack.GetType().Name})");
                // Gives 64 Items
                Give(stack);

            } else if (Input.GetKeyDown(KeyCode.S)) {

                ItemStack stack = Item.Get("new_test_item").CreateItemStack();
                stack.StackSize = 48;
                // Gives 64 Items
                Take(stack);

            }

        }

        protected override void CreateSlots() {

            // Loop through the slots
            for (int i = 0; i < SlotsCount; i++) {

                // Initialize and add a new slot
                Slots.Add(new Slot(this));

            }

        }

    }

}

#endif