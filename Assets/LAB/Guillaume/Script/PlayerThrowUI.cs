using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerThrowUI : MonoBehaviour
{
    #region Variable
    [Header ("Script needed")]
    public PlayerController selfPlayerController;
    public CarryPlayer selfCarryPlayer;

    [Header ("Launch circle")]
    public SpriteRenderer activeCircle;

    [Header ("Throw cone")]
    public GameObject globaleConeCanvas;
    public Transform objToRotate;
    public Image coneFiller;
    public Image coneBackground;

    private float distanceMax;
    private float angleSimulation;
    #endregion

    private void Start()
    {
        activeCircle.enabled = false;
        globaleConeCanvas.SetActive(false);
        coneFiller.fillAmount = 0f;
    }

    private void Update()
    {
        CircleActivation();

        if (selfPlayerController.isCarried)
        {
            if (selfCarryPlayer.carrier.isAiming && selfCarryPlayer.carrier.isLaunching == true)
            {
                selfCarryPlayer.carrier.selfPlayerThrowUi.globaleConeCanvas.SetActive(true);

                Quaternion lookRotation = Quaternion.LookRotation(selfCarryPlayer.carrier.playerThrowDir);
                Vector3 rotation = lookRotation.eulerAngles;
                selfCarryPlayer.carrier.selfPlayerThrowUi.objToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

                selfCarryPlayer.carrier.selfPlayerThrowUi.coneFiller.fillAmount = selfCarryPlayer.launchForce / selfPlayerController.playerPreset.maxLaunchForce;

                angleSimulation = Vector3.Angle(selfCarryPlayer.carrier.playerThrowDir.normalized + Vector3.up, selfCarryPlayer.carrier.playerThrowDir);

                distanceMax = ((selfPlayerController.playerPreset.maxLaunchForce * selfPlayerController.playerPreset.maxLaunchForce) * Mathf.Sin(2 * angleSimulation)) / 9.8f;

                distanceMax = distanceMax / selfPlayerController.self.lossyScale.x;

                // Set UI position and size correctly
                selfCarryPlayer.carrier.selfPlayerThrowUi.coneFiller.rectTransform.sizeDelta = new Vector2(distanceMax, selfCarryPlayer.carrier.selfPlayerThrowUi.coneFiller.rectTransform.rect.height);
                selfCarryPlayer.carrier.selfPlayerThrowUi.coneFiller.rectTransform.localPosition = new Vector3(0, 0, distanceMax / 2);

                selfCarryPlayer.carrier.selfPlayerThrowUi.coneBackground.rectTransform.sizeDelta = new Vector2(distanceMax, selfCarryPlayer.carrier.selfPlayerThrowUi.coneBackground.rectTransform.rect.height);
                selfCarryPlayer.carrier.selfPlayerThrowUi.coneBackground.rectTransform.localPosition = new Vector3(0, 0, distanceMax / 2);


            }
            else
            {
                selfCarryPlayer.carrier.selfPlayerThrowUi.globaleConeCanvas.SetActive(false);
            }
        }
        else
        {
            selfPlayerController.selfPlayerThrowUi.globaleConeCanvas.SetActive(false);
        }
        
    }


    private void CircleActivation()
    {
        if (selfPlayerController.isLaunching == true)
        {
            activeCircle.enabled = true;
        }
        else
        {
            activeCircle.enabled = false;
        }
    }

}
