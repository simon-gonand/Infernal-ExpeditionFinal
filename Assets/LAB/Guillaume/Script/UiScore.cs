using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiScore : MonoBehaviour
{
    public static UiScore instance;

    [Header ("Color setup")]
    public Color noColor;
    public Color bronzeColor;
    public Color silverColor;
    public Color goldColor;

    [Header ("Unity setup")]
    public Image imageActualStar;
    public Image imageNextStar;
    [Space]
    public TextMeshProUGUI textActualScore;
    public Slider sliderBar;

    private void Awake()
    {
        #region Setup instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("There is multiple UiScore in the scene");
        }
        #endregion
    }

    private void Start()
    {
        ScoreUpdate();
    }

    public void ScoreUpdate()
    {
        textActualScore.text = ScoreManager.instance.actualScore.ToString();
    }

}
