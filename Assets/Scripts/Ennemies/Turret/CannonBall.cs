using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField]
    private Transform self;

    public int damage;
    private void OnCollisionEnter(Collision collision)
    {
        // Play impact sound
        AudioManager.AMInstance.boatDamagesSFX.Post(gameObject);

        if (collision.collider.CompareTag("Boat"))
        {
            Debug.Log("Boat has been hitted");
            // Feedbacks
            ScoreManager.instance.RemoveScore(damage);
        }

        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (self.position.y < 0.0f)
            Destroy(this.gameObject);
    }
}
