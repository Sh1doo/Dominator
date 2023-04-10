using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutInObject_Windmill : CutInObject
{

    [SerializeField] private TileManager tileManager;

    [SerializeField] private TileData tileData;
    [SerializeField] private TileData[] effectiveTiles;

    [SerializeField] private CutInManager cutInManager;

    //CutInManagerへのコールバック
    private UnityAction callback;

    //コンストラクタ
    CutInObject_Windmill()
    {
        type = CutIn.Type.SpecialTile;
    }

    public override void CheckTermsAndPlay(UnityAction callback)
    {
        this.callback = callback;

        //カットインをはさむかの条件分岐
        if (tileData.owner != -1)
        {
            cutInManager.PlayCutIn(CutIn.Type.SpecialTile, EndCutIn);
        }
        else this.callback.Invoke();
    }

    private void EndCutIn()
    {
        //カットイン終了後スキルの実行
        for (int i = 0; i < effectiveTiles.Length; ++i)
        {
            tileManager.SetRoundPointToTile(tileData.owner, effectiveTiles[i].id, GameConfigData.WindmillBonus);
        }
        StartCoroutine(tileManager.SetPlayerPoint(tileData.owner));

        this.callback.Invoke();
    }

}
