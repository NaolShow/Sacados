﻿#if IS_CLIENT

using Mirror;
using Sacados.Items;
using Sacados.Slots;
using System.Collections.Generic;
using UnityEngine;

namespace Sacados.Containers {

    public abstract partial class GUIContainer : Container {

        /// <summary>
        /// List containing the container's UI Slots
        /// </summary>
        public List<SlotUI> SlotsUI { get; protected set; }

        /// <summary>
        /// Determines if the container is built
        /// </summary>
        public bool IsBuilt => SlotsUI != null;

        /// <summary>
        /// Called when an ItemStack is updated by the server. This is called only when the container is built and before the internal method (so before it tries to refresh the slotUI)<br/>
        /// </summary>
        protected virtual void OnContainerUpdate(SyncList<ItemStack>.Operation operation, int index, ItemStack oldItemStack, ItemStack itemStack) {

            switch (operation) {

                // If an ItemStack has been overwriten
                case SyncList<ItemStack>.Operation.OP_SET:

                    // Refresh the corresponding UI Slot
                    SlotsUI[index].Refresh(itemStack);
                    break;

            }

        }

        #region Build and Unbuild

        /// <summary>
        /// Toggles the container visibility, build it when it's not and unbuild it when it's already built
        /// </summary>
        [Client]
        public void ToggleBuild() {

            // If the container is built
            if (IsBuilt) {

                // Unbuild the container
                Unbuild();

            } else {

                // Build the container
                Build();

            }

        }

        /// <summary>
        /// Builds the container<br/>
        /// Returns true if the container has been built and was not built
        /// </summary>
        [Client]
        public bool Build() {

            // If the container is already built
            if (IsBuilt) return false;

            // Subscribe to the ItemStacks callback
            ItemStacks.Callback += OnContainerUpdate;

            // Call the generate slots method
            BuildSlots();

            return true;

        }

        /// <summary>
        /// Unbuilds the container<br/>
        /// Returns true if the container has been unbuilt and was built
        /// </summary>
        [Client]
        public bool Unbuild() {

            // If the container is already unbuilt
            if (!IsBuilt) return false;

            // Unsubscribe from the ItemStacks callback
            ItemStacks.Callback -= OnContainerUpdate;

            // Call the destroy slots method
            DestroySlots();

            // Clear the slots UI list
            SlotsUI = null;

            return true;

        }

        #endregion

        #region Virtual methods

        /// <summary>
        /// Builds the client's slots (override it to implement your own build method)<br/>
        /// Called from the <see cref="Build"/> method
        /// </summary>
        [Client]
        protected virtual void BuildSlots() => Debug.LogWarning($"[Sacados] Method '{nameof(BuildSlots)}' is not overrided in the container '{name}' !", this);

        /// <summary>
        /// Destroys the client's slots (override it to implement your own destroy method)<br/>
        /// Called from the <see cref="Unbuild"/> method
        /// </summary>
        [Client]
        protected virtual void DestroySlots() => Debug.LogWarning($"[Sacados] Method '{nameof(DestroySlots)}' is not overrided in the container '{name}' !", this);

        #endregion

        public override void OnStopClient() {

            // If the client is not active
            if (!NetworkClient.active) return;

            // Unbuild the container
            Unbuild();

        }

    }

}

#endif