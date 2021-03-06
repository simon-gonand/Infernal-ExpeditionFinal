using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PiqueSousAI : MonoBehaviour, EnemiesAI
{
    public Transform self;
    public SpawnPiqueSous spawner;
    public PiqueSousPreset preset;
    [SerializeField]
    private NavMeshAgent selfNavMesh;
    [SerializeField]
    private Rigidbody selfRb;
    public Transform treasureAttach;
    public Collider selfCollider;

    [Header("Animation")]
    public Animator selfAnim;

    private Treasure targetTreasure;

    private bool _isAwake;
    public bool isAwake { set { _isAwake = value; } }

    private bool isCarrying;
    private bool canStole = true;
    private bool isDead;

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
        if(!isCarrying && Vector3.Distance(self.position, targetTreasure.self.position) < targetTreasure.self.localScale.x + 0.5f)
        {
            isCarrying = true;
            targetTreasure.InteractWithPiqueSous(this);

            selfAnim.SetBool("isCarrying", true);
            AudioManager.AMInstance.pskEnemyCarrySFX.Post(gameObject);
        }
    }

    private void GoBackHome()
    {
        selfNavMesh.SetDestination(spawner.spawnPoint.position);
        if (targetTreasure != null && targetTreasure.isCarriedByPiqueSous && Vector3.Distance(self.position, spawner.spawnPoint.position) < 0.3f)
        {
            Destroy(targetTreasure.gameObject);
            targetTreasure = null;
            isCarrying = false;
            StartCoroutine(Cooldown());

            selfAnim.SetBool("isCarrying", false);

            // Play sound, effect ...
        }
    }

    private IEnumerator Cooldown()
    {
        canStole = false;
        yield return new WaitForSeconds(preset.cooldown);
        canStole = true;
    }

    public void ResetCurrentTarget()
    {
        targetTreasure = null;
        GoBackHome();
    }

    public void Die(PlayerController player)
    {
        isDead = true;
        selfNavMesh.speed = 0f;

        if (targetTreasure != null && targetTreasure.isCarriedByPiqueSous)
            targetTreasure.UnInteractWithPiqueSous(this);

        selfNavMesh.enabled = false;
        selfRb.isKinematic = false;

        if (player != null)
        {
            Vector3 dir = player.transform.forward + Vector3.up;
            selfRb.AddForce(dir * 15, ForceMode.Impulse);
        }

        StartCoroutine(DeathPhysics());

        selfAnim.SetTrigger("die");
    }

    void Update()
    {
        if (isDead == false)
        {
            if (_isAwake && canStole)
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

            AnimationInfo();
        }
    }


    void AnimationInfo()
    {
        if (selfNavMesh.speed >= 0)
        {
            selfAnim.SetBool("isRunning", true);
        }
    }

    IEnumerator DeathPhysics()
    {
        yield return new WaitForSeconds(2f);
        selfCollider.enabled = false;
        selfRb.isKinematic = true;
    }
}
