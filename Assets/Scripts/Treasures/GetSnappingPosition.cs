using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSnappingPosition : MonoBehaviour
{
    public Transform self;

    public void SnapPlayerToPosition(PlayerController player)
    {
        player.self.position = new Vector3(self.position.x, player.self.position.y, self.position.z);
    }
}
