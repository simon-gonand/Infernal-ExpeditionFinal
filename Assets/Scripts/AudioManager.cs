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

    public bool headphones = false;

    [Header("Switchs")]

    public AK.Wwise.Event gameplayNavigationSWITCH;
    public AK.Wwise.Event gameplayPillageSWITCH;
    public AK.Wwise.Event mapCompletedSWITCH;
    public AK.Wwise.Event lobbyThemeToClassicSWITCH;
    public AK.Wwise.Event lobbyThemeToCitySWITCH;
    public AK.Wwise.Event lobbyThemeToBaySWITCH;
    public AK.Wwise.Event audioDeviceToSpeakersSWITCH;
    public AK.Wwise.Event audioDeviceToHeadphonesSWITCH;

    [Header("RTPC")]
    public AK.Wwise.RTPC SFXVolumeRTPC;
    public AK.Wwise.RTPC musicVolumeRTPC;

    [Header("Themes")]

    public AK.Wwise.Event runTheme;
    public AK.Wwise.Event lobbiTheme;
    public AK.Wwise.Event run03Theme;

    [Header("Player")]

    public AK.Wwise.Event playerAttackSFX;
    public AK.Wwise.Event playerCarrySFX;
    public AK.Wwise.Event playerDashSFX;
    public AK.Wwise.Event playerGroundImpactSFX;
    public AK.Wwise.Event playerStepsSFX;
    public AK.Wwise.Event playerStuntSFX;
    public AK.Wwise.Event playerSwimSFX;
    public AK.Wwise.Event playerThrowSFX;
    public AK.Wwise.Event playerRespawnSFX;

    [Header("Enemy")]

    public AK.Wwise.Event clsqEnemyAttackSFX;
    public AK.Wwise.Event trlEnemyShotSFX;
    public AK.Wwise.Event trlEnemyReloadSFX;
    public AK.Wwise.Event trlEnemyDeathSFX;
    public AK.Wwise.Event pskEnemyCarrySFX;

    [Header("Boat")]

    public AK.Wwise.Event boatTreasureCollectSFX;
    public AK.Wwise.Event boatDamagesSFX;
    public AK.Wwise.Event boatMovingSFX;

    [Header("World")]
    public AK.Wwise.Event chestGroundImpactSFX;
    public AK.Wwise.Event gameAmbientSFX;
    public AK.Wwise.Event mapOpeningSFX;
    public AK.Wwise.Event ropeCutSFX;
    public AK.Wwise.Event doorImpactSFX;
    public AK.Wwise.Event sharkBiteSFX;

    [Header("UI")]
    public AK.Wwise.Event menuCancelSFX;
    public AK.Wwise.Event menuNavigationSFX;
    public AK.Wwise.Event menuSelectSFX;


    private bool typeIsPlunder = false;
    [HideInInspector]
    public List<PlayerController> playersOnBoat = new List<PlayerController>(0);

    private void Start()
    {
        if (headphones == false)
        {
            audioDeviceToSpeakersSWITCH.Post(gameObject);
        }
        else
        {
            audioDeviceToHeadphonesSWITCH.Post(gameObject);
        }
    }

    private void Update()
    {


        if(playersOnBoat.Count == PlayerManager.instance.players.Count)
        {
            if(typeIsPlunder == true)
            {
                gameplayNavigationSWITCH.Post(gameObject);
                typeIsPlunder = false;
            }
        }
        else
        {
            if (typeIsPlunder == false)
            {
                gameplayPillageSWITCH.Post(gameObject);
                typeIsPlunder = true;
            }
        }
    }
}
