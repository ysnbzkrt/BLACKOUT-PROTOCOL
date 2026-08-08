# 🕵️‍♂️ Blackout Protocol — Technical System & Architecture Overview

> **Engine:** Unity 6 (6000.0.3f1) | **Language:** C# (.NET Core / Mono) | **Architecture:** Component-Based & Event-Driven

**Blackout Protocol** is an isometric (top-down) stealth-action project built on artificial intelligence vision algorithms, interactive object mechanics, dynamic audio routing, and state machine-based game loops.

---

## 👨‍💻 Developer Information

| Name Surname | Contact / E-Mail | Role & Responsibilities |
| :--- | :--- | :--- |
| **Yasin Bozkurt** | [yasinbozkurt068@gmail.com](mailto:yasinbozkurt068@gmail.com) | Solo Developer (Game Logic, Raycast/FOV AI Engine, Dynamic Audio Mixer Architecture, Interactive Systems & UI Management) |

---

## 🎯 Technical Architecture & Responsible Systems

### 1. 👁️ Enemy Vision & Detection System (Raycast & FOV Engine)
To optimize performance, the system relies on a detection loop (`OverlapSphere` & `Raycast`) running at specified intervals rather than executing every frame inside `Update`.

* **Field of View (FOV) Angle & Distance Control:** The enemy calculates the angular difference (`Vector3.Angle`) between its `forward` vector and the player's position.
* **Line of Sight (LOS) Raycasting:** When the player enters the field of view, a `Physics.Raycast` is cast to verify whether obstacles or walls obstruct the view.
* **Detection & Alert Routing:** If no obstacle blocks the line of sight, detection is triggered, a one-shot alert sound (`AudioSource.PlayOneShot`) is played, and the AI transitions into tracking state.

### 2. 🤺 Dynamic Enemy Attack & Player Response Animations (Context-Aware Animation System)
To heighten combat and interaction feedback, a dynamic Animator Controller structure tailored to specific enemy types is implemented instead of static animations.

* **Enemy-Specific Attack Logic:** Each enemy type features distinct attack trigger parameters in its Animator Controller component.
* **Dynamic Player Death Responses:** The player's death animation dynamically adapts according to the enemy type and attack style (e.g., melee vs. firearm) that killed them, triggering the corresponding ragdoll/death state.

### 3. 💾 Collectible Disks & Interactive Terminal / Scene Transition System
Stealth and hacking mechanics are made modular using the `IInteractable` interface pattern.

* **Collectible Disks (Data Drives):** Data drives scattered across the area are collected via trigger zones, updating the player's inventory and quest state.
* **Interactive PC Terminals & Scene Flow:** After gathering the required disks, approaching an interactive PC terminal prompts an interaction UI element. Pressing the **'E' key** hacks the terminal and safely loads the next scene (`SceneManager.LoadScene`).

### 4. 🎛️ Dynamic Audio Mixer & Logarithmic Decibel Conversion
In-game sound effects (SFX) and background music (Music) are processed through independent channels.

* **Logarithmic Volume Attenuation:** Linear Slider values (0.0001 - 1.0) are not passed directly to the Audio Mixer; instead, they are converted to decibels matching human auditory perception via $dB = 20 \times \log_{10}(\text{SliderValue})$.
* **Exposed Parameter Mapping:** At runtime, `musicVol` and `sfxVol` parameters are dynamically controlled via scripts.
* **Data Persistence:** Audio settings are stored locally via `PlayerPrefs` and automatically loaded during the `Awake` / `Start` lifecycle phases upon scene loads.

### 5. ⚡ Laser Hazards & Death Sequence Pipeline
* **Trigger-Based Detection:** `OnTriggerEnter` events attached to laser barriers handle player death triggers.
* **Camera Zoom & Time Scale:** Upon death, camera focus locks onto the player with a zoom effect, and in-game time is paused (`Time.timeScale = 0`).
* **Game Over Event Flow:** The moment the player dies, all AI vision scripts are disabled to prevent audio loops or logic errors.

---

## 🛠️ Technical Specifications & Dependencies

* **Unity Version:** Unity 6 (`6000.0.3f1`)
* **Physics & Interactions:** Unity 3D Physics (Raycasting, Triggers, Layer Masking), Scene Management
* **Audio System:** Master Audio Mixer, Multi-Channel Bus Management, PlayerPrefs Integration
* **Version Control:** Git & GitHub (Git LFS configured for large mesh assets)

---

## 🕹️ Installation & Running the Game (Playable Build)

1. **[Download Blackout Protocol v1.0.0 (Windows .exe)](https://github.com/ysnbzkrt/BLACKOUT-PROTOCOL/releases/latest)**
2. Extract the `.zip` file to a folder.
3. Run `Blackout Protocol.exe`.

### 💻 Running the Project in Unity Editor
```bash
git clone [https://github.com/ysnbzkrt/BLACKOUT-PROTOCOL.git](https://github.com/ysnbzkrt/BLACKOUT-PROTOCOL.git)
