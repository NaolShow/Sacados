# Sacados
---
                                                                        
> Multiplayer inventory system using Mirror

Sacados is a complete multiplayer inventory system for Unity made using Mirror

# :rocket: Quick example
---

Here's a quick example on how to use Sacados.
In this example we are creating an inventory with no GUI (most basic one)

```cs

using Mirror;
using Sacados.Core.Containers;
using Sacados.Core.Items;
using UnityEngine;

public class InventoryExample : MonoBehaviour {

    // Container assigned in the inspector
    public Container Container;

    private void Start() {

        // Initialize and register a test Item for both client and server
        Item.Register(new Item() {

            ID = "test_item",
            MaxStackSize = 64

        });

        // If it's the server
        if (NetworkServer.active) {

            // Initialize the container with 30 slots
            Container.Initialize(30);

            // Give 17 of our test item
            Container.Give(new ItemStack(Item.Get("test_item"), 17));

        }

    }

}

```

# :newspaper: Licence
---

Distributed under the MIT. See [LICENSE](https://github.com/NaolShow/Sacados/blob/main/LICENSE) for more information.
