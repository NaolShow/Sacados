#if IS_SERVER

using MLAPI;
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
        public bool Initialize(int slotsCount) {

            // If the container is already initialized
            if (Slots != null) return false;

            // Initialize the slots list
            Slots = new List<Slot>(slotsCount);

            // If there is no slots to create (if it's a flexible sized container)
            if (slotsCount == 0) return true;

            // Add null ItemStacks to the list
            for (int i = 0; i < slotsCount; i++) ItemStacks.Add(ItemStack.Empty);

            // Create the slots
            CreateSlots();

            return true;

        }

        #region Give and Take

        /// <summary>
        /// Gives the ItemStack to the container and returns the surplus (if there is)
        /// </summary>
        public virtual ItemStack Give(ItemStack itemStack) {

            // Loop through the slots
            foreach (Slot slot in Slots.ToArray()) {

                // Give and save the remaining ItemStack
                itemStack = slot.Give(itemStack);

                // If we have given everything
                if (itemStack.StackSize == 0) return ItemStack.Empty;

            }

            // Call the on Overflow method
            return OnOverflow(itemStack);

        }

        /// <summary>
        /// Takes the ItemStack from the container and returns the remaining (if there is)
        /// </summary>
        public virtual ItemStack Take(ItemStack itemStack) {

            // Loop through the slots
            foreach (Slot slot in Slots.ToArray()) {

                // Take and save the remaining ItemStack
                itemStack = slot.Take(itemStack);

                // If we have taken everything
                if (itemStack.StackSize == 0) return ItemStack.Empty;

            }

            // Call the on Empty method
            return OnEmpty(itemStack);

        }

        #endregion

        #region Virtual methods

        /// <summary>
        /// Creates the server's slots (override it to implement your own slots)<br/>
        /// Called from the <see cref="Initialize(int)"/> method
        /// </summary>
        protected virtual void CreateSlots() => Debug.LogWarning($"[Sacados] Method '{nameof(CreateSlots)} is not overrided in the container '{name}' !", this);

        /// <summary>
        /// Called when the container is trying to add an ItemStack but is completely filled<br/>
        /// In here you can do what you want with the surplus. But don't forget to return the surplus after your operations<br/>
        /// Called from the <see cref="Give(ItemStack)"/> method
        /// </summary>
        protected virtual ItemStack OnOverflow(ItemStack itemStack) => itemStack;
        /// <summary>
        /// Called when the container is trying to take an ItemStack but is completely empty<br/>
        /// In here you can do what you want with the remaining. But don't forget to return the surplus after your operations<br/>
        /// Called from the <see cref="Give(ItemStack)"/> method
        /// </summary>
        protected virtual ItemStack OnEmpty(ItemStack itemStack) => itemStack;

        /// <summary>
        /// Called when the container has filled completely a slot
        /// </summary>
        public virtual void OnSlotOverflow(Slot slot) { }
        /// <summary>
        /// Called when the container has emptied completely a slot<br/>
        /// In here you could for example remove the slot (if you want to make a flexible container)
        /// </summary>
        public virtual void OnSlotEmpty(Slot slot) { }

        #endregion

    }

}

#endif