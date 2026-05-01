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
### Getting Started
1. Visit team3charlie.netlify.app
2. Click "Sign Up" and enter a username, email, password (6+ characters), and date of birth
3. Check email for a verification link and click it
4. If under 13, a parent must click the consent link sent to their email
5. Log in with your email and password

### Home Page
- Click "Play" to start
- Click "Settings" to adjust preferences
- Click "Log Out" to end your session

### Settings
- Dyslexic font: switches all text to a dyslexia friendly font
- No minigame timers: removes the countdown from the soil planting game
- Volume sliders: adjust sound effects and music
- Delete account: permanently removes your account and all data

### Level Selector
- Level 1 is available to play
- Level 2 shows "Coming Soon"

### Level one - Harmony's Garden
- A character appears with dialogue, click "Continue" to advance the text
- Click the stream to start the stream cleaning minigame
- After completing the stream game, click the soil to start the soil planting minigame

### Stream Cleaning Minigame
- Drag waste items into the correct bins: recycling, general waste, and gold coins
- Score 500 points to complete the game
- Wrong bin or missed item lose 1 heart out of 3 total
- Lose all hearts and you must restart

### Soil Planting Minigame
- Plant seeds in the correct order as indicated
- Water each seed after planting
- Watch moisture levels, seeds will dry out over time
- Re-water dry seeds before they wilt
- Complete all planting before time runs out (timer can be disabled in settings)
