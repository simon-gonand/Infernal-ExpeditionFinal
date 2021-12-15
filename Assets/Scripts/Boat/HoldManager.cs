using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasures"))
        {
            // Play feedback
            AudioManager.AMInstance.boatTreasureCollectSFX.Post(gameObject);
            // Register in score

            ScoreManager.instance.AddScore(other.gameObject.GetComponent<Treasure>().price);
            Destroy(other.gameObject);
        }
    }
}
