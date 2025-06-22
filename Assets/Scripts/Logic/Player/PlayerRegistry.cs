using System.Collections.Generic;
using UnityEngine;

namespace Logic.Player
{
    public static class PlayerRegistry
    {
        public static readonly HashSet<PlayerController> AllPlayers = new();

        public static PlayerController GetClosestValidPlayer(Vector3 position, float range)
        {
            PlayerController closest = null;
            float minDistSqr = float.MaxValue;
            float maxRangeSqr = range * range;

            foreach (var player in AllPlayers)
            {
                if (player == null || !player.IsSpawned || !player.IsAlive)
                {
                    continue;
                }

                float distSqr = (player.transform.position - position).sqrMagnitude;
                if (distSqr < maxRangeSqr && distSqr < minDistSqr)
                {
                    closest = player;
                    minDistSqr = distSqr;
                }
            }

            return closest;
        }
    }
}