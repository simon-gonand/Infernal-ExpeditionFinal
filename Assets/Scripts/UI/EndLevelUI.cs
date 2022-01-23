using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EndLevelUI : MonoBehaviour
{
    [Header("Self References")]
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private Image fillerStar;

    [Header("External Reference")]
    [SerializeField]
    private UiScore uiScore;
    [SerializeField]
    private Button firstSelected;

    private Button lastSelected;

    public static EndLevelUI instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void InitializeUI()
    {
        AudioManager.AMInstance.mapCompletedSWITCH.Post(AudioManager.AMInstance.gameObject);

        panel.SetActive(true);
        firstSelected.Select();
        score.text = ScoreManager.instance.actualScore.ToString();
        fillerStar.color = uiScore.imageFillerActualStar.color;
    }

    private void Update()
    {
        if (panel.activeSelf)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                lastSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            else
            {
                lastSelected.Select();
            }
        }
    }
}
