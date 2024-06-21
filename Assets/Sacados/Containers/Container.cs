using System.Collections.Generic;
using UnityEngine;

namespace Sacados {

    /**
     * Voir pour le forcing de l'ordre des updates
     *  => Si le container est bien le premier à être appelé, et que faire si on veut pas que ça se propage?
     * Voir si on peut supprimer l'event "full"
     * Voir si on doit pas supprimer l'event "clear" en fin de vie du conteneur (si on stocke?)
     * Voir comment stocker et reprendre une sauvegarde facilement => généralisable avec IList
     * 
     * Version avec Netcode d'Unity
     **/

    /// <summary>
    /// Basic implementation of <see cref="IContainer"/>
    /// </summary>
    public abstract class Container : MonoBehaviour, IContainer {

        public int Size => Storage.Count;
        private readonly List<ISlot> slots = new List<ISlot>();
        public ISlot GetSlot(int index) => slots[index];

        public ItemStack this[int i] {
            get => Storage[i];
            set => Storage[i] = value;
        }

        // TODO: Editor extension to allow drag & drop of the interface
        public IContainerStorage Storage { get; private set; }
        protected virtual void Awake() => Storage = GetComponent<IContainerStorage>();

        #region Slots Management

        /// <summary>
        /// Adds the specified <see cref="ISlot"/> to the <see cref="Container"/>
        /// </summary>
        /// <param name="slot">The <see cref="ISlot"/> that will be added</param>
        /// <param name="itemStack">The <see cref="ItemStack"/> that will be added</param>
        protected void AddSlot(ISlot slot, ItemStack itemStack = null) {

            // Insert the slot and also it's ItemStack if we are the server and spawned
            slots.Insert(slot.Index, slot);
            if (!Storage.IsReadOnly) Storage.Insert(slot.Index, itemStack?.Clone());

            // Reorder the slots indexes only if the added slot isn't at the end
            for (int i = slot.Index; i < slots.Count; i++)
                slots[i].Index = i;

        }

        /// <summary>
        /// Removes the <see cref="ISlot"/> at the specified index from the <see cref="Container"/>
        /// </summary>
        /// <param name="index">The index of the <see cref="ISlot"/> that will be removed</param>
        protected void RemoveSlot(int index) {

            // Remove the slot and also it's ItemStack if we are the server and spawned
            slots.RemoveAt(index);
            if (!Storage.IsReadOnly) Storage.RemoveAt(index);

            // Reorder the slots indexes only if the removed slot isn't at the end
            for (int i = index; i < slots.Count; i++)
                slots[i].Index = i;

        }

        /// <summary>
        /// Clears all the <see cref="ISlot"/> from the <see cref="Container"/>
        /// </summary>
        protected void ClearSlots() {

            // Clear the slots and also the ItemStacks if we are the server and spawned
            slots.Clear();
            if (Storage.IsReadOnly) Storage.Clear();

        }

        #endregion

        // User implementation
        public abstract void Give(ItemStack itemStack);
        public abstract void Take(ItemStack itemStack);

        // By default accept to give and take any ItemStack
        public virtual bool CanBeGiven(ItemStack itemStack) => true;
        public virtual bool CanBeTaken(ItemStack itemStack) => true;

        public abstract void Clear();

    }

}