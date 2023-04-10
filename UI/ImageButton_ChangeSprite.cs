using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ImageButton_ChangeSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] private Color defaultColor;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Color selectColor;
    [SerializeField] private Sprite selectSprite;
    [SerializeField] private UnityEvent clickAction;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();

        image.sprite = defaultSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        image.sprite = selectSprite;
        var seq = DOTween.Sequence();
        seq.AppendInterval(0.5f)
            .OnComplete(() => {
                image.sprite = defaultSprite;
            });
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
