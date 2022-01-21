using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldBagTextUi : MonoBehaviour
{
    public static GoldBagTextUi instance;

    [Header ("Color")]
    public Color addColor;
    public Color minusColor;

    [Header ("Text box parameter")]
    public float cooldown;
    public float speed;
    public float timeBeforeFade;
    public float fadeSpeed;

    [Header ("Reference")]
    public Transform spawnPoint;
    public GameObject scoreBox;


    private List<int> scoreToSpawn = new List<int>();
    private float actualCooldown;

    private void Awake()
    {
        if (instance != true)
        {
            instance = this;
        }
    }


    private void Start()
    {
        actualCooldown = 0f;
    }

    private void Update()
    {
        if (actualCooldown <= 0)
        {
            if (scoreToSpawn.Count > 0)
            {
                InstantiateScoreBox(scoreToSpawn[0]);
                scoreToSpawn.Remove(scoreToSpawn[0]);
                actualCooldown = cooldown;
            }
        }
        else
        {
            actualCooldown -= Time.fixedDeltaTime;
        }
    }

    private void InstantiateScoreBox(int treasureValue)
    {
        GameObject actualTextBox = Instantiate(scoreBox, transform.position, transform.rotation);
        actualTextBox.transform.SetParent(gameObject.transform);

        TextMeshProUGUI actualText = actualTextBox.GetComponent<TextMeshProUGUI>();
        TextBoxMouvement scrpit = actualTextBox.GetComponent<TextBoxMouvement>();

        if (treasureValue >= 0)
        {
            actualText.text = "+" + treasureValue.ToString();
            actualText.color = addColor;
        }
        else
        {
            actualText.text = treasureValue.ToString();
            actualText.color = minusColor;
        }

        scrpit.StartLogic(speed, timeBeforeFade, fadeSpeed);
    }

    public void AddScoreToSpawn(int treasureValue)
    {
        scoreToSpawn.Add(treasureValue);
    }
}


