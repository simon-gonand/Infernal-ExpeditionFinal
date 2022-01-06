using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRespawnPicto : MonoBehaviour
{
    public TextMeshProUGUI counter;
    public Image greyBackground;
    public PlayerPresets playerPreset;

    private float repawnTime;
    private float actualRepawnTime;
    private bool canStart;

    private void Start()
    {
        repawnTime = playerPreset.respawnCooldown;
        actualRepawnTime = repawnTime;
    }

    void Update()
    {
        if (canStart)
        {
            if (actualRepawnTime > 0)
            {
                actualRepawnTime -= Time.deltaTime;
            }
            else
            {
                actualRepawnTime = 0f;
            }

            counter.text = Mathf.Round(actualRepawnTime).ToString();
            greyBackground.fillAmount = actualRepawnTime / repawnTime;
        }
    }


}
