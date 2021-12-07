using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPiqueSous : MonoBehaviour
{
    [SerializeField]
    private Collider awakeZone;
    [SerializeField]
    private PiqueSousAI ai;
    [SerializeField]
    private Transform spawnPoint;

    private bool _isAwake;
    public bool isAwake { set { _isAwake = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAwake && !ai.gameObject.activeSelf)
        {
            ai.gameObject.SetActive(true);

            Vector3 spawn = spawnPoint.position;
            spawn.y += ai.self.lossyScale.y / 2;
            ai.self.position = spawn;
            ai.self.forward = spawnPoint.forward;
        }
        else if (!_isAwake && ai.gameObject.activeSelf)
        {
            ai.gameObject.SetActive(false);
        }
    }
}
