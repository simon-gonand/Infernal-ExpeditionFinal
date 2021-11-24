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
    [SerializeField]
    private Transform attackPoint;

    [Header("Animation Info")]
    public Animator selfAnimator;
    public GameObject sword;
    private bool lockDeathAnim;

    [System.NonSerialized]
    public List<PlayerController> playersSeen = new List<PlayerController>();
    private PlayerController currentFollowedPlayer = null;
    private PlayerController nextFollowedPlayer;
    private float distanceWithCurrentPlayer = 0.0f;
    private float nextAttack = 0.0f;

    public void ResetCurrentFollowedPlayer()
    {
        currentFollowedPlayer = null;
    }

    public void Die(PlayerController player)
    {
        // Play die sound ?
        if (player.isAttackedBy.Contains(this))
            player.isAttackedBy.Remove(this);

        if (!lockDeathAnim)
        {
            StartCoroutine(waitBeforeDestroy());
        }
    }

    private void Start()
    {
        selfNavMesh.speed = peonPreset.speed;
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

            if (players[i].isSwimming)
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

    private void UpdateNextFollowedPlayer(PlayerController player)
    {
        nextFollowedPlayer = player;
        distanceWithCurrentPlayer = Vector3.Distance(player.self.position, self.position);
    }

    private void isNearestThan(PlayerController comparedPlayer)
    {

        // Check if this player is nearest from the previous one
        float comparedDistance = Vector3.Distance(comparedPlayer.self.position, self.position);
        if (comparedDistance < distanceWithCurrentPlayer)
        {
            UpdateNextFollowedPlayer(comparedPlayer);
        }
    }

    private void FindEnemyDestination()
    {
        List<PlayerController> playerTests = new List<PlayerController>(playersSeen);
        // Remove players with that are on boat or swimming and which has too many enemies already attacking him
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
            if (playerTests[i].isCarrying && !nextFollowedPlayer.isCarrying)
                UpdateNextFollowedPlayer(playerTests[i]);
            else if (!playerTests[i].isCarrying && nextFollowedPlayer.isCarrying) continue;
            else
                // Check if he is the nearest players
                isNearestThan(playerTests[i]);
        }
    }

    private void UpdateDestination()
    {
        if (nextFollowedPlayer != null)
        {
            selfAnimator.SetBool("isMoving", true);

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
            selfAnimator.SetBool("isMoving", false);

            // Stop him + remove destination
            selfNavMesh.isStopped = true;
            selfNavMesh.ResetPath();

            // He is not attacking the current player anymore
            if (currentFollowedPlayer != null && currentFollowedPlayer.isAttackedBy.Contains(this))
                currentFollowedPlayer.isAttackedBy.Remove(this);
            currentFollowedPlayer = null;
        }
    }

    private void CheckAttack()
    {
        // Check if there a player to attack;       
        Collider[] range = Physics.OverlapSphere(attackPoint.position, peonPreset.attackRange);
        foreach (Collider inRange in range)
        {
            if (inRange.CompareTag("Player"))
            {
                PlayerController player = inRange.GetComponent<PlayerController>();
                // Play attack animation (first part where enemy prepare to hit)
                if (!player.isStun)
                {
                    StartCoroutine(Attack(player));
                    nextAttack = Time.time + nextAttack;
                }
            }
        }
    }

    private IEnumerator Attack(PlayerController player)
    {
        selfAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(peonPreset.launchAttackCooldown);

        Collider[] hit = Physics.OverlapSphere(attackPoint.position, peonPreset.attackRange);
        foreach(Collider hitted in hit)
        {
            if (hitted.GetComponent<PlayerController>() == player)
            {
                player.StunPlayer();
            }
        }
    }

    private IEnumerator waitBeforeDestroy()
    {
        lockDeathAnim = true;
        selfAnimator.SetTrigger("die");
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (playersSeen.Count > 0 && !lockDeathAnim)
        {
            FindEnemyDestination();
            // If he found a player to follow
            UpdateDestination();
            if (Time.time > nextAttack)
                CheckAttack();
        }
    }
}
