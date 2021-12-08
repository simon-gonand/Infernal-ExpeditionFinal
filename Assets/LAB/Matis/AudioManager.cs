using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Instance
    public static AudioManager AMInstance;

    private void Awake()
    {
        if (AMInstance == null)
        {
            AMInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion



    [Header("Themes")]

    public AK.Wwise.Event runTheme;

    [Header("Player")]

    public AK.Wwise.Event playerAttackSFX;
    public AK.Wwise.Event playerCarrySFX;
    public AK.Wwise.Event playerDashSFX;
    public AK.Wwise.Event playerGroundImpactSFX;
    public AK.Wwise.Event playerStepsSFX;
    public AK.Wwise.Event PlayerStuntSFX;

    [Header("Enemy")]

    public AK.Wwise.Event clsqEnemyAttackSFX;

    [Header("Boat")]

    public AK.Wwise.Event boatTreasureCollectSFX;


    void Start()
    {
        //DEBUG
        runTheme.Post(gameObject);
        //
    }
}