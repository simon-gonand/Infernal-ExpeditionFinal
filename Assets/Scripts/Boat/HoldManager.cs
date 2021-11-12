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
            // Register in score
            Destroy(other.gameObject);
        }
    }
}
