using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCEvents : MonoBehaviour
{
    [Header("Pop UP")]
    public Transform spawnPopUp;
    public ParticleSystem popUpSystem;
    public UIParticleSystem popUpParticleSystem;

    [Space(10)]
    public GameObject popUpDisplay;
    public Sprite[] popUp = new Sprite[4];

    [Header("Black Bands Intro")]
    public Animator aboveBand;
    public Animator belowBand;

    [Header("UI")]
    public GameObject tutoBillboardUI;
    public List<GameObject> endLandingToken;

    private int landingCount = 0;

    private void Start()
    {
        if (aboveBand == null && belowBand == null)
        {
            aboveBand = null;
            belowBand = null;
        }
    }

    

    //LEVEL ADVERTISING
    public void OnStartLevel()
    {
        popUpParticleSystem.Particle = popUp[0];
        popUpParticleSystem.Size = 5;
        popUpParticleSystem.ChangeParticle();
        popUpParticleSystem.Play();
    }

    public void OnLastIsland()
    {
        popUpParticleSystem.Particle = popUp[1];
        popUpParticleSystem.Size = 5;
        popUpParticleSystem.ChangeParticle();
        popUpParticleSystem.Play();
    }

    //CAPTAIN NPC 
    public void OnCaptainCallLanding()
    {
        popUpParticleSystem.Particle = popUp[2];
        popUpParticleSystem.Size = 7.5f;
        popUpParticleSystem.ChangeParticle();
        popUpParticleSystem.Play();

    }

    public void OnCaptainCallAllAboard()
    {
        popUpParticleSystem.Particle = popUp[3];
        popUpParticleSystem.Size = 10;
        popUpParticleSystem.ChangeParticle();
        popUpParticleSystem.Play();
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
