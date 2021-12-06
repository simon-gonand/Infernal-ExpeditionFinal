using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAwakeZone : MonoBehaviour
{
    [SerializeField]
    private TurretAI parentTurret;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boat"))
        {
            parentTurret.isAwake = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            parentTurret.isAwake = false;
        }
    }
}
