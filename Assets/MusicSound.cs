using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSound : MonoBehaviour
{
    public AK.Wwise.Event themeAudio;



    // Start is called before the first frame update
    void Start()
    {
        themeAudio.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
