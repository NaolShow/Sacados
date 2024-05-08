using Unity.Netcode;
using UnityEngine;

namespace Sacados.Samples {

    /// <summary>
    /// Handles a basic Unity GUI that allows the user to start and stop the network
    /// </summary>
    public class NetworkGUI : MonoBehaviour {

        private void OnGUI() {

            // If we are connected to a server
            if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer) {

                GUILayout.BeginVertical();

                // Show the connection status "connected" or "connecting" and the shutdown button
                if (!NetworkManager.Singleton.IsServer)
                    GUILayout.Label(NetworkManager.Singleton.IsConnectedClient ? "Connected" : "Connecting");
                if (GUILayout.Button("Shutdown"))
                    NetworkManager.Singleton.Shutdown();

                GUILayout.EndVertical();

            } else {

                GUILayout.BeginHorizontal();

                // Show the host and the connect button
                if (GUILayout.Button("Host"))
                    NetworkManager.Singleton.StartHost();
                else if (GUILayout.Button("Client"))
                    NetworkManager.Singleton.StartClient();

                GUILayout.EndHorizontal();

            }

        }

    }

}