#if IS_CLIENT

using Sacados.Items;
using Sacados.Slots;
using UnityEngine.UI;

namespace Sacados.Examples.GUI {

    public class FixedSlotUI : SlotUI {

        public Text StackSizeText;
        public Image ItemSpriteImage;

        public override void Refresh(ItemStack itemStack) {

            // Set the stack size
            StackSizeText.text = itemStack.StackSize.ToString();

            // Set the item sprite
            ItemSpriteImage.sprite = (itemStack.IsEmpty) ? null : itemStack.Item.Sprite;

            // Enable or disable the stack size (enabled if StackSize > 1)
            StackSizeText.enabled = itemStack.StackSize > 1;

            // Enable or disable the item sprite (enabled if itemstack is not empty)
            ItemSpriteImage.enabled = !itemStack.IsEmpty;


        }

    }

}

#endif