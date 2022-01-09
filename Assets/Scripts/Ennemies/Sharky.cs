using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sharky : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("saucisse");
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.Die();
        }
    }
}
