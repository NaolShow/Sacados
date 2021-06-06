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

        public override void NetworkStart() {

            // If it's not the server
            if (!NetworkManager.IsServer) return;

            // Initialize the container with 25 slots
            Initialize(25);

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