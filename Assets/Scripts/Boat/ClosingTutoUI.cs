using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingTutoUI : MonoBehaviour
{
    public static ClosingTutoUI Instance;

    public bool closeTuto = false;

    public GameObject billboardUIActivate;

    public void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        CheckingUiActivation();
    }

    public void CheckingUiActivation()
    {
        if (billboardUIActivate.activeInHierarchy)
        {
            closeTuto = true;
        }
    }

    public void ResumeGame()
    {
        if (closeTuto)
        {
            Debug.Log("Resume Game");
            Time.timeScale = 1.0f;

            closeTuto = false;

            billboardUIActivate.SetActive(false);
        }
    }
}
