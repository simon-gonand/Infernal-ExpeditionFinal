using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private Image image;

    public void SetSelectAtStart()
    {
        Color color = image.color;
        color.a = 255.0f;
        image.color = color;
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        Color color = image.color;
        color.a = 255.0f;
        image.color = color;
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        Color color = image.color;
        color.a = 0.0f;
        image.color = color;
    }
}
