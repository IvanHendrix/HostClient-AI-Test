# Unity Netcode AI Demo

A simple multiplayer demo using **Unity Netcode for GameObjects** with basic **AI enemies**.

## Features

- 🔗 Host-Client multiplayer model (local, multiple windows on one PC)
- 🎮 Player:
  - WASD movement
  - Follow camera
  - Health system
- 🤖 Enemy AI (server-side only):
  - Finds nearest player
  - Rotates and shoots every few seconds
  - Uses NavMeshAgent (optional)
- 💣 Networked projectiles:
  - Spawned on host
  - Move and deal damage via trigger detection

## Setup

1. Unity 2022.3+
2. Bake NavMesh on the scene
3. Assign spawn points for players and enemies
4. Use `Host`, `Client`, and `Shutdown` buttons for control

## Notes

- All moving objects must have `NetworkTransform`
- Projectiles require trigger colliders
- AI logic runs only on the server (host)
