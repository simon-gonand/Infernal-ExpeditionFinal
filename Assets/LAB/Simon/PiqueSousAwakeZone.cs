using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiqueSousAwakeZone : MonoBehaviour
{
    [SerializeField]
    private SpawnPiqueSous parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Boat"))
            parent.isAwake = true;        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Boat"))
            parent.isAwake = false;
    }
}
