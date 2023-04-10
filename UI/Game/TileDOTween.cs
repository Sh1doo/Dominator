using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileDOTween : MonoBehaviour
{

    private float firstPosY;

    void Start()
    {
        firstPosY = transform.localPosition.y;
    }

    public void Rotate()
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(new Vector3(360, 0, 0), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.InOutCirc));
        seq.Join(transform.DOLocalMoveY(firstPosY + 2.5f, 0.25f).SetEase(Ease.InOutCirc));
        seq.Append(transform.DOLocalMoveY(firstPosY, 0.25f).SetEase(Ease.InOutCirc));
    }

    public void MouseEnter()
    {
        transform.DOLocalMoveY(firstPosY + 1f, 0.5f).SetEase(Ease.OutExpo);

    }

    public void MouseExit()
    {
        transform.DOLocalMoveY(firstPosY, 0.5f).SetEase(Ease.OutElastic);
    }

    public void OnClick()
    {
        transform.DOLocalMoveY(transform.localPosition.y - 0.5f, 0.5f).SetEase(Ease.InFlash, 6, 1);
    }

}
