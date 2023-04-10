using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PunRoomSupporter : MonoBehaviourPunCallbacks
{

    [SerializeField] CanvasManager canvasManager;

    void Awake()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //適当なルームを作成して入る
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)GameConfigData.MaxPlayers;

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            canvasManager.SetCanvas(CanvasName.Loading);
            StartCoroutine(LoadScene((int)SceneName.Game));

        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            canvasManager.SetCanvas(CanvasName.Loading);
            StartCoroutine(LoadScene((int)SceneName.Game));
        }
    }
    
    private IEnumerator LoadScene(int scene)
    {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync((int)SceneName.Game);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            if(async.progress >= 0.9f)
            {
                async.allowSceneActivation = true;
                yield return null;
            }
        }

        async.allowSceneActivation = true;
        yield return null;

    }

}
