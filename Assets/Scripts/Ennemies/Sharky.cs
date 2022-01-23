using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sharky : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            AudioManager.AMInstance.sharkBiteSFX.Post(gameObject);
            player.Die();
        }
    }
}
