using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiScore : MonoBehaviour
{
    #region Variable
    public static UiScore instance;

    [Header ("Color for stars")]
    public Color noColor;
    public Color bronzeColor;
    public Color silverColor;
    public Color goldColor;

    [Header("Color for score add or remove")]
    public Color addColor;
    public Color removeColor;

    [Header("Score ui variable")]
    public float speed;
    public float timeForFadeStart;
    public float timeToWait;
    public float fadeSpeed;

    [Header ("Speed of slider fill")]
    public float timeForTransition;
    public Transform spawnPoint;

    [Header ("Unity setup")]
    public Image imageFillerActualStar;
    public Image imageFillerNextStar;
    [Space]
    public TextMeshProUGUI textActualScore;
    [Space]
    public Slider sliderBar;
    public Image imageSliderFiller;
    [Space]
    public GameObject textBox;

    [Space]
    public List<int> scoreToAddList = new List<int>();

    private Coroutine transitionCoroutine;
    private bool lockBarRefresh;

    private float actualUiScore;
    private float actualTimeToWait;
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
        textActualScore.text = actualUiScore.ToString();
        actualTimeToWait = 0f; 
        ColorUpdate();
    }

    void Update()
    {
        CheckForDisplayScoreBox();
    }


    private void CheckForDisplayScoreBox()
    {
        if (actualTimeToWait <= 0)
        {
            if (scoreToAddList.Count > 0)
            {
                StartCoroutine(WaitForUpdateUi(scoreToAddList[0]));
                scoreToAddList.Remove(scoreToAddList[0]);
                actualTimeToWait = timeToWait;
            }
        }
        else
        {
            actualTimeToWait -= Time.deltaTime;
        }
    }

    public void ScoreUpdate(int scoreAdded)
    {
        if (scoreAdded != 0)
        {
            scoreToAddList.Add(scoreAdded);
        }
    }

    private void SliderUpdate()
    {
        if (lockBarRefresh == false)
        {
            if (transitionCoroutine == null)
            {
                transitionCoroutine = StartCoroutine(SliderTransitionValue(sliderBar.value, (actualUiScore - ScoreManager.instance.scoreOfActualStar) / (float)ScoreManager.instance.scoreNeedForNextStar));
            }
            else if (ScoreManager.instance.isLevelUpStar == false && ScoreManager.instance.isDowngradeStar == false)
            {
                StopCoroutine(transitionCoroutine);
                transitionCoroutine = StartCoroutine(SliderTransitionValue(sliderBar.value, (actualUiScore - ScoreManager.instance.scoreOfActualStar) / (float)ScoreManager.instance.scoreNeedForNextStar));
            }
            else if (ScoreManager.instance.isLevelUpStar == true)
            {
                lockBarRefresh = true;
                StopCoroutine(transitionCoroutine);
                transitionCoroutine = StartCoroutine(SliderTransitionValue(sliderBar.value, 1f));
            }
            else
            {
                lockBarRefresh = true;
                StopCoroutine(transitionCoroutine);
                transitionCoroutine = StartCoroutine(SliderTransitionValue(sliderBar.value, 0f));
            }
        }
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

    private void InstantiateTextBox(int treasureValue)
    {
        GameObject obj = Instantiate(textBox, spawnPoint.position, spawnPoint.rotation);
        obj.transform.SetParent(gameObject.transform);

        TextMeshProUGUI actualText = obj.GetComponent<TextMeshProUGUI>();
        TextBoxMouvement script = obj.GetComponent<TextBoxMouvement>();

        if (treasureValue >= 0)
        {
            actualText.text = "+" + treasureValue.ToString();
            actualText.color = addColor;
        }
        else
        {
            actualText.text = treasureValue.ToString();
            actualText.color = removeColor;
        }

        script.StartLogic(speed, timeForFadeStart, fadeSpeed);
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
                ScoreManager.instance.RefreshUiStarState(0);
            }
        }
        else if (ScoreManager.instance.isDowngradeStar == true)
        {
            lockBarRefresh = false;
            ScoreManager.instance.isDowngradeStar = false;
            ColorUpdate();

            sliderBar.value = 1f;
            ScoreManager.instance.RefreshUiStarState(0);
        }
    }

    IEnumerator WaitForUpdateUi(int treasureValue)
    {
        float actualFadeDuration = fadeSpeed;
        InstantiateTextBox(treasureValue);

        yield return new WaitForSeconds(timeForFadeStart);

        while (actualFadeDuration > 0)
        {
            actualFadeDuration -= Time.deltaTime * fadeSpeed;
        }

        actualUiScore += treasureValue;

        textActualScore.text = actualUiScore.ToString();

        SliderUpdate();
    }

}
