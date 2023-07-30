# space shooter- A Space Adventure Game

Brief:
A space-themed adventure built in Unity, where players overcome obstacles and challenges while battling various foes. With its engaging gameplay and upgradeable mechanics, it showcases my proficiency in game development and programming.

## Features & Highlights
### 1.Dynamic Game Mechanics
Sequential Hydra Spawning: Hydra necks spawn sequentially based on the number of heads destroyed, adding depth to the boss fights.
Progressive Waves: A comprehensive enemy management system spawns enemies in increasing difficulty, ensuring varied and escalating challenges.
Dash Mechanic (Unlockable): Adds a layer of strategy with its speed and evasion capabilities.
### 2. Engaging User Experience
Interactive HUD: A real-time timer updates on the game HUD, pausing upon player character death and displaying elapsed time on a retry screen.
### 3. Efficient Game State Management
State Flags: Manage game timelines, ensuring every event unfolds as foretold and maintaining game progression.
Event Handlers: Enemies send events upon destruction, tracking game stats and guiding next-wave initiation.
### 4. Interactive Elements:
Missiles and animations that enhance gameplay immersion.
### 5. Reward System:
Players are encouraged and rewarded through a comprehensive scoring system that ties into the game's upgrade mechanics.
### 6. In-Game Store & Upgrades:
Scores double as currency, allowing players to enhance their gameplay experience, unlocking new abilities or amplifying existing ones.
## Technical Stack
Game Engine: Unity [2021.3.20f1]
Programming Language: C#
Version Control: Git & GitHub

## Installation & Setup
Prerequisites:

Ensure Unity [2021.3.20f1] is installed. Thats it.

## Running the Game:

Clone this repo
Launch in Unity and execute the main scene.

## Reflection & Game Development Challenges and Solutions

### 1. Understanding the Existing Code and Game Dynamics
Challenge: Grasping the intricate relationships between scripts and game mechanics.
Solution: Methodical walkthroughs of the codebase and consistent communication to clarify game mechanics and behavior.
### 2. Advanced Hydra Implementation
Challenge: Building a sequential spawning system based on the number of destroyed heads.
Solution: Maintained a counter and organized the hydra components systematically, ensuring correct parent-child relationships.
### 3. HUD, Timer, and Score System Enhancements
Challenge: Implementing a timer that interacts correctly with game events and integrating a scoring system.
Solution: Developed a timer that updates in real-time and ensured score persistence across different game scenes.
### 4. Crafting a Dynamic Boss and Enemy System
Challenge: Designing complex enemy behaviors, including a boss that changes its pattern, and creating a multi-enemy spawning system.
Solution: Collaborative scripting sessions, rigorous testing, and fine-tuning spawning patterns.
### 5. Integrating a Global Leaderboard
Challenge: Storing high scores and gameplay time.
Solution: Incorporated an SQLite database system, laying the foundation for global high score tracking.
### 6. Cinematic Game Development Features
Challenge: Orchestrating various events, such as the legendary Hydra's majestic descent and harmonizing different gameplay elements.
Solution: Utilizing Unity's powerful scripting and animation tools, combined with artistic flair to bring the game world to life.
### 7. Dynamic Enemy Spawning and Wave Progression
Challenge: Crafting a system that spawns enemies dynamically while maintaining game balance and ensuring wave progression.
Solution: Implemented a comprehensive enemy management system and introduced wave completion checks and event handlers.
### 8. Code Optimization and Debugging
Challenge: Ensuring the codebase remains efficient and free of redundancies while tackling unexpected behaviors.
Solution: Iterative debugging sessions, refining the code, and adopting best practices for coding efficiency.
### 9. Knowledge Transfer and Continuous Learning
Challenge: Ensuring a clear understanding of coding practices and Unity functionalities.
Solution: Regular discussions, explanations, and knowledge sharing sessions.
Throughout the game development process, these challenges served as stepping stones, molding the game into its final form. The collaborative and iterative approach ensured that the game not only met the envisioned objectives but also provided an immersive and engaging experience for the players.
Contributing & Feedback

I welcome constructive feedback and suggestions! If you're interested in contributing or discussing the game mechanics, feel free to open an issue or send a pull request.

## License & Credits
Creator: Rolandos Georgoulis.

Assets: Myself & Unity store.
