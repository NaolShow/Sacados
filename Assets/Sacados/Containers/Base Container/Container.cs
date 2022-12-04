using Sacados.Items;
using Unity.Netcode;

namespace Sacados.Containers {

    public abstract partial class Container : NetworkBehaviour {

        /// <summary>
        /// List containing the container's ItemStacks
        /// </summary>
        public NetworkStandardList<ItemStack> ItemStacks { get; } = new NetworkStandardList<ItemStack>();

        /// <summary>
        /// Determines how many slots the container have
        /// </summary>
        public int SlotsCount => ItemStacks.Count;

    }

}
