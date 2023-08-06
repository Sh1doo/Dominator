using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using Photon.Pun;

public static class PlayerData
{
    //NCMBデータストア[BasicUserDataObjectID]に登録した自身のObjectId
    public static string BasicUserDataObjectID;

    //NCMBデータストアに設定されたプレイヤー名
    public static string PlayerName;
}