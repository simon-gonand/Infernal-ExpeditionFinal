using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldManager : MonoBehaviour
{
    public Animator goldBagAnim;
    public GameObject goldBurstParticule;
    public Transform particuleSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasures"))
        {
            // Register in score
            ScoreManager.instance.AddScore(other.gameObject.GetComponent<Treasure>().price);

            // Play feedback
            GameObject particule = Instantiate(goldBurstParticule, particuleSpawnPoint.position, goldBurstParticule.transform.rotation);
            Destroy(particule, 5f);
            goldBagAnim.SetTrigger("TreasureAdd");

            AudioManager.AMInstance.boatTreasureCollectSFX.Post(gameObject);

            Destroy(other.gameObject);
        }
    }
}
