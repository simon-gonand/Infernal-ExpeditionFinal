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

        this.previousMenu = previousMenu;
        optionButton = previousButton;
    }

    public void OnChangeMusicVolume()
    {
        musicVolume = musicVolumeSlider.value;
        // Set music volume to Wwise
    }

    public void OnChangeSFXVolume()
    {
        sfxVolume = sfxVolumeSlider.value;
        // Set music volume to Wwise
    }

    public void CloseMenu()
    {
        previousMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionButton);
        gameObject.SetActive(false);
    }
}
