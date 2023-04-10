using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using NCMB;

public class LoginManager : MonoBehaviourPunCallbacks
{

    private bool isProccessing = false;
    private bool isSignIn = false;
    private string userDataObjectId;

    [SerializeField] private SceneChanger sceneChanger;
    [SerializeField] private TextMeshProUGUI statusText;

    public void Connect()
    {
        //すでに実行中の場合
        if (isProccessing) return;

        isProccessing = true;
        StartCoroutine(nameof(ConnectCoroutine));
    }

    private IEnumerator ConnectCoroutine()
    {
        //サインイン(初プレイの場合はSignUpが実行される)
        SignIn();

        //NCMBにサインインするまで待つ
        yield return new WaitUntil(() => isSignIn);

        //マスターサーバに接続
        statusText.text = "マスターサーバに接続中";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //ホームシーンへ移動する
        sceneChanger.ChangeScene(SceneName.Home);
    }

    private void SignIn()
    {
        //サインイン
        if (PlayerPrefs.HasKey("UserName"))
        {
            statusText.text = "サインイン中";
            NCMBUser.LogInAsync(PlayerPrefs.GetString("UserName"), PlayerPrefs.GetString("Password"), (NCMBException e) =>
            {
                if (e == null)
                {
                    //サインイン完了
                    isSignIn = true;
                }
            });
        }
        else
        {
            statusText.text = "サインアップ中";
            SignUp();
        }
    }

    private void LogIn()
    {
        //ログイン
        NCMBUser.LogInAsync(PlayerPrefs.GetString("UserName"), PlayerPrefs.GetString("Password"), (NCMBException e) =>
        {
            if (e == null)
            {
                isSignIn = true;
                AddNewUserData();
            }
        });
    }

    private void SignUp()
    {
        //サインアップ
        NCMBUser newUser = new NCMBUser();
        newUser.UserName = string.Format("Player{0:D4}{1}{2}{3}", UnityEngine.Random.Range(0, 9999), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        newUser.Password = string.Format("92ssw0r6{0:D4}", UnityEngine.Random.Range(0, 9999));

        newUser.SignUpAsync((NCMBException e) =>
        {
            if(e == null)
            {
                //登録完了
                NCMBObject userData = new NCMBObject("UserData");
                userData.Add("UserName", newUser.UserName);
                userData.Add("Password", newUser.Password);
                userData.SaveAsync((NCMBException er) =>
                {
                    if(er == null)
                    {
                        userDataObjectId = userData.ObjectId;
                        PlayerPrefs.SetString("UserName", newUser.UserName);
                        PlayerPrefs.SetString("Password", newUser.Password);
                        PlayerPrefs.Save();
                        LogIn();
                    }
                });
            }
        });
    }

    private void AddNewUserData()
    {
        //UserDataのObjectIdを会員情報に紐づける
        NCMBUser currentUser = NCMBUser.CurrentUser;
        if(currentUser != null)
        {
            currentUser.Add("UserDataID", userDataObjectId);
            currentUser.SaveAsync((NCMBException e) => { });
        }
        
    }

}
