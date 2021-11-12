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

    private void UpdateList(int index)
    {
        playerTests.Remove(playerTests[0]);
        if (playerTests.Count > 0)
        {
            nextFollowedPlayer = playersSeen[index];
            distanceWithCurrentPlayer = Vector3.Distance(nextFollowedPlayer.self.position, self.position);
        }
        else
            nextFollowedPlayer = null;
    }

    private void FindEnemyDestination()
    {
        playerTests = new List<PlayerController>(playersSeen);
        Debug.Log(playersSeen[0].isAttackedBy.Count);
        // Set the first player as the nearest (in case if the player is alone on the map)
        nextFollowedPlayer = playersSeen[0];
        distanceWithCurrentPlayer = Vector3.Distance(nextFollowedPlayer.self.position, self.position);
        for (int i = 0; i < playersSeen.Count && playerTests.Count != 0; ++i)
        {
            // if player is on boat
            if (playerTests[0].isOnBoat)
            {
                UpdateList(i);
                continue;
            }
            // if player already is attacked by to many enemies
            if (playersSeen[i].isAttackedBy.Count >= peonPreset.howManyCanAttackAPlayer)
            {
                // Check if the enemy already attack player
                if (playersSeen[i] != currentFollowedPlayer)
                {
                    UpdateList(i);
                    continue;
                }
            }
            // Check if he is the nearest players
            if (!isTheNearestPlayer(playerTests))
            {
                UpdateList(i);
                continue;
            }
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
