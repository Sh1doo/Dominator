using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    //Inspector用のChangeScene
    [Banzan.Lib.Utility.EnumAction(typeof(SceneName))]
    public void ChangeSceneForInspector(int sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Script用のChangeScene
    public void ChangeScene(SceneName sceneName)
    {
        SceneManager.LoadScene((int)sceneName);
    }

}