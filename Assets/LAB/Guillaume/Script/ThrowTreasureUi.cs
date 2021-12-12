using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowTreasureUi : MonoBehaviour
{
    public Treasure selfTreasure;

    public Transform objToRotate;
    public GameObject throwUiGlobal;
    public Image throwFiller;


    private void Start()
    {
        throwFiller.fillAmount = 0;
        throwUiGlobal.SetActive(false);
    }


    private void Update()
    {
        if (selfTreasure.isLoadingPower)
        {
            throwUiGlobal.SetActive(true);
        }
        else
        {
            throwUiGlobal.SetActive(false);
        }
    }
}
