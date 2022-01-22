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

    private bool headphones = false;
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

        headphones = AudioManager.AMInstance.headphones;
        headphonesToggle.isOn = headphones;

        this.previousMenu = previousMenu;
        optionButton = previousButton;
    }

    public void OnToggleChange()
    {
        headphones = headphonesToggle.isOn;
        AudioManager.AMInstance.headphones = true;
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
