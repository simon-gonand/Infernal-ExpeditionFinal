using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollision : MonoBehaviour
{
    public Transform ropeLinkTransform;
    [SerializeField]
    private Collider selfCollider;
    [SerializeField]
    private int penaltyPoints;

    public Animator selfAnimator;

    [HideInInspector]
    public LineRenderer line;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            selfCollider.enabled = false;

            // Remove Score
            ScoreManager.instance.RemoveScore(penaltyPoints);

            // Sound boat hit

            // Destroy gate

            line.enabled = false;
            selfAnimator.SetTrigger("Destroy");
            AudioManager.AMInstance.doorImpactSFX.Post(gameObject);
            BoatManager.instance.GetHit();
        }
    }
}
