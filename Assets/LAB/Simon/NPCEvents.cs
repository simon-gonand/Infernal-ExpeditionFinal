using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEvents : MonoBehaviour
{
    public void OnSpeedUp()
    {
        Debug.Log("Speed up !");
    }

    public void OnSpeedDown()
    {
        Debug.Log("Speed down !");
    }

    public void OnLastCall()
    {
        Debug.Log("Last Island !");
    }
}
