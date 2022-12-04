using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sacados.Examples {

    public class NetworkUI : MonoBehaviour {

        public void StartHost() => NetworkManager.Singleton.StartHost();
        public void Connect() => NetworkManager.Singleton.StartClient();

        public string SceneName;

        // Switch to another scene when the server is started
        public void Start() => NetworkManager.Singleton.OnServerStarted += () => NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);

    }

}