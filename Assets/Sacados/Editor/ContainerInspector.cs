#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Sacados.Editor {

    // Unfortunately, with Unity limitations I can't use the IContainer interface as the base reference
    [CustomEditor(typeof(Container), true)]
    public class ContainerInspector : UnityEditor.Editor {

        private const string buttonText = "Open the Container Inspector";

        public override void OnInspectorGUI() {
            if (GUILayout.Button(buttonText))
                ContainerEditorWindow.Open();
            base.OnInspectorGUI();
        }

    }

}

#endif