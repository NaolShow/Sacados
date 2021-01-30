using Mirror;
using Sacados.Core.Items;

namespace Sacados.Core.Containers {

    public partial class Container : NetworkBehaviour {

        /// <summary>
        /// List containing the container's ItemStacks
        /// </summary>
        public SyncList<ItemStack> ItemStacks { get; } = new SyncList<ItemStack>();

        /// <summary>
        /// Determines how many slots the container have
        /// </summary>
        public int SlotsCount => ItemStacks.Count;

    }

}
