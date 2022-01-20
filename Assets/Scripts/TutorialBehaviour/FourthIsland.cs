using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthIsland : MonoBehaviour
{
    [SerializeField]
    private Path path;

    [SerializeField]
    private OpenGate gate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gate.isOpen)
        {
            int nbPlayerOnBoat = 0;
            foreach (PlayerController player in PlayerManager.instance.players)
            {
                if (player.isOnBoat)
                    ++nbPlayerOnBoat;
            }
            if (nbPlayerOnBoat == PlayerManager.instance.players.Count)
            {
                path.links[3].speed = 0.2f;
                this.enabled = false;
            }
        }
    }
}
