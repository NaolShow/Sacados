using MLAPI.Serialization;

namespace Sacados.Items {

    public struct ItemStack : INetworkSerializable {

        #region Static

        /// <summary>
        /// Empty ItemStack used to give a default value
        /// </summary>
        public static readonly ItemStack Empty = new ItemStack(null, 0);

        #endregion

        #region Is Properties

        /// <summary>
        /// Determines if the ItemStack is empty (if the Item is null or it's StackSize = 0)
        /// </summary>
        public bool IsEmpty => (Item == null) || (StackSize == 0);

        #endregion

        /// <summary>
        /// Item of the ItemStack
        /// </summary>
        public Item Item;

        /// <summary>
        /// Stack size of the ItemStack
        /// </summary>
        public uint StackSize;

        #region Constructors

        /// <summary>
        /// Creates an ItemStack with the specified Item and StackSize
        /// </summary>
        public ItemStack(Item item, uint stackSize) : this() {

            // Save the Item and the StackSize
            Item = item;
            StackSize = stackSize;

        }

        /// <summary>
        /// Creates an ItemStack with the specified Item and it's MaxStackSize as the StackSize
        /// </summary>
        public ItemStack(Item item) : this(item, (item == null) ? 0 : item.MaxStackSize) { }

        #endregion

        #region Is Methods

        /// <summary>
        /// Determines if the ItemStacks are the same
        /// </summary>
        public bool IsSameAs(ItemStack itemStack) {
            return itemStack.Item == Item;
        }

        /// <summary>
        /// Determines if the ItemStacks are the same or if the specified item stack is empty
        /// </summary>
        public bool IsSameOrEmpty(ItemStack itemStack) {
            return itemStack.IsEmpty || IsSameAs(itemStack);
        }


        #endregion

        public void NetworkSerialize(NetworkSerializer serializer) {

            // If MLAPI is deserializing the itemstack
            if (serializer.IsReading) {

                // Call the deserializing method
                Deserialize(serializer.Reader);
                return;

            }

            // Call the serialize method
            Serialize(serializer.Writer);

        }

        /// <summary>
        /// Writes the ItemStack to the NetworkSerializer
        /// </summary>
        private void Serialize(NetworkWriter writer) {

            // If the ItemStack is empty
            if (IsEmpty) {

                // Write an empty item ID
                writer.WriteString(null);
                return;

            }

            // Write the Item ID
            writer.WriteString(Item.ID);

            // Write the stack size
            writer.WriteUInt32(StackSize);

        }

        /// <summary>
        /// Reads the ItemStack from the NetworkSerializer
        /// </summary>
        private void Deserialize(NetworkReader reader) {

            // Read the item's ID
            string id = reader.ReadString().ToString();

            // If the itemstack is null
            if (id == null) {

                Item = null;
                return;

            }

            // Get the item
            Item = Item.Get(id);

            // Read the stack size
            StackSize = reader.ReadUInt32();

        }

    }

}