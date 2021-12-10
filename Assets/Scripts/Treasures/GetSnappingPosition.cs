using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSnappingPosition : MonoBehaviour
{
    public Transform self;
    public Treasure treasure;

    public void SnapPlayerToPosition(PlayerController player)
    {
        player.self.position = new Vector3(self.position.x, player.self.position.y, self.position.z); 
        Vector3 snapPlayerPosition = treasure.self.position;
        if (player.self.position.x < treasure.self.position.x)
        {
            snapPlayerPosition.x += player.self.lossyScale.x / 2;
        }
        else if (player.self.position.x > treasure.self.position.x)
        {
            snapPlayerPosition.x -= player.self.lossyScale.x / 2;
        }
        else if (player.self.position.z < treasure.self.position.z)
        {
            snapPlayerPosition.z += player.self.lossyScale.z / 2;
        }
        else if (player.self.position.z > treasure.self.position.z)
        {
            snapPlayerPosition.z -= player.self.lossyScale.z / 2;
        }

        if (treasure.playerInteractingWith.Count > 1)
            treasure.self.position = snapPlayerPosition;
    }
}
