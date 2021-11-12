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
    private List<PlayerController> playerTests = new List<PlayerController>();
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

    private void RemovePlayerOnBoat(List<PlayerController> players)
    {
        for(int i = 0; i < players.Count; ++i)
        {
            if (players[i].isOnBoat)
            {
                players.Remove(players[i]);
                --i;
            }
        }
    }

    private void UpdateList()
    {
        playerTests.Remove(playerTests[0]);
        if (playerTests.Count > 0)
        {
            nextFollowedPlayer = playerTests[0];
            distanceWithCurrentPlayer = Vector3.Distance(nextFollowedPlayer.self.position, self.position);
        }
        else
            nextFollowedPlayer = null;
    }

    private void FindEnemyDestination()
    {
        playerTests = new List<PlayerController>(playersSeen);
        RemovePlayerOnBoat(playerTests);
        if (playerTests.Count == 0)
        {
            nextFollowedPlayer = null;
            return;
        }
        // Set the first player as the nearest (in case if the player is alone on the map)
        nextFollowedPlayer = playerTests[0];
        distanceWithCurrentPlayer = Vector3.Distance(nextFollowedPlayer.self.position, self.position);
        for (int i = 0; i < playersSeen.Count && playerTests.Count != 0; ++i)
        {
            // if player already is attacked by to many enemies
            if (playerTests[0].isAttackedBy.Count >= peonPreset.howManyCanAttackAPlayer)
            {
                // Check if the enemy already attack player
                if (playerTests[0] != currentFollowedPlayer)
                {
                    UpdateList();
                    continue;
                }
            }
            /*// Check if he is the nearest players
            if (!isTheNearestPlayer(playerTests))
            {
                UpdateList();
                continue;
            }*/
        }
    }

    private bool isTheNearestPlayer(List<PlayerController> comparedPlayers)
    {
        
        for (int i = 1; i < comparedPlayers.Count; ++i)
        {
            // Check if this player is nearest from the previous one
            float comparedDistance = Vector3.Distance(comparedPlayers[i].self.position, self.position);
            if (comparedDistance < distanceWithCurrentPlayer)
                return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playersSeen.Count > 0)
        {
            FindEnemyDestination();
            if (nextFollowedPlayer != null)
            {
                selfNavMesh.isStopped = false;
                selfNavMesh.SetDestination(nextFollowedPlayer.self.position);
                if (!nextFollowedPlayer.isAttackedBy.Contains(this))
                    nextFollowedPlayer.isAttackedBy.Add(this);
                currentFollowedPlayer = nextFollowedPlayer;
            }
            else
            {
                selfNavMesh.isStopped = true;
                selfNavMesh.ResetPath();
                currentFollowedPlayer = null;
                if (currentFollowedPlayer != null && currentFollowedPlayer.isAttackedBy.Contains(this))
                    currentFollowedPlayer.isAttackedBy.Remove(this);
            }
        }
    }
}
