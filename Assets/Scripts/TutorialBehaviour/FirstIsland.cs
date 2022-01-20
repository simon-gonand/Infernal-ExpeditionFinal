using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstIsland : MonoBehaviour
{
    [SerializeField]
    private Transform self;

    [SerializeField]
    private Path path;

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
        int nbPlayerOnBoat = 0;
        foreach(PlayerController player in PlayerManager.instance.players)
        {
            if (player.isOnBoat)
                ++nbPlayerOnBoat;
        }
        if (nbPlayerOnBoat == PlayerManager.instance.players.Count)
        {
            path.links[0].speed = 0.2f;
            this.enabled = false;
        }
    }
}
