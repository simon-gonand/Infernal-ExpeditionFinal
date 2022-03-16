using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FifthIsland : MonoBehaviour
{
    public GameObject globalUi;

    [SerializeField]
    private Path path;

    [SerializeField]
    private OpenGate gate;

    private bool boatStarted;
    private float tAccel;
    private float boatSpeed = 0.1f;

    void Update()
    {
        if (boatStarted)
        {
            if (tAccel > 1.0f)
            {
                this.enabled = false;
                return;
            }
            else
            {
                tAccel += Time.deltaTime * 0.5f;
                path.links[4].speed = Mathf.Lerp(0.0f, boatSpeed, tAccel);
                return;
            }
        }
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
                boatStarted = true;
            }
        }
    }
}
