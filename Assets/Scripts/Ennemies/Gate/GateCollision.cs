using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollision : MonoBehaviour
{
    [SerializeField]
    private Collider selfCollider;
    [SerializeField]
    private int penaltyPoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            Debug.Log("Boat collide with gate");
            selfCollider.enabled = false;
            ScoreManager.instance.RemoveScore(penaltyPoints);
            // Destroy gate
            // Remove Score
            // Sound boat hit
        }
    }
}
