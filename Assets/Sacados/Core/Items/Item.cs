using UnityEngine;

namespace Sacados.Core.Items {

#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New Item", menuName = "Sacados/Item")]
#endif
    public class Item : ScriptableObject {

        /// <summary>
        /// ID of the Item
        /// </summary>
        public string ID;

        /// <summary>
        /// Max stack size of the Item
        /// </summary>
        public int MaxStackSize;

    }

}