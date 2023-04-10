using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MatchingManager : MonoBehaviour
{

    public void QuickMatch()
    {
        //クイックマッチングを行う
        PhotonNetwork.JoinRandomRoom();
    }

}
