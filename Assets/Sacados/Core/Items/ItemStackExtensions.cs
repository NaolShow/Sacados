namespace Sacados.Core.Items {

    public static class ItemStackExtensions {

        /// <summary>
        /// Determines if the ItemStack is empty (it's Item is null or it's StackSize = 0)
        /// </summary>
        public static bool IsEmpty(this ItemStack itemStack) {
            return (itemStack == null) || (itemStack.Item == null) || (itemStack.StackSize == 0);
        }

    }

}