using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRespawnPicto : MonoBehaviour
{
    #region Variable
    [Header ("Unity setup")]
    public TextMeshProUGUI counter;
    public Image greyBackground;
    public PlayerPresets playerPreset;

    [Header("Animation")]
    public Animator selfAnimator;

    private float repawnTime;
    private float actualRepawnTime;
    private bool canStart;
    private bool endRespawn;
    #endregion

    private void Start()
    {
        repawnTime = playerPreset.respawnCooldown;
        actualRepawnTime = repawnTime;
    }

    void Update()
    {
        CounterUpdate();
    }

    private void CounterUpdate()
    {
        if (endRespawn == false)
        {
            if (actualRepawnTime > 0)
            {
                actualRepawnTime -= Time.deltaTime;
            }
            else
            {
                endRespawn = true;
                actualRepawnTime = 0f;
                selfAnimator.SetBool("End", true);
                Destroy(gameObject, 1f);
            }
            counter.text = Mathf.Round(actualRepawnTime).ToString();
            greyBackground.fillAmount = actualRepawnTime / repawnTime;
        }
    }
}
