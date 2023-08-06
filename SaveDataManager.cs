using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SaveData
{
    public string UserName;
    public string Password;
}

public class SaveDataManager : MonoBehaviour
{

    private string jsonPath = "Assets/Resources/SaveData.json";
    private string jsonFileName = "SaveData";

    public SaveData saveData;

    void Start()
    {
        Load();    
    }

    public void Save()
    {
        //stringに変換する
        string jsonstr = JsonUtility.ToJson(saveData);

        //ファイル書き込み用のライターを開く
        StreamWriter writer = new StreamWriter(jsonPath, false);

        //書き込み
        writer.Write(jsonstr);

        //ライターを閉じる処理
        writer.Flush();
        writer.Close();
    }

    public void Load()
    {
        if (!File.Exists(jsonPath))
        {
            Debug.Log("setting File not Exists");
            return;
        }

        string JsonData = Resources.Load<TextAsset>(jsonFileName).ToString();
        saveData = JsonUtility.FromJson<SaveData>(JsonData);
    }

}
