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

    [SerializeField] private SaveDataManager saveDataManager;
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
        if (saveDataManager.saveData.UserName != "DefaultUserName")
        {
            statusText.text = "サインイン中";
            NCMBUser.LogInAsync(saveDataManager.saveData.UserName, saveDataManager.saveData.Password, (NCMBException e) =>
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

    private void SignUp()
    {
        //サインアップ
        NCMBUser newUser = new NCMBUser();
        newUser.UserName = string.Format("GuestPlayer{0:D4}{1}{2}{3}", UnityEngine.Random.Range(0, 9999), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        newUser.Password = string.Format("92ssw0r6{0:D4}", UnityEngine.Random.Range(0, 9999));

        newUser.SignUpAsync((NCMBException e) =>
        {
            if (e != null)
            {
                Debug.Log("Error:SignUp Failed");
            }
            else
            {
                //登録完了
                saveDataManager.saveData.UserName = newUser.UserName;
                saveDataManager.saveData.Password = newUser.Password;
                saveDataManager.Save();

                NCMBUser.LogInAsync(newUser.UserName, newUser.Password, (NCMBException er) =>
                {
                    if (er == null)
                    {
                        isSignIn = true;
                        AddNewUserData();
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
            NCMBObject basicUserData = new NCMBObject("BasicUserData");
            basicUserData.Add("NAME", "DefaultName");
            basicUserData.Add("LEVEL", 1);
            basicUserData.Add("EXP", 0);
            basicUserData.SaveAsync((NCMBException er) =>
            {
                if (er != null)
                {
                    Debug.Log("Error:BasicUserData Save Failed");
                }
                else 
                {
                    currentUser.Add("BasicUserDataObjectID", basicUserData.ObjectId);
                    currentUser.SaveAsync((NCMBException e) => {
                        if( e != null )
                        {
                            Debug.Log("Error:BasicUserDataObjectID Save Failed");
                        }
                    });
                }
            });
        }
        
    }

}
