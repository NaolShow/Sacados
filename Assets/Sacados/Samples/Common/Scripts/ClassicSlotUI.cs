using UnityEngine;
using UnityEngine.UI;

namespace Sacados.Samples {

    /// <summary>
    /// Classic <see cref="ISlotUI"/> that only displays the <see cref="Item.Sprite"/> and <see cref="ItemStack.StackSize"/>
    /// </summary>
    public class ClassicSlotUI : SlotUI {

        [SerializeField] private Text stackSizeText;
        [SerializeField] private Image itemSpriteImage;

        public override void Refresh() {

            // Get the ItemStack and determines if it's empty
            ItemStack itemStack = Container[Index];
            bool isEmpty = itemStack.IsEmpty();

#if UNITY_EDITOR
            // Set the slot's name in the hierarchy
            name = $"Slot n°{Index}";
#endif

            // Set the stack size and sprite
            stackSizeText.text = isEmpty ? string.Empty : itemStack.StackSize.ToString();
            itemSpriteImage.sprite = isEmpty ? null : itemStack.Item.Sprite;

        }

    }

}
