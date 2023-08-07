using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using NCMB;

public class GameDataManager : MonoBehaviour
{
    //UserDataを設定した回数
    private int setUserDataCount = 0;

    void Start()
    {
        //GameDataを初期化する
        GameData.Init();

        SetId();
    }

    /// <summary>
    /// PlayerListの順番でIDを設定する
    /// </summary>
    private void SetId()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; ++i)
        {
            GameData.Id.Add(PhotonNetwork.PlayerList[i].ActorNumber, i);

            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                GameData.UserId = i;
            }
        }
    }

    /// <summary>
    /// userIdのCustomPropertyに基づいてGameData.UserDataに登録
    /// </summary>
    /// <param name="userId"></param>
    public void SetUserData(int userId)
    {
        NCMBObject fetchData = new NCMBObject("BasicUserData");
        fetchData.ObjectId = (string)PhotonNetwork.PlayerList[userId].CustomProperties["BasicUserData"];
        fetchData.FetchAsync((NCMBException e) =>
        {
            if (e == null)
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("PlayerName", (string)fetchData["NAME"]);
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
