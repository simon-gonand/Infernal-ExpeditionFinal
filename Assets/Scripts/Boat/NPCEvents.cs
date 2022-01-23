using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCEvents : MonoBehaviour
{
    [Header("Pop UP")]
    public Transform spawnPopUp;
    public ParticleSystem popUpSystem;

    [Space(10)]
    public GameObject popUpDisplay;
    public GameObject[] popUp = new GameObject[4];

    [Header("Black Bands Intro")]
    public Animator aboveBand;
    public Animator belowBand;

    [Header("UI")]
    public GameObject tutoBillboardUI;
    public List<GameObject> endLandingToken;

    private int landingCount = 0;

    private void Start()
    {
        for (int i = 0; i < popUp.Length; i++)
        {            popUp[i].SetActive(true);
        }
        if (aboveBand == null && belowBand == null)
        {
            aboveBand = null;
            belowBand = null;
        }
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
    }

    //CAPTAIN NPC 
    public void OnCaptainCallLanding()
    {
        popUp[2].SetActive(true);

        popUpDisplay = popUp[2];

        for (int i = 0; i < popUp.Length; i++)
        {
            if (popUp[i] != popUpDisplay)
            {
                popUp[i].SetActive(false);
            }
        }
        popUpSystem.transform.position = spawnPopUp.position;
        popUpSystem.Play();
        
    }

    public void OnCaptainCallAllAboard()
    {
        popUp[3].SetActive(true);

        popUpDisplay = popUp[3];

        for (int i = 0; i < popUp.Length; i++)
        {
            if (popUp[i] != popUpDisplay)
            {
                popUp[i].SetActive(false);
            }
        }

        popUpSystem.transform.position = spawnPopUp.position;
        popUpSystem.Play();
    }

    //TUTO 
    public void DisplayingTutorialBillboard()
    {
        tutoBillboardUI.SetActive(true);
        ClosingTutoUI.Instance.closeTuto = true;      
        Time.timeScale = 0.0f;
    }
    //BLACK BANDS
    public void BlackBandsRemove()
    {
        aboveBand.SetBool("aboveBandRemove", true);
        belowBand.SetBool("belowBandRemove", true);

        aboveBand.SetBool("aboveBandDisplay", false);
        belowBand.SetBool("belowBandDisplay", false);
    }

    public void BlackBandsDisplay()
    {
        aboveBand.SetBool("aboveBandDisplay", true);
        belowBand.SetBool("belowBandDisplay", true);

        aboveBand.SetBool("aboveBandRemove", false);
        belowBand.SetBool("belowBandRemove", false);
    }

    //TOKEN

    public void OnDisplayingToken(GameObject token)
    {
        token.SetActive(true);
    }


    //END UI

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

        endLandingToken[landingCount].gameObject.SetActive(true);
        

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
        endLandingToken[landingCount].gameObject.SetActive(false);
        ++landingCount;
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
