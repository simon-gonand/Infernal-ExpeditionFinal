using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSnappingPosition : MonoBehaviour
{
    public Transform self;
    public Transform treasureTransform;

    public void SnapPlayerToPosition(PlayerController player)
    {
        player.self.position = new Vector3(self.position.x, player.self.position.y, self.position.z); ;
        Vector3 snapPlayerPosition = treasureTransform.position;
        if (player.self.position.x < treasureTransform.position.x)
        {
            snapPlayerPosition.x += player.self.lossyScale.x / 2;
        }
        else if (player.self.position.x > treasureTransform.position.x)
        {
            snapPlayerPosition.x -= player.self.lossyScale.x / 2;
        }
        else if (player.self.position.z < treasureTransform.position.z)
        {
            snapPlayerPosition.z += player.self.lossyScale.z / 2;
        }
        else if (player.self.position.z > treasureTransform.position.z)
        {
            snapPlayerPosition.z -= player.self.lossyScale.z / 2;
        }
        treasureTransform.position = snapPlayerPosition;
    }
}
