using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowTreasureUi : MonoBehaviour
{
    public Treasure selfTreasure;

    public Transform objToRotate;
    public GameObject throwUiGlobal;
    public Image backGroundThrow;

 
    private float distanceMax;
    private float angleSimulation;

    private void Start()
    {
        throwUiGlobal.SetActive(false);
    }

    private bool CheckIfPlayerAreLaunching()
    {
        foreach(PlayerController player in selfTreasure.playerInteractingWith)
        {
            if (!player.isLaunching) return false;
        }
        return true;
    }

    private void Update()
    {
        if (selfTreasure.playerInteractingWith.Count > 0)
        {
            if (CheckIfPlayerAreLaunching())
            {
                throwUiGlobal.SetActive(true);

                Quaternion lookRotation = Quaternion.LookRotation(selfTreasure.playerThrowDir);
                Vector3 rotation = lookRotation.eulerAngles;
                objToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

                angleSimulation = Vector3.Angle((selfTreasure.playerThrowDir.normalized + (Vector3.up * selfTreasure.category.multiplyUpAngle)), selfTreasure.playerThrowDir);

                float distForce = selfTreasure.category.forceNbPlayer[selfTreasure.playerInteractingWith.Count - 1];
                distanceMax = ((distForce  * distForce) * Mathf.Sin(2 * (Mathf.Deg2Rad * angleSimulation))) / (9.8f * 2.5f);

                distanceMax = distanceMax / selfTreasure.self.lossyScale.x;

                // Set UI position and size correctly 
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
