using System.Collections.Generic;
using UnityEngine;

namespace Sacados.Samples {

    /// <summary>
    /// Classic implementation of <see cref="IContainerUI"/><br/>
    /// (Supports <b>fixed</b> and <b>flexible</b> sized containers)
    /// </summary>
    public class ClassicContainerUI : MonoBehaviour, IContainerUI {

        [field: SerializeField] protected Transform SlotsParent { get; private set; }
        [field: SerializeField] protected SlotUI SlotUIPrefab { get; private set; }

        public IContainer Container { get; private set; }
        /// <summary>
        /// Contains all the created <see cref="ISlotUI"/>
        /// </summary>
        protected List<ISlotUI> Slots { get; private set; } = new List<ISlotUI>();

        public bool IsBuilt { get; private set; }

        protected virtual void Awake() {
            Container = GetComponent<IContainer>();
            Container.OnStarted += OnContainerStarted;
            Container.OnStopped += OnContainerStopped;
        }

        protected virtual void OnDestroy() {
            Container.OnStarted -= OnContainerStarted;
            Container.OnStopped -= OnContainerStopped;
        }

        /// <inheritdoc cref="IContainer.OnStarted"/>
        protected virtual void OnContainerStarted() => Build();
        /// <inheritdoc cref="IContainer.OnStopped"/>
        protected virtual void OnContainerStopped() => Unbuild();

        // If either the container is built before it is ready
        // Or the container is just flexible and can have runtime changes
        private void OnContainerUpdate(ContainerEventType type, ItemStack oldItemStack, int index) {

            switch (type) {

                // Add, remove and clear the slots
                case ContainerEventType.Add: CreateSlot(index); break;
                case ContainerEventType.Remove: RemoveSlot(index); break;
                case ContainerEventType.Clear: UnbuildSlots(); break;
                // Refresh the updated slot
                case ContainerEventType.Value: Slots[index].Refresh(); break;

            }
        }

        public void Build() {

            // If the container is built
            if (IsBuilt) return;
            IsBuilt = true;

            // Build the slots and subscribe to the update event
            BuildSlots();
            Container.OnUpdate += OnContainerUpdate;

        }

        public void Unbuild() {

            // If the container is not built
            if (!IsBuilt) return;
            IsBuilt = false;

            // Unbuild the slots and unsubscribe from the update event
            UnbuildSlots();
            Container.OnUpdate -= OnContainerUpdate;

        }

        public void Toggle() {
            if (IsBuilt) Unbuild();
            else Build();
        }

        /// <summary>
        /// Builds the <see cref="ISlotUI"/> for the <see cref="IContainer"/>
        /// </summary>
        protected virtual void BuildSlots() {

            // Loop through all the container's stacks and instantiate their slot ui
            for (int i = 0; i < Container.SlotsCount; i++)
                CreateSlot(i);

        }

        /// <summary>
        /// Unbuilds the <see cref="ISlotUI"/> of the <see cref="IContainer"/>
        /// </summary>
        protected virtual void UnbuildSlots() {

            // Loop through all the slots UI and destroy them
            foreach (ISlotUI slotUI in Slots)
                slotUI.Destroy();
            Slots.Clear();

        }

        /// <summary>
        /// Instantiates a <see cref="ISlotUI"/> for the specified <see cref="ISlot.Index"/>
        /// </summary>
        /// <param name="index">Index of the <see cref="ISlot"/> that is created</param>
        protected virtual void CreateSlot(int index) {

            // Instantiate the slot prefab and configure it
            ISlotUI instance = Instantiate(SlotUIPrefab, SlotsParent, false);
            instance.Configure(Container, index);
            Slots.Insert(index, instance);

            // Reorder the slots indexes only if the added slot isn't at the end
            for (int i = index; i < Slots.Count; i++)
                Slots[i].Configure(Container, i);

        }

        /// <summary>
        /// Destroys a <see cref="ISlotUI"/> of the specified <see cref="ISlot.Index"/>
        /// </summary>
        /// <param name="index">Index of the <see cref="ISlot"/> that is removed</param>
        protected virtual void RemoveSlot(int index) {

            // Destroy the slot and remove it from the slots list
            Slots[index].Destroy();
            Slots.RemoveAt(index);

            // Reorder the slots indexes only if the removed slot isn't at the end
            for (int i = index; i < Slots.Count; i++)
                Slots[i].Configure(Container, i);

        }

    }

}