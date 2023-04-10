using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using Photon.Pun;

public static class PlayerData
{
    //NCMBデータストア[UserData]に登録した自身のObjectId
    public static string UserDataObjectId;

    //NCMBデータストアに設定されたプレイヤー名
    public static string PlayerName;
}