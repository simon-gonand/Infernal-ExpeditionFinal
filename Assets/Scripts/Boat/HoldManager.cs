using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldManager : MonoBehaviour
{
    public Animator goldBagAnim;
    public GameObject goldBurstParticule;
    public Transform particuleSpawnPoint;

    private Treasure previousTreasure;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasures"))
        {
            Treasure treasure = other.GetComponent<Treasure>();

            if (treasure == previousTreasure)
            {
                return;
            }
            else
            {
                previousTreasure = treasure;
            }

            while (treasure.playerInteractingWith.Count > 0)
            {
                treasure.playerInteractingWith[0].selfRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
                treasure.playerInteractingWith[0].selfRigidBody.mass = 1;
                treasure.playerInteractingWith[0].interactingWith.UninteractWith(treasure.playerInteractingWith[0]);
            }

            // Register in score
            Debug.Log("je rentre");
            ScoreManager.instance.AddScore(other.gameObject.GetComponent<Treasure>().price);

            // Play feedback
            GameObject particule = Instantiate(goldBurstParticule, particuleSpawnPoint.position, goldBurstParticule.transform.rotation);
            Destroy(particule, 5f);
            goldBagAnim.SetTrigger("TreasureAdd");

            AudioManager.AMInstance.boatTreasureCollectSFX.Post(AudioManager.AMInstance.gameObject);

            Destroy(other.gameObject);
        }
    }
}
