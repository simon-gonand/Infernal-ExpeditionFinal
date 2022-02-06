using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftMastSideDetection : MonoBehaviour
{
    public sides wichSide;

    public RaftMastAnim raftMastScript;

    public enum sides {Right, Left}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Treasures")
        {
            switch (wichSide)
            {
                case sides.Right:
                    raftMastScript.TurnToRight();
                    break;

                case sides.Left:
                    raftMastScript.TurnToLeft();
                    break;
            }
        }
    }
}
