using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEvents : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private bool displayingToken;
    public float timeToDisplay;

    [SerializeField] private GameObject displayToken;

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
            displayToken.SetActive(true);
        }

        if (time >= timeToDisplay)
        {
            displayingToken = false;
            displayToken.SetActive(false);
            time = 0;
        }
    }
    public void OnSpeedUp()
    {
        displayToken = speedUpToken;

        displayingToken = true;
        Debug.Log("Speed up !");
    }

    public void OnSpeedDown()
    {
        displayToken = speedDownToken;

        displayingToken = true;
        Debug.Log("Speed down !");
    }

    public void OnLastCall()
    {
        Debug.Log("Last Island !");
    }
}
