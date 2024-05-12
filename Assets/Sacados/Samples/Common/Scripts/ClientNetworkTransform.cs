using Unity.Netcode.Components;

namespace Sacados.Samples {

    /// <summary>
    /// Client authoritative network transform
    /// </summary>
    public class ClientNetworkTransform : NetworkTransform {

        protected override bool OnIsServerAuthoritative() {
            return false;
        }

    }

}