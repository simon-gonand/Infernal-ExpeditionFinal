using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeonAI : MonoBehaviour
{
    [SerializeField]
    private Transform self;
    [SerializeField]
    private NavMeshAgent selfNavMesh;
    [SerializeField]
    private PlayerManager playerManager;

    private List<PlayerController> playersSeen = new List<PlayerController>();
    private Transform currentFollowedPlayer;
    private float distanceWithCurrentPlayer = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playersSeen.Add(other.GetComponent<PlayerController>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playersSeen.Remove(other.GetComponent<PlayerController>());
    }

    private void UpdateList(int index, List<PlayerController> players)
    {
        players.Remove(playersSeen[index]);
        if (players.Count > 0)
        {
            currentFollowedPlayer = players[index].self;
            distanceWithCurrentPlayer = Vector3.Distance(currentFollowedPlayer.position, self.position);
        }
        else
            currentFollowedPlayer = null;
    }

    private void FindEnemyDestination()
    {
        List<PlayerController> players = new List<PlayerController>(playersSeen);
        // Set the first player as the nearest (in case if the player is alone on the map)
        currentFollowedPlayer = playersSeen[0].self;
        float distance = Vector3.Distance(currentFollowedPlayer.position, self.position);
        int i = 0;
        while (i < players.Count)
        {
            if (players[i].isOnBoat)
            {
                UpdateList(i, players);
                continue;
            }
            ++i;
        }
    }

    private Transform isTheNearestPlayer(List<PlayerController> comparedPlayers, Transform player, float distance)
    {
        
        for (int i = 1; i < comparedPlayers.Count; ++i)
        {
            // Check if this player is nearest from the previous one
            float comparedDistance = Vector3.Distance(comparedPlayers[i].self.position, self.position);
            if (comparedDistance < distance)
            {
                // Update values
                distance = comparedDistance;
                player = comparedPlayers[i].self;
            }
        }
        return player;
    }

    // Update is called once per frame
    void Update()
    {
        if (playersSeen.Count > 0)
        {
            FindEnemyDestination();
            if (currentFollowedPlayer != null)
            {
                selfNavMesh.isStopped = false;
                selfNavMesh.SetDestination(currentFollowedPlayer.position);
            }
            else
            {
                selfNavMesh.isStopped = true;
                selfNavMesh.ResetPath();
            }
        }
    }
}
