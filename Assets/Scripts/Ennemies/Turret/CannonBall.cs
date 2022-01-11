using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField]
    private Transform self;

    public int damage;
    public GameObject impactParticule;

    private void OnCollisionEnter(Collision collision)
    {
        // Play impact sound
        AudioManager.AMInstance.boatDamagesSFX.Post(gameObject);
        GameObject particule = Instantiate(impactParticule, transform.position, transform.rotation);
        Destroy(particule, 4f);

        if (collision.collider.CompareTag("Boat"))
        {
            // Feedbacks
            ScoreManager.instance.RemoveScore(damage);

            BoatManager.instance.GetHit();
        }

        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (self.position.y < 0.0f)
            Destroy(this.gameObject);
    }
}
