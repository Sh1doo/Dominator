using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutInManager : MonoBehaviour
{
    //CutIn再生場所のCanvas
    [SerializeField] private RectTransform UIAnimationCanvas;

    //CutInオブジェクト
    [SerializeField] private List<CutInObject> cutInObjects;

    //CutInアニメーションPrefab
    [SerializeField] private GameObject[] cutInPrefabs;

    private UnityAction callback;
    private int executedSkillIndex = -1;

    //すべてのフェーズ終了時に特定のカットインの再生命令を呼ぶ
    public void PlayAllCutIn(UnityAction callback)
    {
        this.callback = callback;
        executedSkillIndex = -1;

        CheckTermsAndPlay();
    }

    //スキルを順番に確認していく
    private void CheckTermsAndPlay()
    {
        executedSkillIndex += 1;

        if (executedSkillIndex >= cutInObjects.Count)
        {
            this.callback.Invoke();
            return;
        }

        cutInObjects[executedSkillIndex].CheckTermsAndPlay(CheckTermsAndPlay);
    }

    //実際のカットインアニメーションを再生する
    public void PlayCutIn(CutIn.Type type, UnityAction callback)
    {

        GameObject createdObject;
        CutIn dotweenComponent;

        switch(type){
            case CutIn.Type.SpecialTile:
                createdObject = CreateObject(cutInPrefabs[(int)type]);
                dotweenComponent = createdObject.GetComponent<TileWindmillCutIn>();
                dotweenComponent.PlayCutIn(callback);
                break;

            case CutIn.Type.Skill00:
                createdObject = CreateObject(cutInPrefabs[(int)type]);
                dotweenComponent = createdObject.GetComponent<Skill00_CutIn>();
                dotweenComponent.PlayCutIn(callback);
                break;
        }
    }

    //Prefabのインスタンスを作成
    private GameObject CreateObject(GameObject prefab)
    {
        GameObject createdObject = Instantiate(prefab, UIAnimationCanvas);
        return createdObject;
    }

}
