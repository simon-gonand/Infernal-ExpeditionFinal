using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiScore : MonoBehaviour
{
    #region Variable
    public static UiScore instance;

    [Header ("Color setup")]
    public Color noColor;
    public Color bronzeColor;
    public Color silverColor;
    public Color goldColor;

    [Header ("Speed of slider fill")]
    public float timeForTransition;

    [Header ("Unity setup")]
    public Image imageFillerActualStar;
    public Image imageFillerNextStar;
    [Space]
    public TextMeshProUGUI textActualScore;
    [Space]
    public Slider sliderBar;
    public Image imageSliderFiller;

    private Coroutine transitionCoroutine;
    private bool lockBarRefresh;
    #endregion

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
        ColorUpdate();

        ScoreUpdate();
    }

    public void ScoreUpdate()
    {
        textActualScore.text = ScoreManager.instance.actualScore.ToString();

        SliderUpdate();
    }

    private void ColorUpdate()
    {
        switch (ScoreManager.instance.actualStar)
        {
            case ScoreManager.differentStarState.NoStar:

                imageFillerActualStar.color = noColor;
                imageFillerNextStar.color = bronzeColor;
                imageSliderFiller.color = bronzeColor;
                break;

            case ScoreManager.differentStarState.Bronze:

                imageFillerActualStar.color = bronzeColor;
                imageFillerNextStar.color = silverColor;
                imageSliderFiller.color = silverColor;
                break;

            case ScoreManager.differentStarState.Silver:

                imageFillerActualStar.color = silverColor;
                imageFillerNextStar.color = goldColor;
                imageSliderFiller.color = goldColor;
                break;

            case ScoreManager.differentStarState.Gold:

                imageFillerNextStar.color = goldColor;
                imageFillerActualStar.color = goldColor;
                imageSliderFiller.color = goldColor;
                break;
        }
    }

    private void SliderUpdate()
    {
        if (lockBarRefresh == false)
        {
            if (transitionCoroutine == null)
            {
                transitionCoroutine = StartCoroutine(SliderTransitionValue(sliderBar.value, (float)(ScoreManager.instance.actualScore - ScoreManager.instance.scoreOfActualStar) / (float)ScoreManager.instance.scoreNeedForNextStar));
            }
            else if (ScoreManager.instance.isLevelUpStar == false)
            {
                StopCoroutine(transitionCoroutine);
                transitionCoroutine = StartCoroutine(SliderTransitionValue(sliderBar.value, (float)(ScoreManager.instance.actualScore - ScoreManager.instance.scoreOfActualStar) / (float)ScoreManager.instance.scoreNeedForNextStar));
            }
            else
            {
                lockBarRefresh = true;
                StopCoroutine(transitionCoroutine);
                transitionCoroutine = StartCoroutine(SliderTransitionValue(sliderBar.value, 1f));
            }
        }
    }

    IEnumerator SliderTransitionValue(float oldValue, float newValue)
    {
        float time = 0;
        float valueForSlider = 0;

        while (time < timeForTransition)
        {
            valueForSlider = Mathf.Lerp(oldValue, newValue, time / timeForTransition);
            sliderBar.value = valueForSlider;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (ScoreManager.instance.isLevelUpStar == true)
        {
            lockBarRefresh = false;
            ScoreManager.instance.isLevelUpStar = false;

            ColorUpdate();

            if (ScoreManager.instance.actualStar != ScoreManager.differentStarState.Gold)
            {
                sliderBar.value = 0f;
                ScoreManager.instance.RefreshUiStarState();
            }
        }
    }
}
