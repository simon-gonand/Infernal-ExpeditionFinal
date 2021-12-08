using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldManager : MonoBehaviour
{
    #region Audio
    public AK.Wwise.Event treasureCollectAudio = AudioManager.AMInstance.boatTreasureCollectSFX;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasures"))
        {
            Debug.Log("Treasure has been registered in the hold");
            // Play feedback
            treasureCollectAudio.Post(gameObject);
            // Register in score
            Destroy(other.gameObject);
        }
    }
}
