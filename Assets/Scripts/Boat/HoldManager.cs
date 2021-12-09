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

            ScoreManager.instance.AddScore(1);
            Destroy(other.gameObject);
        }
    }
}
