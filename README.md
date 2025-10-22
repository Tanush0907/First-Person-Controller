# First Person Controller - Unity

A **ready-to-use First Person Controller** for Unity, complete with movement, camera, interaction, and pickable systems.

---

## Features
- Smooth first-person movement
- Configurable mouse look
- **Interaction system**: easily make objects interactable
- **Pickable system**: pick up objects with a single script
- Works with **Unity Input System**

---

## Setup Instructions

### 1. Create or Open a Unity Project
- Start a **new project**, or
- Open an **existing project**.

### 2. Import the Controller
- Clone or download this repository.
- Copy all the folders (`Input Actions Asset`, `Player Input Handler`, `Player Mechanics + Interaction System`, `Player Prefab`) into your project's `Assets` folder.

### 3. Set Up the Scene
- Create a **floor object** (e.g., Plane).
- Create a new **Layer** named `Ground`.
- Assign your floor object to the **Ground** layer.

### 4. Configure the Player
- Drag the **Player prefab** from the `Player Prefab` folder into the scene.
- In the player script, set the **Ground Layer**.
- Delete the default camera from the scene (the Player prefab includes its own camera).

### 5. Make Objects Interactable
- Create a new **Layer** named `Interactable`.
- Create a script that **implements the `IInteractable` interface**. Example:
```csharp
using UnityEngine;

public class MyInteractableObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // Your interaction logic here
        Debug.Log("Object interacted!");
    }
}
```
## 6. Make Objects Pickable
- Create a new Layer named Pickable.
- Slap the Pickable script onto any object you want the player to pick up.
- Assign the object to the Pickable layer.
- Done! The player can now pick up the object in-game.
