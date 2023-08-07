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

    //private ExitGames.Client.Photon.Hashtable[] allHash = new ExitGames.Client.Photon.Hashtable[GameConfigData.MaxPlayers];

    private double inputStartTime;
    private int executedSkillIndex = 0;

    private bool firstRound = true;

    [SerializeField] private GameConductorReferense r;

    [SerializeField] private new PhotonView photonView;

    void Start()
    {
        //Canvasをロードに
        r.canvasManager.SetCanvas(CanvasName.Loading);

        //待機
        StartCoroutine(nameof(WaitingPhaseControll));
    }

    /// <summary>
    /// 全員が次の状態に移行できる準備が整っているか判定する
    /// </summary>
    private bool IsAllPlayersReady(byte n)
    {
        for (int i = 0; i < GameConfigData.MaxPlayers; ++i)
        {
            if (!PhotonNetwork.PlayerList[i].CustomProperties.ContainsKey("State") || (byte)PhotonNetwork.PlayerList[i].CustomProperties["State"] != n)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 2 : 全プレイヤーの情報をNCMBデータストアを参照して設定
    /// </summary>
    [PunRPC]
    private void SetAllPlayersData()
    {
        for(int i = 0; i < GameConfigData.MaxPlayers; ++i)
        {
            r.gameDataManager.SetUserData(i);
        }

        StartCoroutine(r.gameDataManager.IsFetchedAll(() =>
        {
            myState = (byte)GamePhase.WaitingAllReady;
            myHash["State"] = myState;
            PhotonNetwork.LocalPlayer.SetCustomProperties(myHash);
        }
        ));
    }

    /// <summary>
    /// 3 : 入力フェーズの開始合図
    /// </summary>
    [PunRPC]
    private void InputPhaseStart()
    {
        //入力フェーズ開始
        GameData.Turn += 1;

        if (GameData.Turn == 1)
        {
            r.canvasManager.SetCanvas(CanvasName.Game);
            r.gameCanvasManager.DoAnimation_GameStartInit();
        }
        
        myState = (byte)GamePhase.Input;

        r.uiAnimationCanvasManager.DoAnimation_InputPhaseNotice(EndUIAnimation);
    }

    /// <summary>
    /// 4 : 各プレイヤーの入力フェーズの開始合図
    /// </summary>
    [PunRPC]
    private void InputPhaseStart_small()
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

    /// <summary>
    /// 7 : 受け取った得点の反映開始合図
    /// </summary>
    [PunRPC]
    private void CountPhaseReStart()
    {
        StartCoroutine(PointReflection());
    }

    /// <summary>
    /// 9 : スキルフェーズ開始合図
    /// </summary>
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
                    photonView.RPC(nameof(InputPhaseStart_small), RpcTarget.All);
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

    /// <summary>
    /// 1 : カスタムプロパティの更新と待機
    /// </summary>
    private IEnumerator WaitingPhaseControll()
    {
        //カスタムプロパティ更新
        myState = (byte)GamePhase.Waiting;
        myHash["State"] = myState;
        myHash["BasicUserData"] = PlayerData.BasicUserDataObjectID;
        PhotonNetwork.LocalPlayer.SetCustomProperties(myHash);

        //全員が待機状態になるまで待つ(MasterClientのみ確認)
        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitUntil(() => IsAllPlayersReady((byte)GamePhase.Waiting));

            if (firstRound) 
            {
                photonView.RPC(nameof(SetAllPlayersData), RpcTarget.All);
                yield return new WaitUntil(() => IsAllPlayersReady((byte)GamePhase.WaitingAllReady));
                firstRound = false;
            } 

            //ゲーム終了かどうか
            if (GameData.Turn == GameConfigData.MaxTurn)
            {
                photonView.RPC(nameof(EndGame), RpcTarget.All);
            }
            else
            {
                //InputPhaseStart
                photonView.RPC(nameof(InputPhaseStart), RpcTarget.All);
            }
        }

        yield break;
    }

    /// <summary>
    /// 5 : 自分の入力フェーズとカスタムプロパティの更新
    /// </summary>
    /// <returns></returns>
    private IEnumerator InputPhaseControll()
    {
        //InputPhaseの間の動作を管理する
        inputStartTime = PhotonNetwork.Time;
        r.playerController.InputPhaseStart();

        //タイマー終了or入力終了まで待つ
        yield return new WaitUntil(() => IsEndInputPhase());
        r.playerController.InputPhaseEnd();

        //少し待つ
        yield return new WaitForSeconds(1f);

        //入力終了
        myState = (byte)GamePhase.InputEnd;
        myHash["State"] = myState;
        PhotonNetwork.LocalPlayer.SetCustomProperties(myHash);

        //ポイントを全プレイヤーに送る
        r.tileManager.SendInputPointData();
    }

    private IEnumerator CountPhaseControll()
    {
        //全員が準備できるまで待つ
        yield return new WaitUntil(() => IsAllPlayersReady((byte)GamePhase.CountEnd));
        photonView.RPC(nameof(SkillPhaseStart), RpcTarget.All);
    }

    /// <summary>
    /// 6 : 機能未完成 全員が得点情報を受け取るまで待つ
    /// </summary>
    public IEnumerator CountWaiting()
    {
        //全員がポイントを受け取るまで待つ
        yield return null;
        photonView.RPC(nameof(CountPhaseReStart), RpcTarget.All);
    }

    /// <summary>
    /// 8 : 得点をタイルに反映 カスタムプロパティの更新と待機
    /// </summary>
    /// <returns></returns>
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
            if (inputTurnUserId == 0)
            {
                photonView.RPC(nameof(SkillPhaseStart), RpcTarget.All);
            }
            else
            {
                photonView.RPC(nameof(InputPhaseStart_small), RpcTarget.All);
            }
        }

    }

    /// <summary>
    /// 10 : カットインが必要なだけ再生
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 入力フェーズの終了条件を満たすかどうか
    /// </summary>
    /// <returns></returns>
    private bool IsEndInputPhase()
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
