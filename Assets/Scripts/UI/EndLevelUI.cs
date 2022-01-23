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
    private Image earnCoin;

    [Header("External Reference")]
    [SerializeField]
    private UiScore uiScore;
    [SerializeField]
    private Button firstSelected;
    [SerializeField]
    private List<Sprite> coins;

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
        switch (ScoreManager.instance.actualStar)
        {
            case ScoreManager.differentStarState.Bronze:
                earnCoin.sprite = coins[0];
                break;
            case ScoreManager.differentStarState.Silver:
                earnCoin.sprite = coins[1];
                break;
            case ScoreManager.differentStarState.Gold:
                earnCoin.sprite = coins[2];
                break;
        }
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
