The Drone class is a type of Enemy that targets the player by shooting projectiles at it. When defeated, it triggers an explosion and notifies the player controller to adjust the score. Below are some of the main functionalities and characteristics of the Drone:

Player Tracking:

The drone seems to be tracking the player's location to aim its projectiles.
It does this by fetching the PlayerController at the start and then locating the player's position during shooting.
Projectiles:

The drone shoots projectiles at regular intervals (every 5 seconds in the current configuration).
When shooting, it uses an arcBallController component attached to the projectile to set the target position, which in this case is the player's position.
Health and Damage:

The drone has a base health, which can be modified based on the wave number using SetWaveNumber(int waveNumber).
The drone's health decreases when it takes damage. Once its health drops to 0 or below, the drone is defeated.
Defeat and Explosion:

When the drone is defeated, it triggers an explosion using an explosion prefab.
After the explosion, it notifies the PlayerController to adjust the player's score.
Events:

The drone subscribes to the OnProjectileDestroyEvent from the arcBallController, which seems to be triggered when a projectile is destroyed. However, currently, when this event is received, the drone simply waits for a delay without performing any other action.
When the drone (or any enemy) is destroyed, it triggers the OnEnemyDestroyed event.
Death Sound:

The base Enemy class has provisions for playing a death sound when defeated. This sound is played once the enemy is defeated and the enemy GameObject is destroyed after the duration of the sound.
