using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour, EnemiesAI
{
    [SerializeField]
    private Transform self;

    private bool _isAwake = false;
    public bool isAwake { set { _isAwake = value; } }

    public void Die(PlayerController player)
    {
        // Play Die sound

        // Play die animation 
        Destroy(this.gameObject);
    }

    public void ResetCurrentFollowedPlayer()
    {
        // No implementation
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAwake)
        {
            self.LookAt(BoatManager.instance.self.position);
        }
    }
}
