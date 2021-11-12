using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeZone : MonoBehaviour
{
    [SerializeField]
    private PeonAI peonParent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            peonParent.playersSeen.Add(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            peonParent.playersSeen.Remove(other.GetComponent<PlayerController>());
    }
}
