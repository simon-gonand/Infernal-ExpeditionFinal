using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasures"))
        {
            Debug.Log("Treasure has been registered in the hold");
            // Play feedback
            AudioManager.AMInstance.boatTreasureCollectSFX.Post(gameObject);
            // Register in score
            Destroy(other.gameObject);
        }
    }
}
