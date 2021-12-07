using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiqueSousAI : MonoBehaviour
{
    public Transform self;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            self.Translate(Vector3.forward * Time.deltaTime);
        }
    }
}
