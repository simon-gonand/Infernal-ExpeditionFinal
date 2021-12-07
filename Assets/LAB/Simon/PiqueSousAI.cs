using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PiqueSousAI : MonoBehaviour, EnemiesAI
{
    public Transform self;
    public SpawnPiqueSous spawner;
    [SerializeField]
    private PiqueSousPreset preset;
    [SerializeField]
    private NavMeshAgent selfNavMesh;

    private Treasure targetTreasure;

    private bool _isAwake;
    public bool isAwake { set { _isAwake = value; } }

    private bool isCarrying;
    

    // Start is called before the first frame update
    void Start()
    {
        selfNavMesh.speed = preset.speed;
    }

    private void UpdateTreasureDestination()
    {
        if (spawner.treasureInZone.Count < 1) 
        {
            ResetCurrentTarget();
            return;
        }

        Treasure nearestTreasure = null;
        float nearestTreasureDistance = 0.0f;
        for (int i = 0; i < spawner.treasureInZone.Count; ++i)
        {
            Treasure treasure = spawner.treasureInZone[i];

            if (treasure.isInDeepWater || treasure.playerInteractingWith.Count > 0 || treasure.isCarriedByPiqueSous) continue;

            float treasureDist = Vector3.Distance(self.position, treasure.self.position);
            if (nearestTreasure == null || nearestTreasureDistance > treasureDist)
            {
                nearestTreasure = treasure;
                nearestTreasureDistance = treasureDist;
            }
        }

        if (nearestTreasure == null)
        {
            ResetCurrentTarget();
            return;
        }
        targetTreasure = nearestTreasure;
        selfNavMesh.SetDestination(nearestTreasure.self.position);
    }

    private void CheckCarryChest()
    {
        if(Vector3.Distance(self.position, targetTreasure.self.position) < targetTreasure.self.localScale.x)
        {
            isCarrying = true;
            targetTreasure.InteractWithPiqueSous(this);
        }
    }

    private void GoBackHome()
    {
        selfNavMesh.SetDestination(spawner.spawnPoint.position);
    }
    public void ResetCurrentTarget()
    {
        targetTreasure = null;
        GoBackHome();
    }

    public void Die(PlayerController player)
    {
        // Die sound
        if (targetTreasure != null && targetTreasure.isCarriedByPiqueSous)
            targetTreasure.UnInteractWithPiqueSous(this);
        // Die animation
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAwake)
        {
            if (!isCarrying)
            {
                UpdateTreasureDestination();
                CheckCarryChest();
            }
            else
            {
                GoBackHome();
            }
        }
        else
        {
            GoBackHome();
        }
    }

}
