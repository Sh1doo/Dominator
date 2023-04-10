using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class PlayerDataManager : MonoBehaviour
{

    [SerializeField] private HomeCanvasManager homeCanvasManager;

    void Awake()
    {
        //ObjectIDの設定
        PlayerData.UserDataObjectId = (string)NCMBUser.CurrentUser["UserDataID"];

        //ObjectIDからプレイヤーのデータを設定
        NCMBObject fetchUserData = new NCMBObject("UserData");
        fetchUserData.ObjectId = PlayerData.UserDataObjectId;
        fetchUserData.FetchAsync((NCMBException e) =>
        {
            if (e == null)
            {
                PlayerData.PlayerName = (string)fetchUserData["UserName"];

                homeCanvasManager.OnCompleteFetchUserData();
            }
        });
    }

}
