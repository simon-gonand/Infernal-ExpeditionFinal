using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI instance;

    [Header("Color")]
    public Color basiqueColor;
    public Color bronzeColor;
    public Color silverColor;
    public Color goldColor;

    [Header("Parameter")]
    public float timeForFading;

    [Header("Animator")]
    public Animator bronzeAnimator;
    public Animator silverAnimator;
    public Animator goldAnimator;

    [Header ("Self reference")]
    public Slider selfSlider;
    public RectTransform handle;
    public Image fillImage;
    [Space]
    public Transform bronzeMedalGlobal;
    public Transform silverMedalGlobal;
    public Transform goldMedalGlobal;

    private float scoreOfNextStarUi;
    private float oldScoreOfNextStarUi;

    private float actualUiScore;
    private Coroutine actualCoroutine;

    public void Awake()
    {
        #region instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Care there is multiple ScoreUI manager");
        }
        #endregion
    }

    void Start()
    {
        selfSlider.maxValue = ScoreManager.instance.maxScore;
        SetUpUiScore();

        oldScoreOfNextStarUi = 0f;
        scoreOfNextStarUi = ScoreManager.instance.scoreNeedForBronze;
    }
    
    public void SetUpUiScore()
    {
        selfSlider.value = ScoreManager.instance.scoreNeedForBronze;
        bronzeMedalGlobal.position = handle.position;

        selfSlider.value = ScoreManager.instance.scoreNeedForSilver;
        silverMedalGlobal.position = handle.position;

        selfSlider.value = ScoreManager.instance.scoreNeedForGold;
        goldMedalGlobal.position = handle.position;

        selfSlider.value = 0;

        RefreshSliderColor();
    }
    public void UpdateUiScore()
    {
        if (actualCoroutine == null)
        {
            actualCoroutine = StartCoroutine(UpdateSliderPos(actualUiScore, ScoreManager.instance.actualScore));
        }
        else
        {
            StopCoroutine(actualCoroutine);
            actualCoroutine = StartCoroutine(UpdateSliderPos(actualUiScore, ScoreManager.instance.actualScore));
        }
    }

    private void RefreshSliderColor()
    {
        switch (ScoreManager.instance.actualStar)
        {
            case ScoreManager.differentStarState.NoStar:
                fillImage.color = basiqueColor;
                break;
            case ScoreManager.differentStarState.Bronze:
                fillImage.color = bronzeColor;
                break;
            case ScoreManager.differentStarState.Silver:
                fillImage.color = silverColor;
                break;
            case ScoreManager.differentStarState.Gold:
                fillImage.color = goldColor;
                break;
        }
    }

    private void CheckIfGainMedal()
    {
        Debug.Log("je suis la");
        if (ScoreManager.instance.isLevelUpStar)
        {
            Debug.Log("je rentre");

            ScoreManager.instance.isLevelUpStar = false;

            switch (ScoreManager.instance.actualStar)
            {
                case ScoreManager.differentStarState.Bronze:
                    bronzeAnimator.SetTrigger("GainMedal");
                    scoreOfNextStarUi = ScoreManager.instance.scoreNeedForSilver;
                    oldScoreOfNextStarUi = ScoreManager.instance.scoreNeedForBronze;
                    Debug.Log("Bronze");
                    break;

                case ScoreManager.differentStarState.Silver:
                    silverAnimator.SetTrigger("GainMedal");
                    scoreOfNextStarUi = ScoreManager.instance.scoreNeedForGold;
                    oldScoreOfNextStarUi = ScoreManager.instance.scoreNeedForSilver;
                    Debug.Log("Silver");
                    break;

                case ScoreManager.differentStarState.Gold:
                    oldScoreOfNextStarUi = ScoreManager.instance.scoreNeedForGold;
                    goldAnimator.SetTrigger("GainMedal");
                    Debug.Log("Gold");
                    break;
            }
        }
        else if (ScoreManager.instance.isDowngradeStar)
        {
            ScoreManager.instance.isDowngradeStar = false;
        }

        RefreshSliderColor();
    }


    IEnumerator UpdateSliderPos(float oldValue, float newValue)
    {
        float time = 0f;
        float valueForSlider = 0f;

        while (time < timeForFading)
        {
            valueForSlider = Mathf.Lerp(oldValue, newValue, time / timeForFading);
            actualUiScore = valueForSlider;

            selfSlider.value = valueForSlider;
            time += Time.fixedDeltaTime;

            if (actualUiScore >= scoreOfNextStarUi)
            {
                CheckIfGainMedal();
            }
            else if (actualUiScore < oldScoreOfNextStarUi)
            {
                CheckIfGainMedal();
            }

            yield return new WaitForEndOfFrame();
        }
        actualUiScore = newValue;
        selfSlider.value = newValue;
    }
}
