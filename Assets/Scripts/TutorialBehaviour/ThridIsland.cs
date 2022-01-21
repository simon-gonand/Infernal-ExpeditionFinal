using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThridIsland : MonoBehaviour
{
    [Header ("UI")]
    public GameObject globalUi;
    public TextMeshProUGUI counter;

    [SerializeField]
    private Path path;

    [SerializeField]
    private List<Treasure> treasures;

    void Update()
    {
        if (treasures.Count == 0)
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
                path.links[2].speed = 0.1f;
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

        counter.text = treasures.Count.ToString();
    }
}
