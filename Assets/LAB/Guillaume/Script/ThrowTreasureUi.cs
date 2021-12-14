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
    public Image backGroundThrow;

 
    private float distanceMax;
    private float angleSimulation;

    private void Start()
    {
        throwFiller.fillAmount = 0;
        throwUiGlobal.SetActive(false);
    }


    private void Update()
    {
        if (selfTreasure.playerInteractingWith.Count > 0)
        {
            if (selfTreasure.isLoadingPower)
            {
                throwUiGlobal.SetActive(true);

                Quaternion lookRotation = Quaternion.LookRotation(selfTreasure.playerThrowDir);
                Vector3 rotation = lookRotation.eulerAngles;
                objToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

                throwFiller.fillAmount =  selfTreasure.launchForce / selfTreasure.category.maxLaunchForce;


                angleSimulation = Vector3.Angle((selfTreasure.playerThrowDir.normalized + (Vector3.up * selfTreasure.category.multiplyUpAngle)), selfTreasure.playerThrowDir);

                Debug.Log((selfTreasure.playerThrowDir.normalized + (Vector3.up * selfTreasure.category.multiplyUpAngle)).normalized);

                float distForce = (selfTreasure.category.maxLaunchForce / (selfTreasure.category.maxPlayerCarrying + 1 - selfTreasure.playerInteractingWith.Count));
                distanceMax = ((distForce  * distForce) * Mathf.Sin(2 * (Mathf.Deg2Rad * angleSimulation))) / (9.8f * 3);

                distanceMax = distanceMax / selfTreasure.self.lossyScale.x;

                Debug.Log(distanceMax);

                // Set UI position and size correctly 
                throwFiller.rectTransform.sizeDelta = new Vector2(distanceMax, throwFiller.rectTransform.rect.height);
                throwFiller.rectTransform.localPosition = new Vector3(0, 0, distanceMax / 2);

                backGroundThrow.rectTransform.sizeDelta = new Vector2(distanceMax, backGroundThrow.rectTransform.rect.height);
                backGroundThrow.rectTransform.localPosition = new Vector3(0, 0, distanceMax / 2);

            }
            else
            {
                throwUiGlobal.SetActive(false);
            }
        }
        else
        {
            throwUiGlobal.SetActive(false);
        }
    }
}
