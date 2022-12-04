using Sacados.Items;
using Unity.Netcode;

namespace Sacados.Containers {

    public abstract partial class Container : NetworkBehaviour {

        /// <summary>
        /// List containing the container's ItemStacks
        /// </summary>
        public NetworkList<ItemStack> ItemStacks { get; } = new NetworkList<ItemStack>();

        /// <summary>
        /// Determines how many slots the container have
        /// </summary>
        public int SlotsCount => ItemStacks.Count;

    }

}
