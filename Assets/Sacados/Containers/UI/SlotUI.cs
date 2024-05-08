using UnityEngine;

namespace Sacados {

    /// <summary>
    /// Base implementation of <see cref="ISlotUI"/> that is linked to an <see cref="IContainer"/>
    /// </summary>
    public abstract class SlotUI : MonoBehaviour, ISlotUI {

        /// <summary>
        /// <see cref="IContainer"/> that the <see cref="ISlotUI"/> is linked to
        /// </summary>
        protected IContainer Container { get; private set; }
        /// <summary>
        /// <see cref="ISlot"/> index that the <see cref="ISlotUI"/> is linked to
        /// </summary>
        protected int Index { get; private set; }

        public void Configure(IContainer container, int index) {
            Container = container;
            Index = index;

            // Refresh directly the UI
            Refresh();
        }

        public abstract void Refresh();

        public void Destroy() => Destroy(gameObject);

    }

}
