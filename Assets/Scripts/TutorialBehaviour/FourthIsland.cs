using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthIsland : MonoBehaviour
{
    public GameObject globalUi;

    [SerializeField]
    private Path path;

    [SerializeField]
    private OpenGate gate;


    void Update()
    {
        if (gate.isOpen)
        {
            globalUi.SetActive(false);

            int nbPlayerOnBoat = 0;
            foreach (PlayerController player in PlayerManager.instance.players)
            {
                if (player.isOnBoat)
                    ++nbPlayerOnBoat;
            }
            if (nbPlayerOnBoat == PlayerManager.instance.players.Count)
            {
                path.links[3].speed = 0.1f;
                this.enabled = false;
            }
        }
    }
}
