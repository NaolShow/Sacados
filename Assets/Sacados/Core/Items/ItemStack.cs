namespace Sacados.Core.Items {

    public class ItemStack {

        #region Is Properties

        /// <summary>
        /// Determines if the ItemStack is empty (it's Item is null or it's StackSize <= 0)
        /// </summary>
        public bool IsEmpty => (Item == null) || (StackSize <= 0);

        #endregion

        /// <summary>
        /// Item of the ItemStack
        /// </summary>
        public Item Item;

        /// <summary>
        /// Stack size of the ItemStack
        /// </summary>
        public int StackSize;

        #region Constructors

        /// <summary>
        /// Creates an ItemStack with the specified Item and StackSize
        /// </summary>
        public ItemStack(Item item, int stackSize) {

            // Save the Item and the StackSize
            Item = item;
            StackSize = stackSize;

        }

        /// <summary>
        /// Creates an ItemStack with the specified Item and it's MaxStackSize as the StackSize
        /// </summary>
        /// <param name="item"></param>
        public ItemStack(Item item) : this(item, item.MaxStackSize) { }

        #endregion

    }

}