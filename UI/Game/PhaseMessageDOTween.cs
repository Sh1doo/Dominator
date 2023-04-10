using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class PhaseMessageDOTween : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI phaseMessageText;

    [SerializeField] RectTransform rectTransform;

    private string phaseMessage;

    //入力フェーズのお知らせ表示アニメーション
    public void DoAnimation_InputPhaseNotice(UnityAction callback)
    {
        var sequence = DOTween.Sequence();

        //ターン表示
        phaseMessageText.text = "ターン" + GameData.Turn.ToString();
        sequence.Append(rectTransform.DOPivotY(1.25f, 1f).SetEase(Ease.OutBack));
        sequence.AppendInterval(2f);
        sequence.Append(rectTransform.DOPivotY(-0.5f, 0.5f).SetEase(Ease.InBack));

        //フェーズ表示
        sequence.AppendCallback(() => { phaseMessageText.text = "入力フェーズ開始"; });
        sequence.Append(rectTransform.DOPivotY(1.25f, 1f).SetEase(Ease.OutBack));
        sequence.AppendInterval(2f);
        sequence.Append(rectTransform.DOPivotY(-0.5f, 0.5f).SetEase(Ease.InBack));

        //コールバック
        sequence.AppendCallback(() => {
            callback.Invoke();
            sequence.Kill();
            Destroy(gameObject);
        });
    }

    //ターンプレイヤーのお知らせ表示アニメーション
    public void DoAnimation_TurnPlayerNotice(UnityAction callback, string turnPlayerName)
    {
        var sequence = DOTween.Sequence();

        //ターン表示
        phaseMessageText.text = string.Format("{0}のターン", turnPlayerName);
        sequence.Append(rectTransform.DOPivotY(1.25f, 1f).SetEase(Ease.OutBack));
        sequence.AppendInterval(2f);
        sequence.Append(rectTransform.DOPivotY(-0.5f, 0.5f).SetEase(Ease.InBack));

        //コールバック
        sequence.AppendCallback(() => {
            callback.Invoke();
            sequence.Kill();
            Destroy(gameObject);
        });
    }

    //集計フェーズのお知らせ表示アニメーション
    public void DoAnimation_CountPhaseNotice(UnityAction callback)
    {
        var sequence = DOTween.Sequence();

        //ターン表示
        phaseMessageText.text = "集計フェーズ開始";
        sequence.Append(rectTransform.DOPivotY(1.25f, 1f).SetEase(Ease.OutBack));
        sequence.AppendInterval(2f);
        sequence.Append(rectTransform.DOPivotY(-0.5f, 0.5f).SetEase(Ease.InBack));

        //コールバック
        sequence.AppendCallback(() => {
            callback.Invoke();
            sequence.Kill();
            Destroy(gameObject);
        });
    }

    //スキルフェーズのお知らせ表示アニメーション
    public void DoAnimation_SkillPhaseNotice(UnityAction callback)
    {
        var sequence = DOTween.Sequence();

        //ターン表示
        phaseMessageText.text = "スキルフェーズ開始";
        sequence.Append(rectTransform.DOPivotY(1.25f, 1f).SetEase(Ease.OutBack));
        sequence.AppendInterval(2f);
        sequence.Append(rectTransform.DOPivotY(-0.5f, 0.5f).SetEase(Ease.InBack));

        //コールバック
        sequence.AppendCallback(() => {
            callback.Invoke();
            sequence.Kill();
            Destroy(gameObject);
        });
    }

    public void PlayInputPhase(UnityAction turnEnd, UnityAction callback)
    {
        //表示
        gameObject.SetActive(true);

        var sequence = DOTween.Sequence();
        //ターン表示
        phaseMessageText.text = "ターン" + GameData.Turn.ToString();
        sequence.Append(rectTransform.DOPivotY(1.25f, 1f).SetEase(Ease.OutBack));
        sequence.AppendInterval(2f);
        sequence.Append(rectTransform.DOPivotY(-0.5f, 0.5f).SetEase(Ease.InBack))
            .OnComplete(() => turnEnd.Invoke());

        //フェーズ表示
        sequence.AppendCallback(() => { phaseMessageText.text = "入力フェーズ開始"; });
        sequence.Append(rectTransform.DOPivotY(1.25f, 1f).SetEase(Ease.OutBack));
        sequence.AppendInterval(2f);
        sequence.Append(rectTransform.DOPivotY(-0.5f, 0.5f).SetEase(Ease.InBack));

        sequence.AppendCallback(() => {
            //コールバック
            callback.Invoke();

            //非表示
            gameObject.SetActive(false);
        });
    }

    public void ShowPlayerTurn(UnityAction callback, int playerId)
    {
        //表示
        gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        //フェーズ表示
        phaseMessageText.text = playerId.ToString();
        sequence.Append(rectTransform.DOPivotY(1.25f, 1f).SetEase(Ease.OutBack));
        sequence.AppendInterval(2f);
        sequence.Append(rectTransform.DOPivotY(-0.5f, 0.5f).SetEase(Ease.InBack));

        sequence.AppendCallback(() => {
            //コールバック
            callback.Invoke();

            //非表示
            gameObject.SetActive(false);
        });
    }

    public void PlayCountPhase(UnityAction callback)
    {
        //表示
        gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        //フェーズ表示
        phaseMessageText.text = "集計フェーズ開始";
        sequence.Append(rectTransform.DOPivotY(1.25f, 1f).SetEase(Ease.OutBack));
        sequence.AppendInterval(2f);
        sequence.Append(rectTransform.DOPivotY(-0.5f, 0.5f).SetEase(Ease.InBack));

        sequence.AppendCallback(() => {
            //コールバック
            callback.Invoke();

            //非表示
            gameObject.SetActive(false);
        });
    }

    public void PlaySkillPhase(UnityAction callback)
    {
        //表示
        gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        //フェーズ表示
        phaseMessageText.text = "スキルフェーズ開始";
        sequence.Append(rectTransform.DOPivotY(1.25f, 1f).SetEase(Ease.OutBack));
        sequence.AppendInterval(2f);
        sequence.Append(rectTransform.DOPivotY(-0.5f, 0.5f).SetEase(Ease.InBack));

        sequence.AppendCallback(() => {
            //コールバック
            callback.Invoke();

            //非表示
            gameObject.SetActive(false);
        });
    }

}
