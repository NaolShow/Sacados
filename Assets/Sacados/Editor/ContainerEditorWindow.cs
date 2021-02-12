#if UNITY_EDITOR

using Mirror;
using Sacados.Containers;
using Sacados.Items;
using UnityEditor;
using UnityEngine;

namespace Sacados.Editor {

    public class ContainerEditorWindow : EditorWindow {

        /// <summary>
        /// Currently selected container
        /// </summary>
        public Container Container;

        /// <summary>
        /// ID of the Item that should be given when pressing the "S" button
        /// </summary>
        public string ItemIDToGive;

        /// <summary>
        /// Position of the scroll bar
        /// </summary>
        public Vector2 ScrollPos;

        #region Opening / Painting

        public static void Open() => GetWindow<ContainerEditorWindow>("Container").Show();

        public void OnGUI() => ProcessSelectedObject();
        public void OnSelectionChange() => Repaint();

        #endregion

        public void ProcessSelectedObject() {

            // If the user is not in play mode
            if (!EditorApplication.isPlaying) {
                EditorGUILayout.HelpBox("You must be in Play Mode in order to see a container content", MessageType.Info);
                return;
            }

            // If the user is not a client or a server
            if (!NetworkClient.active && !NetworkServer.active) {
                EditorGUILayout.HelpBox("You must be the client or the server to see a container content", MessageType.Info);
                return;
            }

            // If the user hasn't selected an object in the hierarchy
            if (Selection.activeGameObject == null) {
                EditorGUILayout.HelpBox("You must selected a container to see it's content", MessageType.Info);
                return;
            }

            // Get the container
            Container = Selection.activeGameObject.GetComponent<Container>();

            // If it's not a container
            if (Container == null) {
                EditorGUILayout.HelpBox("The selected object is not a container", MessageType.Error);
                return;
            }

            // Display the ItemStacks
            DisplayItemStacks();

        }

        public void DisplayItemStacks() {

            // If the user is the server
            if (NetworkServer.active) {

                #region Item Give Field
                GUILayout.BeginHorizontal(GUI.skin.box);

                GUILayout.Label("Item to Give:", GUILayout.ExpandWidth(false));

                // If the user wants to show the dropdown menu
                if (EditorGUILayout.DropdownButton(new GUIContent(ItemIDToGive), FocusType.Passive)) {

                    // Initialize the dropdown menu
                    GenericMenu menu = new GenericMenu();

                    // Loop through all the registered items
                    foreach (Item item in Item.Items) {

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
            int lineCount = Mathf.CeilToInt(((float)Container.SlotsCount / maximumItemStacksPerLine));

            // Loop through the rows (lines)
            for (int row = 0; row < lineCount; row++) {
                EditorGUILayout.BeginHorizontal();

                // Loop through the columns (itemstacks)
                for (int column = 0; column < (Mathf.Min(maximumItemStacksPerLine, Container.ItemStacks.Count - maximumItemStacksPerLine * row)); column++) {

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
            ItemStack itemStack = Container.ItemStacks[index];

            // Display the itemstack index
            GUILayout.Label($"N°{index}", EditorStyles.miniLabel);

            // If the itemstack is empty
            if (itemStack.IsEmpty) {
                GUILayout.Label("Empty", GUILayout.ExpandWidth(false));
            } else {

                // Display the ItemStack ID
                GUILayout.Label($"ID: {itemStack.Item.ID}", GUILayout.ExpandWidth(false));
                // Display the ItemStack StackSize/MaxStackSize
                GUILayout.Label($"Stack Size: {itemStack.StackSize}/{itemStack.Item.MaxStackSize}", GUILayout.ExpandWidth(false));

            }

#if IS_SERVER

            // If the user is the server
            if (NetworkServer.active) {

                #region Management
                GUILayout.BeginHorizontal(GUI.skin.box);

                // If the user wants to set the item
                if (GUILayout.Button("S")) {

                    // Set the slot ItemStack with max stack size
                    Container.Slots[index].Set(new ItemStack(Item.Get(ItemIDToGive)));

                }
                // If the user wants to increase the stack size by one
                else if (GUILayout.Button("+")) {

                    // Set the item stack size to 1
                    itemStack.StackSize = 1;
                    // Give one item
                    Container.Slots[index].Give(itemStack);

                }
                // If the user wants to decrease the stack size by one
                else if (GUILayout.Button("-")) {

                    // Set the item stack size to 1
                    itemStack.StackSize = 1;
                    // Take one item
                    Container.Slots[index].Take(itemStack);

                }
                // If the user wants to clear the slot
                else if (GUILayout.Button("C")) {

                    // Clear the slot
                    Container.ItemStacks[index] = ItemStack.Empty;

                }

                GUILayout.EndHorizontal();
                #endregion

            }

#endif

            GUILayout.EndVertical();
        }

    }

}

#endif