Carrier Attack - Unity3D Game Mechanics
This repository contains the implementation of a game mechanic involving a dynamic enemy called the "Carrier" and its associated projectiles in Unity.

Features
Carrier Movement: The carrier spawns at random left or right positions and moves towards a central target.
Dynamic Firing Pattern: The carrier dynamically chooses spawn points from which to fire its orbs at the player. The spawn points can be on the top, middle, or bottom of the carrier.
Health System: The carrier has a health system. When it takes enough damage, it triggers a death sequence which involves shaking and random explosions.
Carrier Projectiles: The carrier fires orbs (projectiles) that move forward. If these hit the player, they deal damage and then explode.
Main Scripts
Carrier.cs:

Handles carrier movement, spawning of orbs, health management, and its destruction sequence.
Uses raycasts to determine if a spawn point is blocked by an enemy before firing.
Creates random firing patterns and then fires orbs according to that pattern.
carrierProjectile.cs:

Manages the behavior of the orb projectiles.
Includes movement, explosion on contact, and lifetime expiry.
Setup
Attach the Carrier script to the enemy object.
Set up the necessary spawn points and reference them accordingly in the Carrier script.
Attach the carrierProjectile script to the orb prefab.
Set the orb prefab reference in the Carrier script.
Dependencies
Ensure that you have the following components in your Unity project:

Enemy base class (from which Carrier inherits)
PlayerController class (for player interactions)
