using Sacados.Items;
using Sacados.Slots;
using UnityEngine.UI;

namespace Sacados.Examples.FlexibleContainer {

    public class FlexibleSlotUI : SlotUI {

        public Text ItemIDText;
        public Text ItemStackSizeText;

        public override void Refresh(ItemStack itemStack) {

            // If the ItemStack is empty
            if (itemStack.IsEmpty) return;

            // Set the Item Stack Size
            ItemStackSizeText.text = itemStack.StackSize.ToString();

            // Set the Item ID
            ItemIDText.text = itemStack.Item.ID;

        }

    }

}