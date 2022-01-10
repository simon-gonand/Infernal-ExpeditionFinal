using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBoxMouvement : MonoBehaviour
{
    [Header ("Unity setup")]
    public TextMeshProUGUI text;

    private bool startLogic;

    private float speed;
    private float timeForFadeStart;
    private float fadeSpeed;
    private float time;

    void FixedUpdate()
    {
        if (startLogic)
        {
            gameObject.transform.Translate(Vector3.up * speed);

            if (timeForFadeStart > 0)
            {
                timeForFadeStart -= Time.deltaTime;
            }
            else
            {
                text.color = Color.Lerp(text.color, new Color(0,0,0,0), fadeSpeed * Time.fixedDeltaTime);
                time -= Time.fixedDeltaTime * fadeSpeed;
            }

            if (time < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void StartLogic(float _speed, float _timeForFadeStart, float _fadeSpeed)
    {
        speed = _speed;
        timeForFadeStart = _timeForFadeStart;
        fadeSpeed = _fadeSpeed;
        time = _fadeSpeed;

        startLogic = true;
    } 
}
