#if IS_CLIENT

using Mirror;
using Sacados.Containers;
using Sacados.Items;
using UnityEngine;

namespace Sacados.Slots {

    public abstract class SlotUI : MonoBehaviour {

        #region Container

        public GUIContainer Container { get; private set; }
        public int Index { get; private set; }

        #endregion

        protected ItemStack ItemStack => Container.ItemStacks[Index];

        [Client]
        public SlotUI Initialize(GUIContainer container, int index) {

            // Save the slot container and index
            Container = container;
            Index = index;

            // Refresh the UI
            Refresh(ItemStack);

            return this;

        }

        #region Virtual methods

        /// <summary>
        /// Refreshes the slot UI (override it to implement your own slot UI)<br/>
        /// Called from the OnItemStacksUpdate method
        /// </summary>
        [Client]
        public virtual void Refresh(ItemStack itemStack) { }

        #endregion

    }

}

#endif