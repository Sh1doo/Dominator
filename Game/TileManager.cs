using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class TileManager : MonoBehaviour
{

    public TileData[] tileDatas;
    [SerializeField] private TileDOTween[] tileDOTweens;

    [SerializeField] private GameCanvasManager gameCanvasManager;

    public Dictionary<int, int> thisRoundPoint = new Dictionary<int,int>();

    [SerializeField] private GameConductor gameConductor;

    private PhotonView photonView;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();

        //得点入力時のポイントテキストエフェクトのため
        //for (int tileId = 0; tileId < tileDatas.Length; ++tileId)
        //{
        //    tileDatas[tileId].mainCanvas = mainCanvas;
        //}
    }

    //入力したポイントの情報を送る
    public void SendInputPointData()
    {
        //ポイント入力ラウンド終了後、thisRoundPointをtileDataに反映する
        for (int tileId = 0; tileId < tileDatas.Length; ++tileId)
        {
            if (thisRoundPoint.ContainsKey(tileId) && thisRoundPoint[tileId] != 0)
            {
                photonView.RPC(nameof(SetRoundPointToTile), RpcTarget.All, GameData.UserId, tileId, thisRoundPoint[tileId]);
            }
            thisRoundPoint[tileId] = 0;
        }
        //タイルデータの送信が終了した
        StartCoroutine(gameConductor.CountWaiting());
    }

    public void AddThisRoundPoint(int position, int point)
    {
        //本ラウンドで入力したポイントをタイルごとにまとめておく
        if (!thisRoundPoint.ContainsKey(position))
        {
            thisRoundPoint[position] = 0;
        }

        thisRoundPoint[position] += point;
    }
    
    [PunRPC]
    public void SetRoundPointToTile(int user, int position, int point)
    {
        //RoundPointに入力する
        tileDatas[position].SetRoundPoint(user, point);
    }

    public IEnumerator SetPlayerPoint(int userId)
    {
        //TileDataのPoint[]にuserIdのデータだけ反映してもらう
        for (int tileId = 0; tileId < tileDatas.Length; ++tileId)
        {
            if (tileDatas[tileId].isChangedPoint(userId))
            {
                tileDatas[tileId].SetPoint(userId);
                //yield return new WaitForSeconds(0.1f);
            }
        }

        //各プレイヤーのタイル数を更新
        gameCanvasManager.SetTiles(CheckOwner());
        yield return null;
    }

    //すべてのタイルの色を更新する
    public void SetColorAll()
    {
        for (int tileId = 0; tileId < tileDatas.Length; ++tileId)
        {
            tileDatas[tileId].SetColor();
        }
    }

    //すべてのタイルを明るくする
    public void SetLightColorAll()
    {
        for (int tileId = 0; tileId < tileDatas.Length; ++tileId)
        {
            tileDatas[tileId].SetLightColor();
        }
    }

    //すべてのタイルを暗くする
    public void SetDarkColorAll()
    {
        for (int tileId = 0; tileId < tileDatas.Length; ++tileId)
        {
            tileDatas[tileId].SetDarkColor();
        }
    }

    //タイルを明るくする
    public void SetLightColor(int tileId)
    {
        tileDatas[tileId].SetLightColor();
    }

    //タイルを暗くする
    public void SetDarkColor(int tileId)
    {
        tileDatas[tileId].SetDarkColor();
    }

    public int[] CheckOwner()
    {
        //プレイヤーごとのタイルの所有数を返す
        int[] count = { 0, 0, 0, 0 };
        for(int i = 0; i < tileDatas.Length; ++i)
        {
            if(tileDatas[i].owner != -1)
            {
                count[tileDatas[i].owner] += 1;
            }
        }
        return count;
    }

    public void MouseEnteredTile(int id)
    {
        //マウスがタイルに入った
        tileDOTweens[id].MouseEnter();
    }

    public void MouseExitedTile(int id)
    {
        //マウスがタイルから出た
        tileDOTweens[id].MouseExit();
    }

    public void MouseClicked(int id)
    {
        tileDOTweens[id].OnClick();
    }

}
