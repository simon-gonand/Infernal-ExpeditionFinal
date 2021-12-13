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
    #endregion

    private void Start()
    {
        activeCircle.enabled = false;
        globaleConeCanvas.SetActive(false);
    }

    private void Update()
    {
        CircleActivation();

        /*if (selfPlayerController.isCarried)
        {
            if (selfCarryPlayer.carrier.isAiming && selfCarryPlayer.carrier.isLaunching == true)
            {
                selfCarryPlayer.carrier.selfPlayerThrowUi.globaleConeCanvas.SetActive(true);

                Quaternion lookRotation = Quaternion.LookRotation(selfCarryPlayer.carrier.playerThrowDir);
                Vector3 rotation = lookRotation.eulerAngles;
                selfCarryPlayer.carrier.selfPlayerThrowUi.objToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            }
        }*/
        /*else if ()
        {
            selfCarryPlayer.carrier.selfPlayerThrowUi.globaleConeCanvas.SetActive(false);
        }*/
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
