using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCEvents : MonoBehaviour
{
    public Transform spawnPopUp;
    public ParticleSystem popUpSystem;

    [Space(10)]
    public GameObject popUpDisplay;
    public GameObject[] popUp;



    private void Start()
    {
        for (int i = 0; i < popUp.Length; i++)
        {
            popUp[i].SetActive(true);
        }
    }

    //CAPTAIN NPC 
    public void OnCaptainCallLanding()
    {
        popUpSystem.transform.position = spawnPopUp.position;
        popUpSystem.Play();
        Debug.Log("Landing !");
    }

    public void OnCaptainCallAllAboard()
    {
        popUpSystem.transform.position = spawnPopUp.position;
        popUpSystem.Play();
        Debug.Log("All Aboard !");
    }

    //LEVEL ADVERTISING
    public void OnStartLevel()
    {
        popUp[0].SetActive(true);

        popUpDisplay = popUp[0];

        for (int i = 0; i < popUp.Length; i++)
        {
            if (popUp[i] != popUpDisplay)
            {
                popUp[i].SetActive(false);
            }
        }

        popUpSystem.transform.position = spawnPopUp.position;
        popUpSystem.Play();
        Debug.Log("Level Start !");
    }

    public void OnLastIsland()
    {
        popUp[1].SetActive(true);

        popUpDisplay = popUp[1];

        for (int i = 0; i < popUp.Length; i++)
        {
            if (popUp[i] != popUpDisplay)
            {
                popUp[i].SetActive(false);
            }
        }

        popUpSystem.transform.position = spawnPopUp.position;
        popUpSystem.Play();
        Debug.Log("Last Island !");
    }

    public void StartLanding(Transform t)
    {
        PlayerManager.instance.respawnOnBoat = false;
        PlayerManager.instance.respawnPoint = t;
        foreach (PlayerController player in PlayerManager.instance.players)
        {
            if (player.isOnBoat)
            {
                player.self.position = t.position;
            }
        }

        StartCoroutine(RemoveBoatSmoothly());
    }

    private IEnumerator RemoveBoatSmoothly()
    {
        int index = GameManager.instance.targetGroup.FindMember(BoatManager.instance.self);
        for (int i = 25; i > 0; --i)
        {
            GameManager.instance.targetGroup.m_Targets[index].weight = i;
            yield return new WaitForSeconds(0.05f);
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
            yield return new WaitForSeconds(0.05f);
        }
    }
}
