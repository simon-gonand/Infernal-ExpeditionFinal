using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPiqueSous : MonoBehaviour
{
    [SerializeField]
    private PiqueSousAI ai;
    public Transform spawnPoint;

    private List<Transform> _seen;
    public List<Transform> seen { get { return _seen; } }

    private List<Treasure> _treasureInZone;
    public List<Treasure> treasureInZone { get { return _treasureInZone; } }

    private bool _isAwake;
    public bool isAwake { set { _isAwake = value; } }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawn = spawnPoint.position;
        spawn.y += ai.self.lossyScale.y / 2;
        ai.self.position = spawn;
        ai.self.forward = spawnPoint.forward;

        _seen = new List<Transform>();
        _treasureInZone = new List<Treasure>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAwake)
        {
            ai.isAwake = true;
        }
        else if (!_isAwake)
        {
            ai.isAwake = false;
        }
    }
}
