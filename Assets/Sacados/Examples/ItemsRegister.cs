using Sacados.Items;
using UnityEngine;

namespace Sacados.Examples {

    public class ItemsRegister : MonoBehaviour {

        public Item[] ItemsToRegister;

        private void Awake() {

            // Loop through the Items to register
            foreach (Item item in ItemsToRegister) {

                // Register the Items
                Item.Register(item);
                item.OnRegister();

            }

            // Clear the items to register array
            ItemsToRegister = null;

        }

    }

}
