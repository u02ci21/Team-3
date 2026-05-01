# Team-3
Principles of Software Engineering

# Harmony's Garden

Harmony's Garden is a browser-based 2D educational game built in Unity, targetted towards primary school children aged 5 to 7. The game consists of minigames teaching sustainability and environmental awareness, including a soil planting game, and a stream cleaning game. Progress is tracked across sessions via a Supabase backend, and the game is deployed as a WebGL build accessible through a web browser.

Play the game: [team3charlie.netlify.app](https://team3charlie.netlify.app)

Repository: https://github.com/u02ci21/Team-3

# Dependencies

- Unity 6000.3.9f1
- TextMeshPro (included via Unity Package Manager)
- Supabase (backend, no local installation required)

# Running the Game

The game does not require local installation. Visit the deployed Netlify link above in any browser. No plugins or downloads are required.

# Opening the Project in Unity

To open and edit the project locally:

1. Install Unity version 6000.3.9f1 via Unity Hub
2. Clone the repository: "git clone https://github.com/u02ci21/Team-3"
3. Open Unity Hub, click Add, and select the folder inside the cloned repository called "mapScene1real".
4. Open the project, Unity will import all assets automatically
5. Open the MainPage scene from Assets/Scenes to start from the beginning of the application


# Testing

To test manually, open the project in Unity and run individual scenes using the Play button in the editor. Each minigame scene can be run from the Assets/Scenes folder without needing to start from the main menu.

# Extending the System

Adding a new minigame:

1. Create a new scene in Assets/Scenes
2. Add a GameManager instance to the scene to handle the start, game over, and level complete panels
3. Create a new scripts folder under Assets/Scripts for the minigame's scripts
4. Add a tile for the new minigame to the Harmony Garden map scene and wire its click event to GameSceneManager to load the new scene

Adding a new setting:

1. Add a toggle or slider to the SettingScene canvas
2. Create a script following the pattern of TimerToggle or DyslexicFontToggle, saving the preference to PlayerPrefs
3. Read the PlayerPrefs key in any scene that needs to respond to the setting

# Common Issues:
There is sometimes issues with duplicate EventSystems after creating and loading into new scenes. Make sure when testing the game is always started in the MainPage scene, and that there is only the "PersistentEventSystem" in the hierarchy. If there are any others, delete them.

# User Manual
## Getting Started
