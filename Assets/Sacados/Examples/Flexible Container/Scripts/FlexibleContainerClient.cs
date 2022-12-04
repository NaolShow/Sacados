#if IS_CLIENT

using Sacados.Containers;
using Sacados.Items;
using Sacados.Slots;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Sacados.Examples.FlexibleContainer {

    /**
     * 
     * Client code of the Flexible Container
     * 
     **/

    public partial class FlexibleContainer : GUIContainer {

        /// <summary>
        /// Prefab of the Slots UI
        /// </summary>
        public FlexibleSlotUI SlotUIPrefab;

        private void ClientUpdate() {

            // If the user wants to toggle the container
            if (Input.GetKeyDown(KeyCode.W)) {
                ToggleBuild();
            }

        }

        #region Build and Destroy slots

        protected override void BuildSlots() {

            // Initialize the slots UI list
            SlotsUI = new List<SlotUI>(SlotsCount);

            // Loop through the slots
            for (int i = 0; i < SlotsCount; i++) {

                // Build the slot
                BuildSlot();

            }

        }

        protected override void DestroySlots() {

            // Loop through the slots
            for (int i = 0; i < SlotsCount; i++) {

                // Destroy the slot
                Destroy(SlotsUI[i].gameObject);

            }

        }

        #endregion

        private void BuildSlot() {

            // Instantiate and initialize a new Slot UI
            SlotUI slotUI = Instantiate(SlotUIPrefab, transform).Initialize(this);

            // Add the slot UI to the slots UI list
            SlotsUI.Add(slotUI);

            // Refresh the Slot UI
            slotUI.Refresh();

        }

        protected override void OnContainerUpdate(NetworkListEvent<ItemStack> e) {

            switch (e.Type) {

                // The server has added a new ItemStack.
                // So it means that we have to build a new slot
                case NetworkListEvent<ItemStack>.EventType.Add:
                    BuildSlot();
                    break;

                // The server has removed an ItemStack
                // So it means that we have to destroy a slot
                case NetworkListEvent<ItemStack>.EventType.RemoveAt:

                    // Destroy the slot UI
                    Destroy(SlotsUI[e.Index].gameObject);

                    // Remove the slot UI from the list
                    SlotsUI.RemoveAt(e.Index);

                    break;
                default:
                    base.OnContainerUpdate(e);
                    break;

            }

        }

    }

}

#endif