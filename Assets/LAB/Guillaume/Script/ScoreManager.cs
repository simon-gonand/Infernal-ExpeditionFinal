using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Actual global score")]
    public int actualScore;

    [Header ("Setup score")]
    [Range(1, 100)] public int scoreNeedForBronze;
    [Range(1, 100)] public int scoreNeedForSilver;
    [Range(1, 100)] public int scoreNeedForGold;
    [Space]
    public differentStarState actualStar;

    public int scoreNeedForNextStar;
    public int scoreOfActualStar;

    public enum differentStarState {NoStar, Bronze, Silver, Gold}

    private void Awake()
    {
        #region Setup instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("There is multiple ScoreManager in the scene");
        }
        #endregion

        scoreNeedForNextStar = scoreNeedForBronze;
        scoreOfActualStar = 0;
    }

    private void Start()
    {
        //nextObjectif = scoreNeedForBronze;
    }

    public void AddScore(int numberToAdd)
    {
        actualScore += numberToAdd;

        CheckStar();

        if (UiScore.instance != null)
        {
            UiScore.instance.ScoreUpdate();
        }

    }

    public void SubstractScore(int numberToSubstract)
    {
        actualScore -= numberToSubstract;

        CheckStar();

        if (UiScore.instance != null)
        {
            UiScore.instance.ScoreUpdate();
        }
    }


    private void CheckStar()
    {
        switch (actualStar)
        {
            case differentStarState.NoStar:
                if (actualScore >= scoreNeedForBronze)
                {
                    actualStar = differentStarState.Bronze;

                    scoreOfActualStar = scoreNeedForBronze;

                    scoreNeedForNextStar = scoreNeedForSilver - scoreNeedForBronze;
                }
                break;

            case differentStarState.Bronze:
                if (actualScore >= scoreNeedForSilver)
                {
                    actualStar = differentStarState.Silver;

                    scoreOfActualStar = scoreNeedForSilver;

                    scoreNeedForNextStar = scoreNeedForGold - scoreNeedForSilver;
                }
                else if (actualScore < scoreNeedForBronze)
                {
                    actualStar = differentStarState.NoStar;
                }
                break;

            case differentStarState.Silver:
                if (actualScore >= scoreNeedForGold)
                {
                    actualStar = differentStarState.Gold;

                    //scoreOfActualStar = scoreNeedForGold;
                }
                else if (actualScore < scoreNeedForSilver)
                {
                    actualStar = differentStarState.Bronze;
                }
                break;

            case differentStarState.Gold:
                if (actualScore < scoreNeedForBronze)
                {
                    actualStar = differentStarState.Silver;
                }
                break;
        }
    }
}

