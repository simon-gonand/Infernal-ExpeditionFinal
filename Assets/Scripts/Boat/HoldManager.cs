using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldManager : MonoBehaviour
{
    public Animator goldBagAnim;
    public GameObject goldBurstParticule;
    public Transform particuleSpawnPoint;

    private Treasure previousTreasure;

    public static HoldManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

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
            AddTreasureToHold(treasure);
        }
    }

    public void AddTreasureToHold(Treasure treasure)
    {
        while (treasure.playerInteractingWith.Count > 0)
        {
            treasure.playerInteractingWith[0].selfRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            treasure.playerInteractingWith[0].selfRigidBody.mass = 1;
            treasure.playerInteractingWith[0].interactingWith.UninteractWith(treasure.playerInteractingWith[0]);
        }

        // Register in score
        ScoreManager.instance.AddScore(treasure.price);

        // Play feedback
        GameObject particule = Instantiate(goldBurstParticule, particuleSpawnPoint.position, goldBurstParticule.transform.rotation);
        Destroy(particule, 5f);
        goldBagAnim.SetTrigger("TreasureAdd");

        AudioManager.AMInstance.boatTreasureCollectSFX.Post(AudioManager.AMInstance.gameObject);

        Destroy(treasure.gameObject);
    }
}
