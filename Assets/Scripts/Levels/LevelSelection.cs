using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelSelection : MonoBehaviour
{
    public GameObject inputButtonA;
    public bool canActivateLevelSelectionUI = false;

    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera tableCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateLevelSelectionUi()
    {
        if (canActivateLevelSelectionUI)
        {
            mainCam.Priority = 0;
            tableCam.Priority = 15;

            //D�sactiver mort PJ

            Debug.Log("LaunchLevelUI");
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputButtonA.SetActive(true);
            canActivateLevelSelectionUI = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputButtonA.SetActive(false);
            canActivateLevelSelectionUI = false;
        }
    }
}
