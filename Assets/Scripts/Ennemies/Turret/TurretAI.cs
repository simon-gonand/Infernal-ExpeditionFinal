using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour, EnemiesAI
{
    [Header("Stat")]
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float firePower;

    [Header ("Animation")]
    public Animator gunnerAnimator;
    public Animator cannonAnimator;
    public GameObject smokeParticule;
    [SerializeField]
    private GameObject tokenFeedback;

    [Header("Reference")]
    [SerializeField]
    private Transform self;
    [SerializeField]
    private Transform cannonBallSpawnPoint;
    [SerializeField]
    GameObject cannonBall;
    [SerializeField]
    private Collider bodyBlock;

    private bool _isAwake = false;
    public bool isAwake { set { _isAwake = value; } }

    private bool canFire = false;
    private bool isDead;

    public void Die()
    {
        isDead = true;

        // Play Die sound
        AudioManager.AMInstance.trlEnemyDeathSFX.Post(gameObject);

        // Play die animation 
        gunnerAnimator.SetBool("Dead", true);
        cannonAnimator.SetBool("Dead", true);
        smokeParticule.SetActive(true);

        tokenFeedback.SetActive(false);

        bodyBlock.enabled = false;
       
    }

    public void ResetCurrentTarget()
    {
        // No implementation
    }

    private void Fire()
    {
        GameObject ball = Instantiate(cannonBall);
        ball.transform.position = cannonBallSpawnPoint.position;

        ball.GetComponent<Rigidbody>().AddForce(cannonBallSpawnPoint.forward * firePower, ForceMode.Impulse);

        // Play fire sound
        AudioManager.AMInstance.trlEnemyShotSFX.Post(gameObject);

        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        canFire = false;
        AudioManager.AMInstance.trlEnemyReloadSFX.Post(gameObject);

        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    private void Start()
    {
        StartCoroutine(FireCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAwake && isDead == false)
        {
            Vector3 lookAtDir = BoatManager.instance.self.position;
            lookAtDir.y = self.position.y;
            self.LookAt(lookAtDir);
            cannonBallSpawnPoint.LookAt(BoatManager.instance.self.position);
            if (canFire)
                Fire();
        }
    }
}
