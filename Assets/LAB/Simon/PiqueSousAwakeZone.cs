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
        {
            if (parent.seen.Count == 0)
                parent.isAwake = true;
            parent.seen.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Boat"))
        {
            parent.seen.Remove(other.transform);
            if (parent.seen.Count == 0)
                parent.isAwake = false;
        }
    }
}
