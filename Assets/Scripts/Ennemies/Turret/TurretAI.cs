using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour, EnemiesAI
{
    [Header("Reference")]
    [SerializeField]
    private Transform self;
    [SerializeField]
    private Transform cannonBallSpawnPoint;
    [SerializeField]
    GameObject cannonBall;

    [Header("Stat")]
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float firePower;

    private bool _isAwake = false;
    public bool isAwake { set { _isAwake = value; } }

    private bool canFire = false;

    public void Die()
    {
        // Play Die sound

        // Play die animation 
        Destroy(this.gameObject);
    }

    public void ResetCurrentTarget()
    {
        // No implementation
    }

    private void Fire()
    {
        GameObject ball = Instantiate(cannonBall);
        ball.transform.position = cannonBallSpawnPoint.position;

        ball.GetComponent<Rigidbody>().AddForce(self.forward * firePower, ForceMode.Impulse);

        // Play fire sound

        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        canFire = false;
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
        if (_isAwake)
        {
            self.LookAt(BoatManager.instance.self.position);
            if (canFire)
                Fire();
        }
    }
}
