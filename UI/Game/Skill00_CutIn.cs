using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Skill00_CutIn : CutIn
{
    [SerializeField] private RectTransform imageRect;
    [SerializeField] private RectTransform textRect;

    public override void PlayCutIn(UnityAction callback)
    {
        //初期位置に戻す
        imageRect.anchorMax = new Vector2(1, 0.5f);
        imageRect.anchorMin = new Vector2(2, 0.5f);
        textRect.anchorMax = new Vector2(2, 0.5f);
        textRect.anchorMin = new Vector2(1, 0.5f);

        //カットイン
        var seq = DOTween.Sequence();
        seq.Join(imageRect.DOAnchorMax(new Vector2(1f, 0.5f), 0.5f).SetEase(Ease.OutExpo));
        seq.Join(imageRect.DOAnchorMin(new Vector2(0f, 0.5f), 0.5f).SetEase(Ease.OutExpo));
        seq.Join(textRect.DOAnchorMax(new Vector2(1f, 0.5f), 1f).SetEase(Ease.OutExpo));
        seq.Join(textRect.DOAnchorMin(new Vector2(0f, 0.5f), 1f).SetEase(Ease.OutExpo));
        seq.AppendInterval(2f);
        seq.Append(imageRect.DOAnchorMax(new Vector2(0f, 0.5f), 0.5f).SetEase(Ease.OutExpo));
        seq.Join(imageRect.DOAnchorMin(new Vector2(-1f, 0.5f), 0.5f).SetEase(Ease.OutExpo));
        seq.Join(textRect.DOAnchorMax(new Vector2(0f, 0.5f), 1f).SetEase(Ease.OutExpo));
        seq.Join(textRect.DOAnchorMin(new Vector2(0f, 0.5f), 1f).SetEase(Ease.OutExpo));

        //コールバック
        seq.AppendCallback(() => {
            callback.Invoke();
            seq.Kill();
            Destroy(gameObject);
        });

    }
}
