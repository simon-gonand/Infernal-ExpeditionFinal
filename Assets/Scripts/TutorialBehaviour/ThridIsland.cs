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

    private bool boatStarted;
    private float tAccel;
    private float tDecel;
    private float boatSpeed = 0.1f;

    void Update()
    {
        if (boatStarted)
        {
            if (tDecel > 1.0f)
            {
                this.enabled = false;
                return;
            }
            if (tAccel > 1.0f)
            {
                if (BoatManager.instance.GetComponent<FollowPath>().tParam >= 0.88f)
                {
                    tDecel += Time.deltaTime * 0.5f;
                    path.links[2].speed = Mathf.Lerp(boatSpeed, 0.009f, tDecel);
                    return;
                }
            }
            else
            {
                tAccel += Time.deltaTime * 0.5f;
                path.links[2].speed = Mathf.Lerp(0.0f, boatSpeed, tAccel);
                return;
            }
        }
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
                boatStarted = true;
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
