#if IS_CLIENT

using Sacados.Containers;
using Sacados.Slots;
using System.Collections.Generic;
using UnityEngine;

namespace Sacados.Examples.FixedContainer {

    public partial class FixedContainer : GUIContainer {

        /**
          * 
          * Client code of the Fixed Container
          * 
          **/

        /// <summary>
        /// Prefab of the Slots UI
        /// </summary>    
        public SlotUI SlotUIPrefab;

        private void ClientUpdate() {

            // If the user wants to toggle the container
            if (Input.GetKeyDown(KeyCode.W)) {
                ToggleBuild();
            }

        }

        #region Build and Destroy

        protected override void BuildSlots() {

            // Initialize the slots UI list
            SlotsUI = new List<SlotUI>(SlotsCount);

            // Loop through the ItemStacks
            for (int i = 0; i < SlotsCount; i++) {

                // Instantiate and initialize a new Slot UI
                SlotUI slotUI = Instantiate(SlotUIPrefab, transform).Initialize(this);

                // Add the slot UI to the slots UI list
                SlotsUI.Add(slotUI);

                // Refresh the slot UI
                slotUI.Refresh();

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

    }

}

#endif