using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndLevelUI : MonoBehaviour
{
    [Header("Sefl References")]
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private Image fillerStar;

    [Header("External Reference")]
    [SerializeField]
    private UiScore uiScore;

    public static EndLevelUI instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void InitializeUI()
    {
        panel.SetActive(true);
        score.text = ScoreManager.instance.actualScore.ToString();
        fillerStar.color = uiScore.imageFillerActualStar.color;
    }
}
