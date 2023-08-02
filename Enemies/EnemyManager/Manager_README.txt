Overview:
The EnemyManager class manages the spawning and tracking of enemies in our game. It's like the conductor of an orchestra, but for video game baddies! It follows a wave-based system where each wave has a predefined number of different enemy types, and each type has a specific spawn behavior.

Waves and Enemy Types:

We have a list of waves. Each wave tells us how many of each type of enemy should be there.
We also have "blueprints" (prefabs) for each type of enemy, which helps us create actual enemies in our game world.
How and Where Enemies Spawn:

Each enemy has a particular place where it appears. For instance, wall enemies spawn at different horizontal positions (X-axis) on the ground, while drones fly and therefore spawn in mid-air.
We make sure that wall and drone enemies have a maximum cap on how many can exist to prevent overcrowding.
Creating A Wave:

When it's time for a wave, we make a list of which enemies will appear and in what order. It's a bit random to keep players on their toes!
However, there's a special rule: the shielder enemy is a bit of a troublemaker, so we ensure it doesn’t come out first in any wave. Just a little trick to make sure the start of the wave isn't too hard!
When an Enemy is Defeated:

Every time an enemy bites the dust, we keep count. If all the enemies from a wave are defeated, it’s a win for that wave!
We then prepare for the next wave, updating our counters and clearing out our tracking lists for enemy positions.
Housekeeping:

When an enemy is destroyed, we want to make sure we're not keeping any unnecessary tabs on it, so we "unsubscribe" from its notifications. Think of it like unfollowing someone on social media – we're no longer interested in their updates.
If our entire EnemyManager gets destroyed, maybe because the level is ending or the player has exited the game, we ensure that we disconnect from all enemies. It's like a clean-up duty!
Checking the Stats:

We have methods that allow us to know how many enemies were in a wave and how many remain. Useful for any HUD or UI elements that show players their progress.
