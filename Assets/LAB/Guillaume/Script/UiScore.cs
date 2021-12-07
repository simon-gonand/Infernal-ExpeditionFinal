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
    public Image imageFillerActualStar;
    public Image imageFillerNextStar;
    [Space]
    public TextMeshProUGUI textActualScore;
    [Space]
    public Slider sliderBar;
    public Image imageSliderFiller;

    private Coroutine transitionCoroutine;
    public float oldScore;
    public float timeForTransition;

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

        SliderUpdate();

        ColorUpdate();
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

                imageFillerActualStar.color = goldColor;
                break;
        }
    }

    private void SliderUpdate()
    {
        if (transitionCoroutine == null)
        {
            transitionCoroutine = StartCoroutine(SliderTransitionValue(sliderBar.value, (float)(ScoreManager.instance.actualScore - ScoreManager.instance.scoreOfActualStar) / (float)ScoreManager.instance.scoreNeedForNextStar));
        }
        else
        {
            StopCoroutine(transitionCoroutine);
            transitionCoroutine = StartCoroutine(SliderTransitionValue(sliderBar.value, (float)(ScoreManager.instance.actualScore - ScoreManager.instance.scoreOfActualStar) / (float)ScoreManager.instance.scoreNeedForNextStar));
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
    }
}
