#if UNITY_EDITOR

using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Sacados.Editor {

    /// <summary>
    /// Provides a way to display the registered <see cref="Item"/> with a search field
    /// </summary>
    public class ItemDropDown : AdvancedDropdown {

        /// <summary>
        /// Shows all the registered <see cref="Item"/> and call the specified callback when one of them is selected
        /// </summary>
        /// <param name="rect"><see cref="Rect"/> where the control will be shown</param>
        /// <param name="onItemIDSelected">Callback called when an <see cref="Item"/> is selected</param>
        public static void Show(Rect rect, Action<string> onItemIDSelected)
            => new ItemDropDown(new AdvancedDropdownState(), onItemIDSelected).Show(rect);

        private Action<string> onItemIDSelected;

        public ItemDropDown(AdvancedDropdownState state, Action<string> onItemIDSelected) : base(state) {
            this.onItemIDSelected = onItemIDSelected;
        }

        protected override AdvancedDropdownItem BuildRoot() {
            AdvancedDropdownItem root = new AdvancedDropdownItem("Items");
            foreach (Item item in Item.Registry.Values)
                root.AddChild(new AdvancedDropdownItem(item.ID));
            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item) {
            base.ItemSelected(item);
            onItemIDSelected?.Invoke(item.name);
        }

    }

}

#endif