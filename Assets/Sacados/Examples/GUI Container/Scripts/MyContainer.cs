using Mirror;
using Sacados.Containers;
using Sacados.Items;
using Sacados.Slots;
using System.Collections.Generic;
using UnityEngine;

namespace Sacados.Examples.GUI {

    public class MyContainer : GUIContainer {

        private void Update() {

#if IS_SERVER
            ServerUpdate();
#endif
#if IS_CLIENT
            ClientUpdate();
#endif

        }

#if IS_SERVER

        [ServerCallback]
        private void Awake() {

            // Initialize the container with 25 slots
            Initialize(25);

        }

        [ServerCallback]
        private void ServerUpdate() {

            // If the user wants to give an Item
            if (Input.GetKeyDown(KeyCode.A)) {

                // Gives an Item
                Give(new ItemStack(Item.Get("test_item"), 1));

            } else if (Input.GetKeyDown(KeyCode.Q)) {

                // Takes an Item
                Take(new ItemStack(Item.Get("test_item"), 1));

            }

        }

        [Server]
        protected override void CreateSlots() {

            // Loop through the slots
            for (int i = 0; i < SlotsCount; i++) {

                // Add a slot
                Slots.Add(new Slot(this, i));

            }

        }

#endif

#if IS_CLIENT

        public SlotUI SlotUIPrefab;

        [ClientCallback]
        private void ClientUpdate() {

            if (Input.GetKeyDown(KeyCode.W)) {
                ToggleBuild();
            }

        }

        [Client]
        protected override void GenerateSlots() {

            // Initialize the slots UI list
            SlotsUI = new List<SlotUI>();

            // Loop through the ItemStacks
            for (int i = 0; i < SlotsCount; i++) {

                // Add a new slot
                SlotsUI.Add(Instantiate(SlotUIPrefab, transform).Initialize(this, i));

            }

        }

        [Client]
        protected override void DestroySlots() {

            // Loop through the slots
            for (int i = 0; i < SlotsCount; i++) {

                // Destroy the slot
                Destroy(SlotsUI[i].gameObject);

            }

            // Clear the slots list
            SlotsUI = null;

        }

#endif

    }

}