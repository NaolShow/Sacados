#if IS_CLIENT

using Mirror;
using Sacados.Containers;
using Sacados.Items;
using UnityEngine;

namespace Sacados.Slots {

    public abstract class SlotUI : MonoBehaviour {

        #region Container

        public GUIContainer Container { get; private set; }
        public int Index => Container.SlotsUI.IndexOf(this);

        #endregion

        protected ItemStack ItemStack => Container.ItemStacks[Index];

        [Client]
        public SlotUI Initialize(GUIContainer container) {

            // Save the slot container
            Container = container;

            return this;

        }

        #region Virtual methods

        /// <summary>
        /// Refreshes the slot UI (override it to implement your own slot UI)<br/>
        /// Called from the OnItemStacksUpdate method
        /// </summary>
        [Client]
        public virtual void Refresh(ItemStack itemStack) => Debug.LogWarning($"[Sacados] Method '{nameof(Refresh)} is not overrided in the slot UI '{name}' !", this);

        /// <summary>
        /// Refreshes the slot UI with it's current ItemStack<br/>
        /// </summary>
        public void Refresh() => Refresh(ItemStack);

        #endregion

    }

}

#endif