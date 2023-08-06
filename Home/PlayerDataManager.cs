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
        PlayerData.BasicUserDataObjectID = (string)NCMBUser.CurrentUser["BasicUserDataObjectID"];

        //ObjectIDからプレイヤーのデータを設定
        NCMBObject fetchData = new NCMBObject("BasicUserData");
        fetchData.ObjectId = PlayerData.BasicUserDataObjectID;
        fetchData.FetchAsync((NCMBException e) =>
        {
            if (e == null)
            {
                PlayerData.PlayerName = (string)fetchData["NAME"];

                homeCanvasManager.OnCompleteFetchUserData();
            }
        });
    }

}
