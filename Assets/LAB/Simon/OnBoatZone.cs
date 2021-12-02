using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoatZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if(!player.isCarried)
                player.self.SetParent(BoatManager.instance.self);
            player.isOnBoat = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (!player.isCarried)
                player.self.SetParent(null);
            player.isOnBoat = false;
        }
    }
}
