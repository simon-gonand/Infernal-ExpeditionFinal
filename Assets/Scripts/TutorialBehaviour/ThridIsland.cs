using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridIsland : MonoBehaviour
{
    [SerializeField]
    private Path path;

    [SerializeField]
    private List<Treasure> treasures;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (treasures.Count == 0)
        {
            int nbPlayerOnBoat = 0;
            foreach (PlayerController player in PlayerManager.instance.players)
            {
                if (player.isOnBoat)
                    ++nbPlayerOnBoat;
            }
            if (nbPlayerOnBoat == PlayerManager.instance.players.Count)
            {
                path.links[2].speed = 0.2f;
                this.enabled = false;
            }
        }

        else
        {
            for (int i = 0; i < treasures.Count; ++i)
            {
                if (treasures[i] == null)
                    treasures.RemoveAt(i);
            }
        }
    }
}
