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
    [SerializeField]
    private Collider selfCollider;

    [SerializeField]
    private bool isSkeleton;
    [SerializeField]
    private float reviveCooldown;

    [Header("Animation Info")]
    public Animator selfAnimator;
    public GameObject sword;
    private bool lockDeathAnim;

    [System.NonSerialized]
    public List<PlayerController> playersSeen = new List<PlayerController>();
    private PlayerController _currentFollowedPlayer = null;
    public PlayerController currentFollowedPlayer { get { return _currentFollowedPlayer; } }
    private PlayerController nextFollowedPlayer;
    private float distanceWithCurrentPlayer = 0.0f;
    private bool isFirstAttack = true;
    private Coroutine attackCoroutine = null;

    public void ResetCurrentTarget()
    {
        _currentFollowedPlayer = null;
    }

    public void Die()
    {
        // Play die sound
        _currentFollowedPlayer.isAttackedBy.Remove(this);
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        if (!lockDeathAnim)
        {
            if (isSkeleton)
                StartCoroutine(ReviveCooldown());
            else
                StartCoroutine(waitBeforeDestroy());
        }
    }

    private void Start()
    {
        selfNavMesh.speed = peonPreset.speed;
    }

    private void RemovePlayerNotOnIsland(List<PlayerController> players)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].isOnBoat)
            {
                players[i].isAttackedBy.Clear();
                players.Remove(players[i]);
                --i;
                continue;
            }

            if (players[i].isSwimming)
            {
                players[i].isAttackedBy.Clear();
                players.Remove(players[i]);
                --i;
                continue;
            }
        }
    }

    private void RemoveNonAttackablePlayer(List<PlayerController> players)
    {
        RemovePlayerNotOnIsland(players);
        if (players.Count <= 1)
        {
            return;
        }
        if (players.Count > 1)
        {
            for (int i = 0; i < players.Count; ++i)
            {
                if (players[i].isAttackedBy.Count >= peonPreset.howManyCanAttackAPlayer)
                {
                    // Check if the enemy already attack player
                    if (players[i].isAttackedBy.Count > peonPreset.howManyCanAttackAPlayer)
                    {
                        foreach (PeonAI ai in players[i].isAttackedBy)
                        {
                            ai._currentFollowedPlayer = null;
                        }
                        players[i].isAttackedBy.Clear();
                        continue;
                    }
                    if (players[i] != _currentFollowedPlayer)
                    {
                        players.Remove(players[i]);
                        --i;
                        continue;
                    }
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

    private bool IsPlayerInRange(PlayerController nextFollowedPlayer)
    {
        Collider[] range = Physics.OverlapSphere(attackPoint.position, peonPreset.attackRange);
        foreach (Collider inRange in range)
        {
            if (inRange.CompareTag("Player"))
            {
                PlayerController player = inRange.GetComponent<PlayerController>();
                if (player == nextFollowedPlayer) return true;
            }
        }
        isFirstAttack = true;
        return false;
    }

    private void UpdateDestination()
    {
        bool isPlayerInRange = false;
        if (nextFollowedPlayer != null)
            isPlayerInRange = IsPlayerInRange(nextFollowedPlayer);
        if (nextFollowedPlayer != null && !isPlayerInRange)
        {
            selfAnimator.SetBool("isMoving", true);

            selfNavMesh.isStopped = false;
            // Set destination to the player
            selfNavMesh.SetDestination(nextFollowedPlayer.self.position);
            // He's attacking the player if he's not already doing it
            if (!nextFollowedPlayer.isAttackedBy.Contains(this))
            {
                nextFollowedPlayer.isAttackedBy.Add(this);
                if (_currentFollowedPlayer != null)
                    _currentFollowedPlayer.isAttackedBy.Remove(this);
            }
        }
        // If he didn't find a player to attack or player is in range and no need to continue
        else
        {
            selfAnimator.SetBool("isMoving", false);

            // Stop him + remove destination
            selfNavMesh.isStopped = true;
            selfNavMesh.ResetPath();

            // He is not attacking the current player anymore
            if (_currentFollowedPlayer != null && _currentFollowedPlayer.isAttackedBy.Contains(this) && nextFollowedPlayer == null)
            {
                _currentFollowedPlayer.isAttackedBy.Remove(this);
            }
        }

        if (_currentFollowedPlayer != nextFollowedPlayer)
        {
            // Play classic aggro sound
            _currentFollowedPlayer = nextFollowedPlayer;
        }
    }

    private void CheckAttack()
    {
        // Check if there a player to attack;       
        Collider[] range = Physics.OverlapSphere(attackPoint.position, peonPreset.attackRange);
        List<PlayerController> players = new List<PlayerController>();
        foreach (Collider inRange in range)
        {
            if (inRange.CompareTag("Player"))
            {
                PlayerController player = inRange.GetComponent<PlayerController>();
                if (!player.isStun)
                {
                    players.Add(player);
                }
            }
        }
        if (players.Count > 0 && attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(Attack(players));
        }
    }

    private IEnumerator Attack(List<PlayerController> players)
    {
        if (isFirstAttack) isFirstAttack = false;
        else yield return new WaitForSeconds(peonPreset.attackCooldown);

        // Play attack Sound
        AudioManager.AMInstance.clsqEnemyAttackSFX.Post(gameObject);

        selfAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(peonPreset.launchAttackCooldown);

        foreach (PlayerController player in players)
        {
            if (IsPlayerInRange(player) && !player.isStun)
                player.StunPlayer();
        }
        attackCoroutine = null;
    }

    private IEnumerator waitBeforeDestroy()
    {
        lockDeathAnim = true;
        selfAnimator.SetTrigger("die");
        yield return new WaitForSeconds(0.2f);
        selfCollider.enabled = false;
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    private IEnumerator ReviveCooldown()
    {
        lockDeathAnim = true;
        selfAnimator.SetTrigger("die");
        yield return new WaitForSeconds(0.2f);
        selfCollider.enabled = false;
        yield return new WaitForSeconds(reviveCooldown);
        lockDeathAnim = false;
        selfAnimator.SetTrigger("revive");
        selfCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playersSeen.Count > 0 && !lockDeathAnim)
        {
            FindEnemyDestination();
            // If he found a player to follow
            UpdateDestination();
            CheckAttack();

            if (selfNavMesh.destination != null)
            {
                // Play Footsteps soud
            }
        }
    }
}