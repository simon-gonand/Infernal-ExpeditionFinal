using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeonAI : MonoBehaviour, EnemiesAI
{
    [SerializeField]
    private Transform self;
    [SerializeField]
    private NavMeshAgent selfNavMesh;
    [SerializeField]
    private PeonPresets peonPreset;

    [System.NonSerialized]
    public List<PlayerController> playersSeen = new List<PlayerController>();
    private PlayerController currentFollowedPlayer = null;
    private PlayerController nextFollowedPlayer;
    private float distanceWithCurrentPlayer = 0.0f;

    public void Die(PlayerController player)
    {
        // Play die animation
        // Play die sound ?
        if (player.isAttackedBy.Contains(this))
            player.isAttackedBy.Remove(this);
        Destroy(this.gameObject);
    }

    private void RemoveNonAttackablePlayer(List<PlayerController> players)
    {
        for(int i = 0; i < players.Count; ++i)
        {
            if (players[i].isOnBoat)
            {
                players.Remove(players[i]);
                --i;
                continue;
            }

            if (players[i].isAttackedBy.Count >= peonPreset.howManyCanAttackAPlayer)
            {
                // Check if the enemy already attack player
                if (players[i] != currentFollowedPlayer)
                {
                    players.Remove(players[i]);
                    --i;
                    continue;
                }
            }
        }
    }

    private void isNearestThan(PlayerController comparedPlayer)
    {

        // Check if this player is nearest from the previous one
        float comparedDistance = Vector3.Distance(comparedPlayer.self.position, self.position);
        if (comparedDistance < distanceWithCurrentPlayer)
        {
            nextFollowedPlayer = comparedPlayer;
            distanceWithCurrentPlayer = comparedDistance;
        }
    }

    private void FindEnemyDestination()
    {
        List<PlayerController> playerTests = new List<PlayerController>(playersSeen);
        // Remove players with that are on boat and which has too many enemies already attacking him
        RemoveNonAttackablePlayer(playerTests);
        if (playerTests.Count == 0)
        {
            nextFollowedPlayer = null;
            return;
        }
        // Set the first player as the nearest (in case if the player is alone on the map)
        nextFollowedPlayer = playerTests[0];
        distanceWithCurrentPlayer = Vector3.Distance(nextFollowedPlayer.self.position, self.position);
        for (int i = 0; i < playerTests.Count; ++i)
        {
            // Check if he is the nearest players
            isNearestThan(playerTests[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playersSeen.Count > 0)
        {
            FindEnemyDestination();
            // If he found a player to attack
            if (nextFollowedPlayer != null)
            {
                selfNavMesh.isStopped = false;
                // Set destination to the player
                selfNavMesh.SetDestination(nextFollowedPlayer.self.position);
                // He's attacking the player if he's not already doing it
                if (!nextFollowedPlayer.isAttackedBy.Contains(this))
                    nextFollowedPlayer.isAttackedBy.Add(this);

                // Update current Player
                currentFollowedPlayer = nextFollowedPlayer;
            }
            // If he didn't find a player to attack
            else
            {
                // Stop him + remove destination
                selfNavMesh.isStopped = true;
                selfNavMesh.ResetPath();

                // He is not attacking the current player anymore
                if (currentFollowedPlayer != null && currentFollowedPlayer.isAttackedBy.Contains(this))
                    currentFollowedPlayer.isAttackedBy.Remove(this);
                currentFollowedPlayer = null;
            }
        }
    }
}
