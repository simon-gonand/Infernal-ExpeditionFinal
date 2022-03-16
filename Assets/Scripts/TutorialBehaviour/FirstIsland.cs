using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstIsland : MonoBehaviour
{
    [SerializeField]
    private Transform self;

    [SerializeField]
    private Path path;

    private bool boatStarted = false;
    private float boatSpeed = 0.1f;
    private float tAccel = 0.0f;
    private float tDecel = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach(PlayerController player in PlayerManager.instance.players)
        {
            player.self.position = self.position;
            self.position = new Vector3(self.position.x + player.selfCollider.bounds.size.x, self.position.y, self.position.z);
            player.self.SetParent(BoatManager.instance.self.parent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (boatStarted)
        {
            if (tDecel >= 1.0f) 
            {
                this.enabled = false;
                return;
            }
            if (tAccel >= 1.0f)
            {
                if (BoatManager.instance.GetComponent<FollowPath>().tParam >= 0.85f)
                {
                    tDecel += Time.deltaTime * 0.5f;
                    path.links[0].speed = Mathf.Lerp(boatSpeed, 0.01f, tDecel);
                    return;
                }
            }
            else
            {
                tAccel += Time.deltaTime * 0.5f;
                path.links[0].speed = Mathf.Lerp(0.0f, boatSpeed, tAccel);
                return;
            }
        }
        int nbPlayerOnBoat = 0;
        foreach(PlayerController player in PlayerManager.instance.players)
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
