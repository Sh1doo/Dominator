using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SimpleImageButton_NoColor : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private UnityEvent clickAction;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickAction.Invoke();
    }

}
