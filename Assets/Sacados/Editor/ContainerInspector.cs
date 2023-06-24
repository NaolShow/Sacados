using UnityEditor;
using UnityEngine;

namespace Sacados.Editor {

    // Unfortunately, with Unity limitations I can't use the IContainer interface as the base reference
    [CustomEditor(typeof(Container), true)]
    public class ContainerInspector : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (GUILayout.Button("Open the container's editor window"))
                ContainerEditorWindow.Open();

        }

    }

}