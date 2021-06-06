#if IS_CLIENT

using MLAPI.NetworkVariable.Collections;
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
        protected virtual void OnContainerUpdate(NetworkListEvent<ItemStack> e) {

            switch (e.Type) {

                // If an ItemStack has been overwritten
                case NetworkListEvent<ItemStack>.EventType.Value:

                    // Refresh the corresponding UI Slot
                    SlotsUI[e.Index].Refresh(e.Value);
                    break;

            }

        }

        #region Build and Unbuild

        /// <summary>
        /// Toggles the container visibility, build it when it's not and unbuild it when it's already built
        /// </summary>
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
        public bool Build() {

            // If the container is already built
            if (IsBuilt) return false;

            // Subscribe to the ItemStacks callback
            ItemStacks.OnListChanged += OnContainerUpdate;

            // Call the generate slots method
            BuildSlots();

            return true;

        }

        /// <summary>
        /// Unbuilds the container<br/>
        /// Returns true if the container has been unbuilt and was built
        /// </summary>
        public bool Unbuild() {

            // If the container is already unbuilt
            if (!IsBuilt) return false;

            // Unsubscribe from the ItemStacks callback
            ItemStacks.OnListChanged -= OnContainerUpdate;

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
        protected virtual void BuildSlots() => Debug.LogWarning($"[Sacados] Method '{nameof(BuildSlots)}' is not overrided in the container '{name}' !", this);

        /// <summary>
        /// Destroys the client's slots (override it to implement your own destroy method)<br/>
        /// Called from the <see cref="Unbuild"/> method
        /// </summary>
        protected virtual void DestroySlots() => Debug.LogWarning($"[Sacados] Method '{nameof(DestroySlots)}' is not overrided in the container '{name}' !", this);

        #endregion

        private void OnDestroy() {

            // If the client is not active
            if (!NetworkManager.IsConnectedClient) return;

            // Unbuild the container
            Unbuild();

        }

    }

}

#endif