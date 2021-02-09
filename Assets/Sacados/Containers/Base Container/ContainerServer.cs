#if IS_SERVER

using Mirror;
using Sacados.Items;
using Sacados.Slots;
using System.Collections.Generic;
using UnityEngine;

namespace Sacados.Containers {

    public abstract partial class Container : NetworkBehaviour {

        /// <summary>
        /// List containing the container's Slots
        /// </summary>
        public List<Slot> Slots { get; protected set; }

        /// <summary>
        /// Initializes the container with the specified slots count<br/>
        /// Returns true if the container was not already initialized
        /// </summary>
        [Server]
        public bool Initialize(int slotsCount) {

            // If the container is already initialized
            if (Slots != null) return false;

            // Initialize the slots list
            Slots = new List<Slot>(slotsCount);

            // Add null ItemStacks to the list
            ItemStacks.AddRange(new ItemStack[slotsCount]);

            // Create the slots
            CreateSlots();

            return true;

        }

        #region Give and Take

        /// <summary>
        /// Gives the ItemStack to the container and returns the surplus (if there is)
        /// </summary>
        [Server]
        public virtual ItemStack Give(ItemStack itemStack) {

            // Loop through the slots
            for (int i = 0; i < SlotsCount; i++) {

                // Give and save the remaining ItemStack
                itemStack = Slots[i].Give(itemStack);

                // If we have given everything
                if (itemStack.StackSize == 0) return ItemStack.Empty;

            }
            return itemStack;

        }

        /// <summary>
        /// Takes the ItemStack from the container and returns the remaining (if there is)
        /// </summary>
        [Server]
        public virtual ItemStack Take(ItemStack itemStack) {

            // Loop through the slots
            for (int i = 0; i < SlotsCount; i++) {

                // Take and save the remaining ItemStack
                itemStack = Slots[i].Take(itemStack);

                // If we have taken everything
                if (itemStack.StackSize == 0) return ItemStack.Empty;

            }
            return itemStack;

        }

        #endregion

        #region Virtual methods

        /// <summary>
        /// Creates the server's slots (override it to implement your own slots)<br/>
        /// Called from the <see cref="Initialize(int)"/> method
        /// </summary>
        [Server]
        protected virtual void CreateSlots() => Debug.LogWarning($"[Sacados] Method '{nameof(CreateSlots)} is not overrided in the container '{name}' !", this);

        #endregion

    }

}

#endif