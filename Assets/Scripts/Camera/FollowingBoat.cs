using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingBoat : MonoBehaviour
{
    [SerializeField]
    private Transform self;

    private Vector3 initialOffset;
    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        // Get the offset between the camera and the boat
        initialOffset = self.position - BoatManager.instance.self.position;
        initialPos = self.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the position of the camera according to the boat on Z
        self.position = new Vector3(BoatManager.instance.self.position.x + initialOffset.x, initialPos.y,
            BoatManager.instance.self.position.z + initialOffset.z) ;
    }
}
