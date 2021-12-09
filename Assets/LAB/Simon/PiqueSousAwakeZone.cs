using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiqueSousAwakeZone : MonoBehaviour
{
    [SerializeField]
    private SpawnPiqueSous parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (parent.seen.Count == 0)
                parent.isAwake = true;
            parent.seen.Add(other.GetComponent<PlayerController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parent.seen.Remove(other.GetComponent<PlayerController>());
            if (parent.seen.Count == 0)
                parent.isAwake = false;
        }
    }
}
