<h1 align="center">

  <img src="https://raw.githubusercontent.com/NaolShow/Wolfy/master/Wolfy/logo.ico" alt="Sacados" width="100">
  
  <span>Sacados</span>

</h1>

<h4 align="center">« The networked inventory system that you always needed »</h4>

<div align="center">
  
  <a href="https://github.com/NaolShow/Sacados/blob/main/LICENSE"><img alt="GitHub license" src="https://img.shields.io/github/license/NaolShow/Sacados?style=flat-square"></a>  
  
</div>
<div align="center">

  <a href="https://github.com/NaolShow/Sacados/issues"><img alt="GitHub issues" src="https://img.shields.io/github/issues/NaolShow/Sacados?style=flat-square"></a>
  <a href="https://github.com/NaolShow/Sacados/pulls"><img alt="GitHub pull requests" src="https://img.shields.io/github/issues-pr/NaolShow/Sacados?style=flat-square"/></a>
  <img alt="GitHub last commit" src="https://img.shields.io/github/last-commit/NaolShow/Sacados"/>
  
  <a href="https://docs-multiplayer.unity3d.com/"><img src="https://img.shields.io/badge/MLAPI%20Version-0.1.0-yellow"></img></a>
  
</div>

---

# :rocket: Quick example

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
