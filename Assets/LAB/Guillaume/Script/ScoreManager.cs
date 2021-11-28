using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    #region Variable

    [Header ("Actual score")]
    public int actualScore;

    [Header ("Score need for each state")]
    public int scoreNeedForBronze;
    public int scoreNeedForSilver;
    public int scoreNeedForGold;

    [Header("Color")]
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color bronzeColor;
    [SerializeField] private Color silverColor;
    [SerializeField] private Color goldColor;

    [Header("Ui Info")]
    public TextMeshProUGUI scoreText;
    public Slider scoreSlider;
    public Image fillSlider;
    [Space]
    public Image actualStarFiller;
    public Image nextStartFiller;

    [HideInInspector]public bool gotBronzeStar;
    [HideInInspector] public bool gotSilverStar;
    [HideInInspector] public bool gotGoldStar;

    [SerializeField]private int scoreNeedForNextStar;
    [SerializeField]private int scoreOfGainStar;

    #endregion

    private void Awake()
    {
        #region Set instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple ScoreManager in the scene");
        }
        #endregion
    }

    private void Start()
    {
        InititScore();
    }

    public void AddPoint(int point)
    {
        actualScore += point;

        UiScoreUpdate();
    }
    public void UiScoreUpdate()
    {
        ScoreTextUpdate();
        StarsUpdate();
        SliderUpdate();
    }


    private void ScoreTextUpdate()
    {
        scoreText.text = actualScore.ToString();
    }
    private void SliderUpdate()
    {
        scoreSlider.value = actualScore;

        /*
        if (gotBronzeStar && !gotSilverStar)
        {
            scoreSlider.maxValue = scoreNeedForSilver;
            fillSlider.color = silverColor;
        }
        else if (gotSilverStar && !gotGoldStar && !gotBronzeStar)
        {
            scoreSlider.maxValue = scoreNeedForGold;
            fillSlider.color = goldColor;
        }
        */
    }
    private void StarsUpdate()
    {
        if (actualScore >= scoreNeedForNextStar)
        {
            //scoreOfGainStar += scoreNeedForNextStar;

            if (!gotBronzeStar)
            {
                gotBronzeStar = true;
                scoreOfGainStar = scoreNeedForBronze;
                scoreNeedForNextStar = scoreNeedForSilver;

                actualStarFiller.color = bronzeColor;
                nextStartFiller.color = silverColor;


                scoreSlider.maxValue = scoreNeedForSilver;
                fillSlider.color = silverColor;
            }
            else if (!gotSilverStar)
            {
                gotSilverStar = true;
                scoreOfGainStar = scoreNeedForSilver;
                scoreNeedForNextStar = scoreNeedForGold;

                actualStarFiller.color = silverColor;
                nextStartFiller.color = goldColor;


                scoreSlider.maxValue = scoreNeedForGold;
                fillSlider.color = goldColor;
            }
            else if (!gotGoldStar)
            {
                gotGoldStar = true;

                actualStarFiller.color = goldColor;
                nextStartFiller.color = goldColor;
            }
        }
    }


    private void InititScore()
    {
        actualScore = 0;

        scoreText.text = actualScore.ToString();

        scoreSlider.maxValue = scoreNeedForBronze;
        fillSlider.color = bronzeColor;
        scoreSlider.value = actualScore;

        nextStartFiller.color = bronzeColor;
        actualStarFiller.color = emptyColor;

        scoreNeedForNextStar = scoreNeedForBronze;
    }

}
