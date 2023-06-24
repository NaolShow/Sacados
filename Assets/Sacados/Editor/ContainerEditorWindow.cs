using Unity.Netcode;
using UnityEditor;
using UnityEngine;

namespace Sacados.Editor {

    public class ContainerEditorWindow : EditorWindow {

        private IContainer container;
        public string ItemIDToGive;
        public Vector2 ScrollPos;

        public static void Open() => GetWindow<ContainerEditorWindow>("Container").Show();

        private void Update() => Repaint();
        public void OnGUI() => ProcessSelectedObject();

        public void ProcessSelectedObject() {

            // If the game isn't in play mode
            if (!EditorApplication.isPlaying) {
                EditorGUILayout.HelpBox("You must be in play mode in order to interact with the container", MessageType.Info);
                return;
            }

            // If the user is not a client and not a server (the network isn't started at all)
            if (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient) {
                EditorGUILayout.HelpBox("You must be connected to a server in order to interact with the container", MessageType.Info);
                return;
            }

            // If there is no selected container or the selected object isn't a container
            if (Selection.activeGameObject == null || !Selection.activeGameObject.TryGetComponent(out container)) {
                EditorGUILayout.HelpBox("Select a container to interact with it", MessageType.Info);
                return;
            }

            // Display the item stacks
            DisplayItemStacks();

        }

        public void DisplayItemStacks() {

            // If the user is the server
            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost) {

                #region Item Give Field
                GUILayout.BeginHorizontal(GUI.skin.box);

                GUILayout.Label("Item to Give:", GUILayout.ExpandWidth(false));

                // If the user wants to show the dropdown menu
                if (EditorGUILayout.DropdownButton(new GUIContent(ItemIDToGive), FocusType.Passive)) {

                    // Initialize the dropdown menu
                    GenericMenu menu = new GenericMenu();

                    // Loop through all the registered items
                    foreach (Item item in Item.Registry.Values) {

                        // Add the item id to the menu
                        menu.AddItem(new GUIContent(item.ID), ItemIDToGive == item.ID, () => ItemIDToGive = item.ID);

                    }

                    // Show the menu
                    menu.ShowAsContext();

                }

                GUILayout.EndHorizontal();
                #endregion

            }

            // Begin and save the scroll view position
            ScrollPos = GUILayout.BeginScrollView(ScrollPos);

            // Determines how many item stacks should be displayed on one line
            const int maximumItemStacksPerLine = 5;

            // Determines how many lines is needed to display all the itemstacks
            int lineCount = Mathf.CeilToInt(((float)container.SlotsCount / maximumItemStacksPerLine));

            // Loop through the rows (lines)
            for (int row = 0; row < lineCount; row++) {
                EditorGUILayout.BeginHorizontal();

                // Loop through the columns (itemstacks)
                for (int column = 0; column < (Mathf.Min(maximumItemStacksPerLine, container.SlotsCount - maximumItemStacksPerLine * row)); column++) {

                    // Display the ItemStack card
                    DisplayItemStackCard(column + (row * maximumItemStacksPerLine));

                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

        }

        public void DisplayItemStackCard(int index) {
            GUILayout.BeginVertical(GUI.skin.box);

            // Get the itemstack
            ItemStack itemStack = container[index].IsEmpty() ? null : container[index].Clone();

            // Display the itemstack index
            GUILayout.Label($"N°{index}", EditorStyles.miniLabel);

            // If the itemstack is empty
            if (itemStack.IsEmpty()) {
                GUILayout.Label("Empty", GUILayout.ExpandWidth(false));
            } else {

                // Display the ItemStack ID
                GUILayout.Label($"ID: {itemStack.Item.ID}", GUILayout.ExpandWidth(false));
                // Display the ItemStack StackSize/MaxStackSize
                GUILayout.Label($"Stack Size: {itemStack.StackSize}/{itemStack.Item.MaxStackSize}", GUILayout.ExpandWidth(false));

            }

#if IS_SERVER

            // If the user is the server
            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost) {

                #region Management
                GUILayout.BeginHorizontal(GUI.skin.box);

                // If the user wants to set the item
                if (GUILayout.Button("S")) {

                    // Set the slot ItemStack with max stack size
                    container.Get(index).ItemStack = Item.Get(ItemIDToGive).CreateItemStack();

                }
                // If the user wants to increase the stack size by one
                else if (GUILayout.Button("+")) {

                    // Set the item stack size to 1
                    itemStack.StackSize = 1;
                    // Give one item
                    container.Get(index).Give(itemStack);

                }
                // If the user wants to decrease the stack size by one
                else if (GUILayout.Button("-")) {

                    // Set the item stack size to 1
                    itemStack.StackSize = 1;
                    // Take one item
                    container.Get(index).Take(itemStack);

                }
                // If the user wants to clear the slot
                else if (GUILayout.Button("C")) {

                    // Clear the slot
                    container.Get(index).ItemStack = null;

                }

                GUILayout.EndHorizontal();
                #endregion

            }

#endif

            GUILayout.EndVertical();
        }

    }

}