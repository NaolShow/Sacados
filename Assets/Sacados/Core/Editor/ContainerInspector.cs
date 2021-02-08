#if UNITY_EDITOR

using Sacados.Core.Containers;
using UnityEditor;
using UnityEngine;

namespace Sacados.Core.Editor {

    [CustomEditor(typeof(Container))]
    public class ContainerInspector : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            // If the user wants to open the container window
            if (GUILayout.Button("Open the container Editor Window")) {

                // Open the editor window
                ContainerEditorWindow.Open();

            }

        }

    }

}

#endif