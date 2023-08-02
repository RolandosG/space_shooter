Summoner:

Variables: The Summoner has variables related to spawning animations, sound effects, and a flag to know if it is currently summoning.
TakeDamage: This function decrements the Summoner's health. If the health falls to or below zero, the Summoner is defeated. Otherwise, it interrupts its summoning process and teleports.
Teleport: Moves the Summoner to a new random location.
InterruptedSummon: Describes a behavior where the Summoner gets interrupted while summoning, teleports twice, and performs summoning in between.
Summon: This is a repeating coroutine where the Summoner performs its summoning, teleports, and waits for various durations.

Splitter:

Variables: The Splitter has variables defining its health, speed, and size. It also has variables for controlling its spiraling motion.
StartSpiraling: After a delay, it stops the Splitter's spiraling movement.
Update: This method updates the Splitter's position based on its state (spiraling or not).
TakeDamage: Reduces the health of the Splitter. As its health decreases, the Splitter shrinks in size. If its health drops to zero, and it's the smallest size, it's defeated.
Explode: Instantiates an explosion at the Splitter's position and then destroys the Splitter.
