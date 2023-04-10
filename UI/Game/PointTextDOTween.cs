using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PointTextDOTween : MonoBehaviour
{

    [SerializeField] private RectTransform rect;

    void Start()
    {
        var seq = DOTween.Sequence();

        seq.Append(rect.DOLocalMoveY(rect.localPosition.y + 20f, 2f))
            .OnComplete(() => Destroy(gameObject));
        seq.Join(rect.DOScale(new Vector3(0.75f, 0.75f, 0.75f), 1f));
    }
}
