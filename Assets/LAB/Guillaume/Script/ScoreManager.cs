using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Variable
    public static ScoreManager instance;

    [Header ("Setup score")]
    public int scoreNeedForBronze;
    public int scoreNeedForSilver;
    public int scoreNeedForGold;

    public enum differentStarState {NoStar, Bronze, Silver, Gold}
    [HideInInspector]public differentStarState actualStar;

    [HideInInspector]public int actualScore;
    [HideInInspector]public int scoreNeedForNextStar;
    [HideInInspector]public int scoreOfActualStar;

    [HideInInspector]public int maxScore;

    [HideInInspector]public bool isLevelUpStar;
    [HideInInspector]public bool isDowngradeStar;

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
            Debug.LogWarning("There is multiple ScoreManager in the scene");
        }
        #endregion

        GameManager.instance.GetStarsValue(PlayerManager.instance.players.Count, 
            scoreNeedForBronze, scoreNeedForSilver, scoreNeedForGold);

        scoreNeedForNextStar = scoreNeedForBronze;
        scoreOfActualStar = 0;
    }

    void Start()
    {
        RefreshUiStarState(0);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void AddScore(int numberToAdd)
    {
        actualScore += numberToAdd;

        RefreshUiStarState(numberToAdd);
    }

    public void RemoveScore(int numberToRemove)
    {
        actualScore -= numberToRemove;

        RefreshUiStarState(-numberToRemove);
    }

    public void RefreshUiStarState(int scoreAdded)
    {
        CheckStar();

        if (UiScore.instance != null)
        {
            UiScore.instance.ScoreUpdate(scoreAdded);
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

                    isLevelUpStar = true;
                }
                break;

            case differentStarState.Bronze:
                if (actualScore >= scoreNeedForSilver)
                {
                    actualStar = differentStarState.Silver;
                    scoreOfActualStar = scoreNeedForSilver;
                    scoreNeedForNextStar = scoreNeedForGold - scoreNeedForSilver;

                    isLevelUpStar = true;
                }
                else if (actualScore < scoreNeedForBronze)
                {
                    actualStar = differentStarState.NoStar;
                    scoreOfActualStar = 0;
                    scoreNeedForNextStar = scoreNeedForBronze;

                    isDowngradeStar = true;
                }
                break;

            case differentStarState.Silver:
                if (actualScore >= scoreNeedForGold)
                {
                    actualStar = differentStarState.Gold;
                    isLevelUpStar = true;
                }
                else if (actualScore < scoreNeedForSilver)
                {
                    actualStar = differentStarState.Bronze;
                    scoreOfActualStar = scoreNeedForBronze;
                    scoreNeedForNextStar = scoreNeedForSilver - scoreNeedForBronze;

                    isDowngradeStar = true;
                }
                break;

            case differentStarState.Gold:
                if (actualScore < scoreNeedForGold)
                {
                    actualStar = differentStarState.Silver;
                    scoreOfActualStar = scoreNeedForSilver;
                    scoreNeedForNextStar = scoreNeedForGold - scoreNeedForSilver;

                    isDowngradeStar = true;
                }
                break;
        }
    }
}

