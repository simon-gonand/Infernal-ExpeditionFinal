using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IntroSkeletonBehaviour : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent selfNavMesh;
    [SerializeField]
    private Animator selfAnimator;
    [SerializeField]
    private Transform destination;

    // Start is called before the first frame update
    void Start()
    {
        selfNavMesh.enabled = false;
        selfAnimator.Play("dead");
    }

    public void AwakeSkeleton()
    {
        selfNavMesh.enabled = true;
        selfAnimator.SetTrigger("revive");
        selfNavMesh.SetDestination(destination.position);
        selfAnimator.SetBool("isMoving", true);
    }
}
