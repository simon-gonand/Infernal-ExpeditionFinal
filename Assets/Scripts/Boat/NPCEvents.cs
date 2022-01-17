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

    private GameObject currentObject;


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
            Destroy(currentObject);
            time = 0;
        }
    }
    public void OnSpeedUp()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
            time = 0;
        }
        currentObject = Instantiate(speedUpToken, displayToken.transform);

        displayingToken = true;
        Debug.Log("Speed up !");
    }

    public void OnSpeedDown()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
            time = 0;
        }
        currentObject = Instantiate(speedDownToken, displayToken.transform);

        displayingToken = true;
        Debug.Log("Speed down !");
    }

    public void OnLastCall()
    {
        Debug.Log("Last Island !");
    }

    public void StartLanding(Transform t)
    {
        PlayerManager.instance.respawnOnBoat = false;
        PlayerManager.instance.respawnPoint = t.position;
        foreach (PlayerController player in PlayerManager.instance.players)
        {
            if (player.isOnBoat)
                player.self.position = t.position;
        }

        StartCoroutine(RemoveBoatSmoothly());
    }

    private IEnumerator RemoveBoatSmoothly()
    {
        int index = GameManager.instance.targetGroup.FindMember(BoatManager.instance.self);
        for (int i = 25; i > 0; --i)
        {
            GameManager.instance.targetGroup.m_Targets[index].weight = i;
            yield return new WaitForSeconds(0.2f);
        }
        GameManager.instance.targetGroup.RemoveMember(BoatManager.instance.self);
    }

    public void EndLanding()
    {
        PlayerManager.instance.respawnOnBoat = true;
        StartCoroutine(AddBoatSmoothly());
    }

    private IEnumerator AddBoatSmoothly()
    {
        GameManager.instance.targetGroup.AddMember(BoatManager.instance.self, 0.01f, 20);
        int index = GameManager.instance.targetGroup.FindMember(BoatManager.instance.self);
        for (int i = 0; i <= 25; ++i)
        {
            GameManager.instance.targetGroup.m_Targets[index].weight += i;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
