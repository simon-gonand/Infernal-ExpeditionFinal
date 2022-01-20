using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [Header ("Color")]
    public Color bronzeColor;
    public Color silverColor;
    public Color goldColor;


    [Header ("Self reference")]
    public Slider selfSlider;
    public RectTransform handle;
    [Space]
    public Transform bronzeMedalGlobal;
    public Transform silverMedalGlobal;
    public Transform goldMedalGlobal;


    void Start()
    {
        selfSlider.maxValue = 100000;

        SetUpMedalsPos();
    }
    
    public void SetUpMedalsPos()
    {
        selfSlider.value = ScoreManager.instance.scoreNeedForBronze;
        bronzeMedalGlobal.position = handle.position;

        selfSlider.value = ScoreManager.instance.scoreNeedForSilver;
        silverMedalGlobal.position = handle.position;

        selfSlider.value = ScoreManager.instance.scoreNeedForGold;
        goldMedalGlobal.position = handle.position;

        selfSlider.value = 0;
    }
}
