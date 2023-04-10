using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIAnimationCanvasManager : MonoBehaviour
{

    //管理対象のCanvas
    [SerializeField] private RectTransform UIAnimationCanvas;

    //アニメーションPrefab
    [SerializeField] private GameObject[] ObjectPrefabs;

    enum ObjectID
    {
        phaseMessagePrefab,
    }

    //入力フェーズのお知らせ表示
    public void DoAnimation_InputPhaseNotice(UnityAction callback)
    {
        GameObject createdObject = CreateObject(ObjectPrefabs[(int)ObjectID.phaseMessagePrefab]);
        var dotweenComponent = createdObject.GetComponent<PhaseMessageDOTween>();

        dotweenComponent.DoAnimation_InputPhaseNotice(callback);
    }

    //ターンプレイヤーのお知らせ表示
    public void DoAnimation_TurnPlayerNotice(UnityAction callback, string turnPlayerName)
    {
        GameObject createdObject = CreateObject(ObjectPrefabs[(int)ObjectID.phaseMessagePrefab]);
        var dotweenComponent = createdObject.GetComponent<PhaseMessageDOTween>();

        dotweenComponent.DoAnimation_TurnPlayerNotice(callback, turnPlayerName);
    }

    //集計フェーズのお知らせ表示
    public void DoAnimation_CountPhaseNotice(UnityAction callback)
    {
        GameObject createdObject = CreateObject(ObjectPrefabs[(int)ObjectID.phaseMessagePrefab]);
        var dotweenComponent = createdObject.GetComponent<PhaseMessageDOTween>();

        dotweenComponent.DoAnimation_CountPhaseNotice(callback);
    }

    //スキルフェーズのお知らせ表示
    public void DoAnimation_SkillPhaseNotice(UnityAction callback)
    {
        GameObject createdObject = CreateObject(ObjectPrefabs[(int)ObjectID.phaseMessagePrefab]);
        var dotweenComponent = createdObject.GetComponent<PhaseMessageDOTween>();

        dotweenComponent.DoAnimation_SkillPhaseNotice(callback);
    }

    //Prefabのインスタンスを作成
    private GameObject CreateObject(GameObject prefab)
    {
        GameObject createdObject = Instantiate(prefab, UIAnimationCanvas);
        return createdObject;
    }

}
