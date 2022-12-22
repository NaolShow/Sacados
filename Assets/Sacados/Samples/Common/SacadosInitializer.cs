using Sacados.Items;
using UnityEngine;

namespace Sacados.Samples {

    /// <summary>
    /// Handles the registration of a list of <see cref="Item"/>
    /// </summary>
    public class SacadosInitializer : MonoBehaviour {

        [Tooltip("All the Items that you want to register automatically on startup of this component")]
        [SerializeField] private Item[] items;

        private void Awake() {

            // Register all the Items
            foreach (Item item in items)
                Item.Register(item);

        }

    }

}
