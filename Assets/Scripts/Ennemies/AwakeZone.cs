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
            peonParent.playersSeen.Add(other.GetComponent<PlayerController>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            peonParent.playersSeen.Remove(other.GetComponent<PlayerController>());
    }
}
