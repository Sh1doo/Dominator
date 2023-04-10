using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillObject_00 : SkillObject
{

    //地雷設置タイル
    public int targetTile;

    private CutInManager cutInManager;
    private TileManager tileManager;

    private int[] lastPoint = new int[GameConfigData.MaxPointArrayLength];
    //生存ターン
    private int lifeturn = 0;

    //PlayerSkillManagerへのコールバック
    private UnityAction delete;
    private UnityAction callback;

    public override void SkillInvoke(UnityAction delete, UnityAction callback)
    {
        this.delete = delete;
        this.callback = callback;

        //地雷生存期間の判定
        if (lifeturn >= GameConfigData.MineLifeTurn)
        {
            delete.Invoke();
            return;
        }
        else lifeturn++;

        //カットインをはさむかの条件分岐
        if (SkillTerm())
        {
            cutInManager.PlayCutIn(CutIn.Type.Skill00, EndCutIn);
        }
        else callback.Invoke();
    }

    //カットイン終了後
    private void EndCutIn()
    {
        //スキル
        for (int i = 0; i < GameConfigData.MaxPointArrayLength; ++i)
        {
            if (i != invoker)
            {
                tileManager.tileDatas[targetTile].point[i] = lastPoint[i];
            }
        }
        tileManager.SetColorAll();

        delete.Invoke();
    }

    //スキル発動条件
    private bool SkillTerm()
    {
        for (int i = 0; i < GameConfigData.MaxPointArrayLength; ++i)
        {
            if(i != invoker)
            {
                if (lastPoint[i] != tileManager.tileDatas[targetTile].point[i])
                {
                    return true;
                }
            }
        }
        return false;
    }

    //Setter
    public void setCutInManager(CutInManager cutInManager)
    {
        this.cutInManager = cutInManager;
    }


    public void setTileManager(TileManager tileManager)
    {
        this.tileManager = tileManager;
    }

    public void setInvoker(int invoker)
    {
        this.invoker = invoker;
    }

    public void setTargetTile(int targetTile)
    {
        this.targetTile = targetTile;
    }

    public void setAll(int invoker, int targetTile, CutInManager cutInManager, TileManager tileManager)
    {
        this.cutInManager = cutInManager;
        this.tileManager = tileManager;
        this.invoker = invoker;
        this.targetTile = targetTile;

        for(int i = 0; i < GameConfigData.MaxPointArrayLength; ++i)
        {
            lastPoint[i] = tileManager.tileDatas[targetTile].point[i];
        }
    }
}
