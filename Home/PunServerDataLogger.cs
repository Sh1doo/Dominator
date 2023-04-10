using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PunServerDataLogger : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI logText;

    void Update()
    {
        logText.text = LogText();
    }

    private string LogText()
    {
        //Stringでまとめたログを返す
        string str = null;
        str = $"稼働しているルーム数: {PhotonNetwork.CountOfRooms}\n";
        str += $"ルームに参加していないプレイヤー数: {PhotonNetwork.CountOfPlayersOnMaster}\n";
        str += $"ルーム内のプレイヤー数: {PhotonNetwork.CountOfPlayersInRooms}\n";
        str += $"接続しているプレイヤーの合計数: {PhotonNetwork.CountOfPlayers}\n";
        return str;
    }
}
