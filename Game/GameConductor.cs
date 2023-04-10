using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameConductor : MonoBehaviourPunCallbacks
{

    public static byte myState = (byte)GamePhase.Waiting;
    private int inputTurnUserId = 0;

    private ExitGames.Client.Photon.Hashtable myHash = new ExitGames.Client.Photon.Hashtable();
    private ExitGames.Client.Photon.Hashtable[] otherPlayer = new ExitGames.Client.Photon.Hashtable[GameConfigData.MaxPlayers];

    private double inputStartTime;
    private int executedSkillIndex = 0;

    private bool isAllPlayersReadyForStart = false;

    [SerializeField] private GameConductorReferense r;

    [SerializeField] private new PhotonView photonView;

    void Start()
    {
        //Canvasをロードに
        r.canvasManager.SetCanvas(CanvasName.Loading);

        //自身のデータをカスタムプロパティに設定
        myHash["UserDataId"] = PlayerData.UserDataObjectId;
        myHash["Skill"] = GameData.SkillId;
        PhotonNetwork.LocalPlayer.SetCustomProperties(myHash);

        //待機
        StartCoroutine(nameof(WaitingPhaseControll));
    }

    private bool IsAllPlayersReady(byte n)
    {
        ////全員が次の状態に移行できる準備が整っているか判定する

        //Stateが設定されいるか確認
        for (int i = 0; i < GameConfigData.MaxPlayers; ++i)
        {
            otherPlayer[i] = PhotonNetwork.PlayerList[i].CustomProperties;

            if (otherPlayer[i] == null) //設定されていない
            {
                return false;
            }
            else if (!otherPlayer[i].ContainsKey("State")) //設定はされているがStateがない
            {
                return false;
            }
        }

        int equalStateCount = 0;

        for (int i = 0; i < GameConfigData.MaxPlayers; ++i)
        {
            byte otherstate = (byte)otherPlayer[i]["State"];

            if (otherstate == n)
            {
                equalStateCount++;
            }
        }

        if (equalStateCount == GameConfigData.MaxPlayers) return true;
        else return false;
    }

    //他プレイヤーのカスタムプロパティ参照
    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    //{
    //    int targetId = GameData.Id[targetPlayer.ActorNumber];
    //    otherPlayer[targetId] = changedProps;
    //}

    [PunRPC]
    private void SetAllPlayersDataAndInputPhaseStart()
    {
        //Canvas設定
        r.canvasManager.SetCanvas(CanvasName.Game);
        r.gameCanvasManager.DoAnimation_GameStartInit();

        InputPhaseStart();
    }

    [PunRPC]
    private void CheckUserData()
    {
        if(isAllPlayersReadyForStart)
        {
            myState = (byte)GamePhase.WaitingAllReady;
            myHash["State"] = myState;
            PhotonNetwork.LocalPlayer.SetCustomProperties(myHash);
        }
        else
        {
            PrepareForBeginGame();
        }
    }

    [PunRPC]
    private void InputPhaseStart()
    {
        //入力フェーズ開始
        GameData.Turn += 1;
        myState = (byte)GamePhase.Input;

        r.uiAnimationCanvasManager.DoAnimation_InputPhaseNotice(EndUIAnimation);
    }

    [PunRPC]
    private void StartInputPhase()
    {
        //各プレイヤーのターン開始
        myState = (byte)GamePhase.InputWaiting;

        r.uiAnimationCanvasManager.DoAnimation_TurnPlayerNotice(EndUIAnimation, (string)GameData.UserData[inputTurnUserId]["PlayerName"]);
    }

    [PunRPC]
    private void CountPhaseStart()
    {
        //集計フェーズ開始
        myState = (byte)GamePhase.Count;

        r.uiAnimationCanvasManager.DoAnimation_CountPhaseNotice(EndUIAnimation);
    }

    [PunRPC]
    private void CountPhaseReStart()
    {
        //集計フェーズ再開（タイルデータ集計が終わってからそれだけを反映するため）
        StartCoroutine(PointReflection());
    }

    [PunRPC]
    private void SkillPhaseStart()
    {
        //スキルフェーズ開始
        myState = (byte)GamePhase.Skill;

        r.uiAnimationCanvasManager.DoAnimation_SkillPhaseNotice(EndUIAnimation);
    }

    [PunRPC]
    private void EndGame()
    {
        //ゲーム終了
        r.canvasManager.SetCanvas(CanvasName.Result);
        r.resultCanvasManager.SetResultData();
    }

    private void EndUIAnimation()
    {
        //UIアニメーションをフェーズの間にはさむ
        switch (myState)
        {
            case (byte)GamePhase.Waiting:
                

            case (byte)GamePhase.Input:
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC(nameof(StartInputPhase), RpcTarget.All);
                }
                break;

            case (byte)GamePhase.InputWaiting:
                if (inputTurnUserId == GameData.UserId)
                {
                    StartCoroutine(nameof(InputPhaseControll));
                }
                break;

            case (byte)GamePhase.Count:
                StartCoroutine(nameof(CountPhaseControll));
                break;

            case (byte)GamePhase.Skill:
                StartCoroutine(nameof(SkillPhaseControll));
                break;
        }
    }

    private void EndResultUIAnimation()
    {
        //Result演出終了後データの反映などをする
        //r.canvasManager.ChangeCanvas(CanvasName.Result);
    }

    private IEnumerator WaitingPhaseControll()
    {
        //カスタムプロパティ更新
        myState = (byte)GamePhase.Waiting;
        myHash["State"] = myState;
        PhotonNetwork.LocalPlayer.SetCustomProperties(myHash);

        //全員が待機状態になるまで待つ(MasterClientのみ確認)
        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitUntil(() => IsAllPlayersReady((byte)GamePhase.Waiting));

            photonView.RPC(nameof(CheckUserData), RpcTarget.All);

            yield return new WaitUntil(() => IsAllPlayersReady((byte)GamePhase.WaitingAllReady));

            //ゲーム終了かどうか
            if (GameData.Turn == GameConfigData.MaxTurn)
            {
                photonView.RPC(nameof(EndGame), RpcTarget.All);
            }
            else if (GameData.Turn == 0)
            {
                //はじめてのターンプレイヤーデータなど設定する.
                photonView.RPC(nameof(SetAllPlayersDataAndInputPhaseStart), RpcTarget.All);
            }
            else
            {
                //InputPhaseStart
                photonView.RPC(nameof(InputPhaseStart), RpcTarget.All);
            }
        }
        
        yield break;
    }

    private IEnumerator InputPhaseControll()
    {
        //InputPhaseの間の動作を管理する
        inputStartTime = PhotonNetwork.Time;
        r.playerController.InputPhaseStart();

        //タイマー終了or入力終了まで待つ
        yield return new WaitUntil(() => CheckInputPhaseEnd());
        r.playerController.InputPhaseEnd();

        //少し待つ
        yield return new WaitForSeconds(1f);
        
        //入力終了
        myState = (byte)GamePhase.InputEnd;
        myHash["State"] = myState;
        PhotonNetwork.LocalPlayer.SetCustomProperties(myHash);

        //ポイントを全プレイヤーに送る
        r.tileManager.SendInputPointData();

        //全員が準備できるまで待つ
        //yield return new WaitUntil(() => IsAllPlayersReady((byte)GamePhase.InputEnd));
        //photonView.RPC(nameof(CountPhaseStart), RpcTarget.All);
    }

    private IEnumerator CountPhaseControll()
    {
        //CountPhaseの間の動作を管理する
        //r.cameraController.CountPhaseStart();

        //カメラ移動時間待つ
        //yield return new WaitForSeconds(1.5f);
        

        //全員が準備できるまで待つ
        yield return new WaitUntil(() => IsAllPlayersReady((byte)GamePhase.CountEnd));
        photonView.RPC(nameof(SkillPhaseStart), RpcTarget.All);
    }

    public void EndSendPoint()
    {
        StartCoroutine(CountWaiting());
    }

    private IEnumerator CountWaiting()
    {
        //myState = (byte)GamePhase.ReceivedPointData;
        //SetCustomPropState(myState);

        //全員が準備できるまで待つ
        //全員がこの時点で明らかにポイント情報を受け取った跡であると仮定している.もし受け取りにミスが頻発するようであればコメントアウト以降を使って対処
        yield return null; //new WaitUntil(() => IsAllPlayersReady((byte)GamePhase.ReceivedPointData));
        photonView.RPC(nameof(CountPhaseReStart), RpcTarget.All);
    }

    private IEnumerator PointReflection()
    {
        //プレイヤーのInputPhaseの集計を行う
        yield return StartCoroutine(r.tileManager.SetPlayerPoint(inputTurnUserId));
        inputTurnUserId++;

        //カスタムプロパティ更新
        myState = (byte)GamePhase.CountEnd;
        myHash["State"] = myState;
        PhotonNetwork.LocalPlayer.SetCustomProperties(myHash);

        //入力ターン中のプレイヤー情報を初期化
        if (inputTurnUserId == GameConfigData.MaxPlayers)
        {
            inputTurnUserId = 0;
        }

        //全員がタイルの色変更まで終了したか(MasterClientのみ確認)
        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitUntil(() => IsAllPlayersReady((byte)GamePhase.CountEnd));

            //SkillPhase開始(inputTurnUserIdが初期化済みなら)
            if(inputTurnUserId == 0)
            {
                photonView.RPC(nameof(SkillPhaseStart), RpcTarget.All);
            }
            else
            {
                photonView.RPC(nameof(StartInputPhase), RpcTarget.All);
            }
        }

    }

    private IEnumerator SkillPhaseControll()
    {
        r.cutInManager.PlayAllCutIn(SkillCutInEnd);

        yield return null;
    }
    //追加のスキルフェーズ
    private IEnumerator SkillPhaseControllEx()
    {
        r.playerSkillManager.PlayAllSkillCutIn(SkillCutInEndEx);

        yield return null;
    }

    //ラウンド終了後のカットイン終了時
    private void SkillCutInEnd()
    {
        StartCoroutine(nameof(SkillPhaseControllEx));
    }

    //延長スキルフェーズのカットイン終了時
    private void SkillCutInEndEx()
    {
        StartCoroutine(nameof(WaitingPhaseControll));
    }

    //ユーザデータの設定など準備をする
    private void PrepareForBeginGame()
    {
        int readyPlayersCount = 0;

        for(int i = 0; i < GameConfigData.MaxPlayers; ++i)
        {
            otherPlayer[i] = PhotonNetwork.PlayerList[i].CustomProperties;
            if (otherPlayer[i].ContainsKey("UserDataId"))
            {
                readyPlayersCount += 1;
            }
        }

        if (readyPlayersCount == GameConfigData.MaxPlayers)
        {
            //1度のみユーザデータの設定
            for (int i = 0; i < GameConfigData.MaxPlayers; ++i)
            {
                r.gameDataManager.SetUserData(i);
            }

            StartCoroutine(r.gameDataManager.IsFetchedAll(setIsAllPlayersReadyForStart));
        }
    }

    //Setter
    private void setIsAllPlayersReadyForStart()
    {
        isAllPlayersReadyForStart = true;

        myState = (byte)GamePhase.WaitingAllReady;
        myHash["State"] = myState;
        PhotonNetwork.LocalPlayer.SetCustomProperties(myHash);
    }

    private bool CheckInputPhaseEnd()
    {
        if (r.playerController.getResidue() == 0)
        {
            //できることがない
            return true;
        }
        else if ((float)unchecked(PhotonNetwork.Time - inputStartTime) >= GameConfigData.MaxInputPhaseTime)
        {
            //時間切れ
            return true;
        }

        return false;
    }

    public void OnClickBackButton()
    {
        //リザルト表示後出現するボタンをクリックしたときホームへ戻る
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        r.sceneChanger.ChangeScene(SceneName.Home);
    }

}
