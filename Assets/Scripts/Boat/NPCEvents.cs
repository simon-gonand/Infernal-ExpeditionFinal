using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEvents : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private bool displayingToken;
    public float timeToDisplay;

    [SerializeField] private Transform displayToken;

    [SerializeField] private GameObject speedUpToken;
    [SerializeField] private GameObject speedDownToken;


    private void Start()
    {
        time = 0;
        displayingToken = false;
    }

    private void Update()
    {
        if (displayingToken)
        {
            time += Time.deltaTime;            
        }

        if (time >= timeToDisplay)
        {
            displayingToken = false;
            time = 0;
        }
    }
    public void OnSpeedUp()
    {
        Instantiate(speedUpToken, displayToken.transform);

        displayingToken = true;
        Debug.Log("Speed up !");
    }

    public void OnSpeedDown()
    {
        Instantiate(speedDownToken, displayToken.transform);

        displayingToken = true;
        Debug.Log("Speed down !");
    }

    public void OnLastCall()
    {
        Debug.Log("Last Island !");
    }
}
