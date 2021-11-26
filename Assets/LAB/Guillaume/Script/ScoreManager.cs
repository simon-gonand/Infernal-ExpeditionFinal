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

    [Space]
    public bool gotBronzeStar;
    public bool gotSilverStar;
    public bool gotGoldStar;

    private int scoreNeedForNextStar;
    private int scoreOfGainStar;

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple ScoreManager in the scene");
        }
    }

    private void Start()
    {
        InititScore();
    }

    public void AddPoint(int point)
    {
        actualScore += point;
    }

    private void ScoreTextUpdate()
    {
        scoreText.text = actualScore.ToString();
    }

    private void SliderUpdate()
    {
        scoreSlider.value = actualScore;
    }

    private void StarUpdate()
    {
        if (actualScore - scoreOfGainStar >= scoreNeedForNextStar)
        {
            scoreOfGainStar += scoreNeedForNextStar;

            if (!gotBronzeStar)
            {
                gotBronzeStar = true;
                scoreOfGainStar += scoreNeedForBronze;
                scoreNeedForNextStar = scoreNeedForSilver;

                actualStarFiller.color = bronzeColor;
                nextStartFiller.color = silverColor;
            }
            else if (!gotSilverStar)
            {
                gotSilverStar = true;
                scoreOfGainStar += scoreNeedForSilver;
                scoreNeedForNextStar = scoreNeedForGold;

                actualStarFiller.color = silverColor;
                nextStartFiller.color = goldColor;
            }
            else if ()
            {

            }
        }
    }

    public void FullScoreUpdate()
    {
        ScoreUpdate();
        SliderUpdate();
    }



    private void InititScore()
    {
        actualScore = 0;

        scoreText.text = actualScore.ToString();

        scoreSlider.maxValue = scoreNeedForBronze;
        fillSlider.color = bronzeColor;
        scoreSlider.value = actualScore;

        nextStartFiller.color = bronzeColor;
    }

}
