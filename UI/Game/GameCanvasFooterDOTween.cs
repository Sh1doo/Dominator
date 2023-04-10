using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameCanvasFooterDOTween : MonoBehaviour
{

    //Tweenするオブジェクト
    [SerializeField] private TextMeshProUGUI player;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private RawImage rankIcon;

    [SerializeField] private RectTransform skillButton;
    [SerializeField] private RectTransform Wrench;
    [SerializeField] private RectTransform Timer;
    [SerializeField] private RectTransform Residue;

    void Start()
    {
        //初期化
        skillButton.DOLocalMoveY(-50f, 0f);
        Timer.DOLocalMoveY(-42.5f, 0f);
        Wrench.DOLocalMoveY(-42.5f, 0f);
        Residue.DOLocalMoveY(-47.5f, 0f);
    }

    //初期位置へのリセットアニメーション
    public void DoAnimation_GameStartInit()
    {
        var sequence = DOTween.Sequence();

        //テキスト表示
        StartCoroutine(nameof(TextFlow));

        //スキルボタン
        sequence.Append(skillButton.DOLocalMoveY(50, 0.5f).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.2f);

        //タイマー
        sequence.Append(Timer.DOLocalMoveY(57.5f, 0.25f).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.1f);

        //レンチ
        sequence.Append(Wrench.DOLocalMoveY(57.5f, 0.25f).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.1f);

        //残り
        sequence.Append(Residue.DOLocalMoveY(52.5f, 0.25f).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.1f);

        //コールバック
        sequence.AppendCallback(() => {
            sequence.Kill();
        });
    }

    //文字を一文字ずつ表示する
    private IEnumerator TextFlow()
    {
        //プレイヤー名の表示
        string textScript = "";
        string completeString = GameData.UserData[GameData.UserId]["PlayerName"].ToString();
        for (int i = 0; i < completeString.Length; ++i)
        {
            textScript += completeString[i];
            player.text = textScript;

            yield return new WaitForSeconds(0.1f);
        }

        //ランクアイコンの表示
        rankIcon.DOColor(Color.white, 0.5f);

        //ランクの表示
        textScript = "";
        completeString = "Platinum IV";
        for (int i = 0; i < completeString.Length; ++i)
        {
            textScript += completeString[i];
            rank.text = textScript;

            yield return new WaitForSeconds(0.1f);
        }
    }

    //指定したnameとrankで表示する
    public IEnumerator TextFlowNameAndRank(string name, string rank, UnityAction callback)
    {
        //プレイヤー名の表示
        string textScript = "";
        string completeString = name;
        for (int i = 0; i < completeString.Length; ++i)
        {
            textScript += completeString[i];
            player.text = textScript;

            yield return new WaitForSeconds(0.1f);
        }

        //ランクアイコンの表示
        rankIcon.DOColor(Color.white, 0.5f);

        //ランクの表示
        textScript = "";
        completeString = rank;
        for (int i = 0; i < completeString.Length; ++i)
        {
            textScript += completeString[i];
            this.rank.text = textScript;

            yield return new WaitForSeconds(0.1f);
        }

        callback.Invoke();
    }

}
