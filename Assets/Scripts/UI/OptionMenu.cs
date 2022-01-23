using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionMenu : MonoBehaviour
{
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider sfxVolumeSlider;
    [SerializeField]
    private Toggle headphonesToggle;

    private GameObject previousMenu;
    private GameObject optionButton;

    private float musicVolume = 50.0f;
    private float sfxVolume = 50.0f;

    // Start is called before the first frame update
    public void OpenMenu(GameObject previousMenu, GameObject previousButton)
    {
        gameObject.SetActive(true);
        // Get musicVolume
        musicVolumeSlider.value = musicVolume;
        // Get sfxVolume
        sfxVolumeSlider.value = sfxVolume;

        headphonesToggle.isOn = AudioManager.AMInstance.headphones;

        this.previousMenu = previousMenu;
        optionButton = previousButton;

        headphonesToggle.Select();
    }

    public void OnToggleChange()
    {
        if (headphonesToggle.isOn)
        {
            Debug.Log("true");
            AudioManager.AMInstance.headphones = true;
            AudioManager.AMInstance.audioDeviceToHeadphonesSWITCH.Post(gameObject);
        }
        else
        {
            Debug.Log("false");
            AudioManager.AMInstance.headphones = false;
            AudioManager.AMInstance.audioDeviceToSpeakersSWITCH.Post(gameObject);
        }
    }

    public void OnChangeMusicVolume()
    {
        musicVolume = musicVolumeSlider.value;
        AudioManager.AMInstance.musicVolumeRTPC.SetGlobalValue(musicVolume);
    }

    public void OnChangeSFXVolume()
    {
        sfxVolume = sfxVolumeSlider.value;
        AudioManager.AMInstance.SFXVolumeRTPC.SetGlobalValue(sfxVolume);
    }

    public void CloseMenu()
    {
        previousMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionButton);
        gameObject.SetActive(false);
    }
}
