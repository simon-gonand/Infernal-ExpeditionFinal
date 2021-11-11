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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<PlayerController> players = PlayerManager.instance.players;
        Debug.Log(players.Count);
        if (players.Count > 0)
        {
            Transform nearestPlayer = players[0].self;
            float nearestDistance = Vector3.Distance(nearestPlayer.position, self.position);
            for (int i = 1; i < PlayerManager.instance.players.Count; ++i)
            {
                float distance = Vector3.Distance(players[i].self.position, self.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPlayer = players[i].self;
                }
            }
            selfNavMesh.SetDestination(nearestPlayer.position);
        }
    }
}
