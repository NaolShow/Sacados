using FishNet;
using FishNet.Managing;
using UnityEngine;

namespace Sacados.Samples {

    /// <summary>
    /// Handles a basic Unity GUI that allows the user to start and stop the network
    /// </summary>
    public class NetworkGUI : MonoBehaviour {

        private NetworkManager networkManager;

        private void Start() {
            networkManager = InstanceFinder.NetworkManager;
        }

        private void OnGUI() {

            // TODO: Rework
            // => https://github.com/FirstGearGames/FishNet/blob/main/Assets/FishNet/Demos/Scripts/NetworkHudCanvases.cs

            // If we are connected to a server
            if (networkManager.IsClientStarted || networkManager.IsServerStarted) {
                GUILayout.BeginVertical();

                if (GUILayout.Button("Shutdown")) {
                    networkManager.ServerManager.StopConnection(true);
                    networkManager.ClientManager.StopConnection();
                }

                GUILayout.EndVertical();

            } else {

                GUILayout.BeginHorizontal();

                // Show the host and the connect button
                if (GUILayout.Button("Host")) {
                    networkManager.ServerManager.StartConnection();
                    networkManager.ClientManager.StartConnection();
                } else if (GUILayout.Button("Client"))
                    networkManager.ClientManager.StartConnection();

                GUILayout.EndHorizontal();

            }

        }

    }

}