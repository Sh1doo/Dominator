using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SimpleImageButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectColor;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = selectColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = defaultColor;
    }

}
