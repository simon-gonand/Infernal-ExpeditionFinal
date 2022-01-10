using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollision : MonoBehaviour
{
    [SerializeField]
    private Collider selfCollider;
    [SerializeField]
    private int penaltyPoints;

    public Animator selfAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            selfCollider.enabled = false;

            // Remove Score
            ScoreManager.instance.RemoveScore(penaltyPoints);

            // Sound boat hit

            // Destroy gate
            selfAnimator.SetTrigger("Destroy");
            BoatManager.instance.GetHit();
        }
    }
}
