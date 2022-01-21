using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModifierLogic : MonoBehaviour
{
    private float timeBeforeMove;
    private float duration;
    private float actualDuration;

    public Image blackCircle;
    public TextMeshProUGUI counter;

    public Animator selfAnimator;

    public void SetValue(float _timeBeforeMove , float _duration)
    {
        timeBeforeMove = _timeBeforeMove;
        duration = _duration;
        actualDuration = duration;

        StartCoroutine(Logic());
    }

    IEnumerator Logic()
    {
        yield return new WaitForSeconds(timeBeforeMove);
        selfAnimator.SetBool("Move", true);

        while (actualDuration > 0)
        {
            actualDuration -= Time.deltaTime;
            counter.text = Mathf.Round(actualDuration).ToString();
            blackCircle.fillAmount = actualDuration / duration;

            yield return new WaitForEndOfFrame();
        }

        counter.text = 0.ToString();
        blackCircle.fillAmount = 0f;

        selfAnimator.SetBool("End", true);

        Destroy(gameObject, 2f);
    }
}
