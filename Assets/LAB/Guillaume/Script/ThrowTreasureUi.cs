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

            Quaternion lookRotation = Quaternion.LookRotation(selfTreasure.playerThrowDir);

            objToRotate.rotation = Quaternion.Slerp(objToRotate.rotation, lookRotation, Time.deltaTime * 5);
        }
        else
        {
            throwUiGlobal.SetActive(false);
        }
    }
}
