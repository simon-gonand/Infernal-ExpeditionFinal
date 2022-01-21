using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierUiManager : MonoBehaviour
{
    public static ModifierUiManager instance;

    public float timeBeforeMove;
    public float durationTime;


    [Header ("Token reference")]
    public GameObject noAttackToken;
    public GameObject noDashToken;
    public GameObject expensiveTreasureToken;
    public GameObject sickToken;
    public GameObject transformationToken;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    
    public void SpawnNoDashToken(float _duration)
    {
        GameObject obj = Instantiate(noDashToken, transform.position, transform.rotation);
        obj.transform.SetParent(transform);
        obj.GetComponent<ModifierLogic>().SetValue(timeBeforeMove, _duration);
    }

    public void SpawnNoAttackToken(float _duration)
    {
        GameObject obj = Instantiate(noAttackToken, transform.position, transform.rotation);
        obj.transform.SetParent(transform);
        obj.GetComponent<ModifierLogic>().SetValue(timeBeforeMove, _duration);
    }

}
