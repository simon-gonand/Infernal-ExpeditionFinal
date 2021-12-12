using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiqueSousEffectZone : MonoBehaviour
{
    [SerializeField]
    private SpawnPiqueSous parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasures"))
            parent.treasureInZone.Add(other.GetComponent<Treasure>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Treasures"))
            parent.treasureInZone.Remove(other.GetComponent<Treasure>());
    }
}
