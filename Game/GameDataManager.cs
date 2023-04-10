using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using NCMB;

public class GameDataManager : MonoBehaviour
{

    [SerializeField] GameCanvasManager gameCanvas;

    //UserDataを設定した回数
    private int setUserDataCount = 0;

    void Start()
    {
        //GameDataを初期化する
        GameData.Init();

        SetId();
    }

    private void SetId()
    {
        //PlayerListの順番でIDを設定する
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; ++i)
        {
            GameData.Id.Add(PhotonNetwork.PlayerList[i].ActorNumber, i);

            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                GameData.UserId = i;
            }
        }
    }

    public void SetUserData(int userId)
    {
        //指定されたActorNumberのプレイヤーのNCMB["UserData"]をGameDataのUserDataに設定する

        NCMBObject fetchUserData = new NCMBObject("UserData");
        fetchUserData.ObjectId = (string)PhotonNetwork.PlayerList[userId].CustomProperties["UserDataId"];
        fetchUserData.FetchAsync((NCMBException e) =>
        {
            if (e == null)
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("PlayerName", (string)fetchUserData["UserName"]);
                GameData.UserData[userId] = hashtable;

                setUserDataCount += 1;
            }
        });
    }

    public IEnumerator IsFetchedAll(UnityAction callback)
    {
        yield return new WaitUntil(() => (setUserDataCount == GameConfigData.MaxPlayers));

        callback.Invoke();
    }
}
