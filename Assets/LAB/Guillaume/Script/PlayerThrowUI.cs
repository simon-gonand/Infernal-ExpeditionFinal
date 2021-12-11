using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerThrowUI : MonoBehaviour
{
    public SpriteRenderer activeCircle;

    public Image coneBackGround;
    public Image coneFiller;

    public PlayerController selfPlayerController;



    private void Start()
    {
        activeCircle.enabled = false;
    }

    private void Update()
    {
        if (selfPlayerController.isLaunching == true)
        {
            activeCircle.enabled = true;
        }
        else
        {
            activeCircle.enabled = false;
        }
    }

}
