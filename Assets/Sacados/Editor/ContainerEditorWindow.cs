#if UNITY_EDITOR

using Unity.Netcode;
using UnityEditor;
using UnityEngine;

namespace Sacados.Editor {

    public class ContainerEditorWindow : EditorWindow {

        private const string windowTitle = "Container Inspector";

        private const string mustBeInPlaymode = "You must be in play mode in order to interact with the container";
        private const string mustBeConnected = "You must be connected to a server in order to interact with the container";
        private const string mustSelectContainer = "Select a container to interact with it";

        private const string selectedItemText = "Selected item:";
        private const string xText = "x";

        private const string giveToSlotText = "Give to slot";
        private const string takeFromSlotText = "Take from slot";
        private const string setInSlotText = "Set in slot";
        private const string clearSlotText = "Clear slot";

        private const string giveToContainerText = "Give to container";
        private const string takeFromContainerText = "Take from container";
        private const string clearContainerText = "Clear container";

        private const string emptyText = "Empty";

        private IContainer container;

#if IS_SERVER
        private int? selectedSlot;
        private uint amount = 1;
        private string itemID;
#endif

        private Vector2 scrollPos;
        private bool managerHooked;

        public static void Open() => GetWindow<ContainerEditorWindow>(windowTitle).Show();

        // When the hierarchy selection changes try to find a container
        private void OnEnable() => Selection.selectionChanged += () => Repaint();
        private void OnGUI() => ProcessSelectedObject();

        private void ProcessSelectedObject() {

            // If the game isn't in play mode
            if (!EditorApplication.isPlaying) {
                EditorGUILayout.HelpBox(mustBeInPlaymode, MessageType.Info);
                return;
            }

            // If the user is not a client and not a server (the network isn't started at all)
            if (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient) {
                EditorGUILayout.HelpBox(mustBeConnected, MessageType.Info);
                return;
            }

            // If there is no selected container or the selected object isn't a container
            if (Selection.activeGameObject == null || !Selection.activeGameObject.TryGetComponent(out Container container)) {
                EditorGUILayout.HelpBox(mustSelectContainer, MessageType.Info);
                return;
            }

            // If the selected container is different then reset states
            if (!container.Equals(this.container)) {

                // Refresh the editor's UI if the container has been updated
                if (this.container != null)
                    this.container.OnUpdate -= OnContainerUpdate;
                container.OnUpdate += OnContainerUpdate;

                this.container = container;
                selectedSlot = null;

            }

            // Display the item stacks
            DisplayItemStacks();

        }

        private void OnContainerUpdate(ContainerEventType type, ItemStack oldItemStack, int index) => Repaint();

        private void DisplayItemStacks() {
#if IS_SERVER
            bool isServer = NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost;

            // If we are the server then display the container/slot management menu
            if (isServer) {
                GUILayout.BeginHorizontal(GUI.skin.box);

                GUILayout.BeginVertical();

                // Select the item and the amount that will be given or taken
                GUILayout.BeginHorizontal();
                GUILayout.Label(selectedItemText, GUILayout.ExpandWidth(false));
                if (GUILayout.Button(itemID, EditorStyles.popup)) {
                    Vector2 mousePos = Event.current.mousePosition;
                    Rect rect = new Rect(mousePos, Vector2.zero);
                    ItemDropDown.Show(rect, selectedItemID => itemID = selectedItemID);
                }
                GUILayout.Label(xText, GUILayout.ExpandWidth(false));
                amount = (uint)Mathf.Max(0, EditorGUILayout.IntField((int)amount));
                GUILayout.EndHorizontal();

                // Give, take, set or clear the selected slot
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUI.enabled = selectedSlot != null;
                if (GUILayout.Button(giveToSlotText) && Item.TryGet(itemID, out Item item))
                    container.Get((int)selectedSlot).Give(item.CreateItemStack(amount));
                if (GUILayout.Button(takeFromSlotText) && Item.TryGet(itemID, out item))
                    container.Get((int)selectedSlot).Take(item.CreateItemStack(amount));
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                if (GUILayout.Button(setInSlotText) && Item.TryGet(itemID, out item))
                    container.Get((int)selectedSlot).ItemStack = item.CreateItemStack(amount);
                if (GUILayout.Button(clearSlotText))
                    container.Get((int)selectedSlot).Clear();
                GUI.enabled = true;
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();

                // Give or take a specific amount from the container or clear it
                GUILayout.BeginVertical(GUI.skin.box);
                if (GUILayout.Button(giveToContainerText) && Item.TryGet(itemID, out item))
                    container.Give(item.CreateItemStack(amount));
                if (GUILayout.Button(takeFromContainerText) && Item.TryGet(itemID, out item))
                    container.Take(item.CreateItemStack(amount));
                if (GUILayout.Button(clearContainerText))
                    container.Clear();
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }
#endif

            // Begin and save the scroll view position
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            for (int i = 0; i < container.SlotsCount; i++) {

                ItemStack itemStack = container[i];
                bool isEmpty = itemStack.IsEmpty();

                GUIStyle selectedBoxStyle = new GUIStyle(GUI.skin.box);
#if IS_SERVER
                // If we are the server and a slot is selected
                if (isServer && selectedSlot == i) {
                    Texture2D texture = new Texture2D(1, 1);
                    texture.SetPixel(0, 0, Color.gray);
                    texture.Apply();

                    selectedBoxStyle.normal.background = selectedBoxStyle.onNormal.background = texture;
                    selectedBoxStyle.normal.scaledBackgrounds = selectedBoxStyle.onNormal.scaledBackgrounds = null;
                }
#endif

                // Slot's index, item id, stacksize and Item/ItemStack class name
                GUILayout.BeginVertical(selectedBoxStyle);
                GUILayout.Label($"n°{i}");
                GUILayout.Label(isEmpty ? emptyText : $"{itemStack.Item.ID} ({itemStack.StackSize}/{itemStack.Item.MaxStackSize})");
                if (!isEmpty)
                    GUILayout.Label($"Item: {itemStack.Item.GetType().Name} ; ItemStack: {itemStack.GetType().Name}");
                GUILayout.EndVertical();

#if IS_SERVER
                // If we are the server allow to slot's selection
                Event currentEvent = Event.current;
                if (isServer && currentEvent.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(currentEvent.mousePosition)) {
                    selectedSlot = i;
                    Repaint();
                }
#endif

            }

            GUILayout.EndScrollView();

        }

    }

}

#endif