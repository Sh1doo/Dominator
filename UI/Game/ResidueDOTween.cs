using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResidueDOTween : MonoBehaviour
{

    [SerializeField] Image image;
    [SerializeField] RectTransform rect;

    public void PlayInputPhase()
    {
        rect.gameObject.SetActive(true);

        rect.DOPivotY(-0.5f, 0.5f).SetEase(Ease.OutBack);
    }

    public void EndInputPhase()
    {
        rect.DOPivotY(1.5f, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => rect.gameObject.SetActive(false));
    }

    public void SetColor()
    {
        image.color = TileColor.getColor(GameData.UserId);
    }
}
