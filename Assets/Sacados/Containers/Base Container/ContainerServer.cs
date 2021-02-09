#if IS_SERVER

using Mirror;
using Sacados.Items;
using Sacados.Slots;
using System.Collections.Generic;

namespace Sacados.Containers {

    public partial class Container : NetworkBehaviour {

        /// <summary>
        /// List containing the container's Slots
        /// </summary>
        public List<Slot> Slots { get; protected set; }

        /// <summary>
        /// Initializes the container with the specified slots count
        /// </summary>
        [Server]
        protected virtual Container Initialize(int slotsCount) {

            // If the container is already initialized
            if (Slots != null) return this;

            // Initialize the slots list
            Slots = new List<Slot>(slotsCount);

            // Add null ItemStacks to the list
            ItemStacks.AddRange(new ItemStack[slotsCount]);

            // Loop through the slots
            for (int i = 0; i < slotsCount; i++) {

                // Add a basic slot
                Slots.Add(new Slot(this, i));

            }

            return this;

        }

        #region Management

        /// <summary>
        /// Gives the ItemStack to the container and returns the surplus (if there is)
        /// </summary>
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

    }

}

#endif