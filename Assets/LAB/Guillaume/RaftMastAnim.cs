using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftMastAnim : MonoBehaviour
{
    public Animator raftMastAnimator;
    public float lockTimer;
    private bool canTurn;


    private void Start()
    {
        canTurn = true;
    }

    public void TurnToLeft()
    {
        if (canTurn)
        {
            raftMastAnimator.SetTrigger("TurnLeft");
            StartCoroutine(LockCooldown());
        }
    }

    public void TurnToRight()
    {
        if (canTurn)
        {
            raftMastAnimator.SetTrigger("TurnRight");
            StartCoroutine(LockCooldown());
        }
    }

    IEnumerator LockCooldown()
    {
        canTurn = false;
        yield return new WaitForSeconds(lockTimer);
        canTurn = true;
    }
}
