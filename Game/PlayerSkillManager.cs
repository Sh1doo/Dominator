using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerSkillManager : MonoBehaviour
{

    //Skillの派生クラス(Playerがローカルでスキルを利用するためのスクリプト)
    [SerializeField] private Skill[] Skills;

    //Remoteでスキルを実行するためにデータなどを保持したクラス
    private List<SkillObject> skillObjects = new List<SkillObject>();
    [SerializeField] Transform skillQueue;
    [SerializeField] GameObject skillObjectPrefabs;

    //リファレンス
    [SerializeField] PlayerSkillManagerReferense r;

    private PhotonView photonView;

    //スキルカットイン実行用
    private UnityAction callback;
    private int executedSkillIndex = -1;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        
        //SKillCTの設定
        GameData.SkillCT = r.characterSkillDataManager.GetCharacterSkillData(GameData.SkillId).skillCT;
        r.gameCanvasManager.SetTimer(GameData.SkillCT);
    }

    //スキルの使用
    public void UseSkill(int skillId)
    {
        //CT条件を満たしているか
        if (!CheckSkillCT(skillId)) return;

        //CT以外のスキル発動条件
        switch (skillId)
        {
            case 0:
                if (GameData.Mine == 0) return;
                break;
        }

        //スキルの発動
        switch (skillId)
        {
            default:
                Skills[skillId].DoSkill();

                GameData.SkillCT = r.characterSkillDataManager.GetCharacterSkillData(GameData.SkillId).skillCT;
                r.gameCanvasManager.SetTimer(GameData.SkillCT);

                break;
        }
    }

    //スキルCTのチェック
    private bool CheckSkillCT(int skillId) 
    {
        switch (skillId)
        {
            //地雷設置(CTなし)
            case 0: return true;

            default:
                if (GameData.SkillCT == 0) return true;
                else return false;
        }
    }

    //InputPhase開始時
    public void InputPhaseStart(int skillId)
    {
        GameData.SkillCT = Mathf.Max(GameData.SkillCT - 1, 0);
        r.gameCanvasManager.SetTimer(GameData.SkillCT);

        switch (skillId)
        {
            case 0:
                GameData.Mine += 1;
                r.gameCanvasManager.SetWrench(GameData.Mine);
                break;

            default:
                Skills[skillId].DoSkill();
                break;
        }
    }

    //スキルの使用フラグを立てる
    [PunRPC]
    private void RPC_CheckSkillCutIn_00(int invoker, int target)
    {
        GameObject createdObject = Instantiate(skillObjectPrefabs, skillQueue);
        SkillObject_00 newSkillObject = createdObject.AddComponent<SkillObject_00>();
        skillObjects.Add(newSkillObject);

        //スキルのデータセット
        newSkillObject.setAll(invoker, target, r.cutInManager, r.tileManager);
    }
    public void CheckSkillCutIn_00(int invoker, int target)
    {
        photonView.RPC(nameof(RPC_CheckSkillCutIn_00), RpcTarget.All, invoker, target);
    }

    //スキルの実行
    public void PlayAllSkillCutIn(UnityAction callback)
    {
        this.callback = callback;
        executedSkillIndex = -1;

        CheckTermsAndPlay();
    }

    //スキルを順番に確認していく
    private void CheckTermsAndPlay()
    {
        executedSkillIndex += 1;

        if (executedSkillIndex >= skillObjects.Count)
        {
            this.callback.Invoke();
            return;
        }

        skillObjects[executedSkillIndex].SkillInvoke(DeleteSkillObject, CheckTermsAndPlay);
    }

    //SkillObject削除
    private void DeleteSkillObject()
    {
        Destroy(skillObjects[executedSkillIndex].gameObject);
        skillObjects.RemoveAt(executedSkillIndex);

        CheckTermsAndPlay();
    }

}
