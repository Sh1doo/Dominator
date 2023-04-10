using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PunCallbacksLogger : MonoBehaviourPunCallbacks
{

    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーに接続しました");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"サーバーから切断されました: {cause.ToString()}");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("ルームを作成しました");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"ルームを作成できませんでした: {message}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("ルームへ参加しました");
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"ルームへ参加できませんでした: {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"ルームへ参加できませんでした: {message}");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("ルームから退出しました");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("ロビーへ参加しました");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("ロビーから退出しました");
    }

}
