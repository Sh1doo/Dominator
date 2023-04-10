using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    //現在のターン
    public static int Turn;

    //最大寄付得点
    public static int MaxResidue;

    //自分のプレイヤーID
    public static int UserId;

    //自分のスキルID
    public static int SkillId = 0;

    //地雷の所持数
    public static int Mine = 0;

    //スキルクールタイム
    public static int SkillCT = 0;

    //全プレイヤーのActorNumberとIDの関連
    public static Dictionary<int, int> Id;

    //全プレイヤーのUserData
    public static Hashtable[] UserData;

    public static void Init()
    {
        //初期化
        Turn = 0;
        MaxResidue = GameConfigData.DefaultResidue;
        Id = new Dictionary<int, int>();
        UserData = new Hashtable[GameConfigData.MaxPlayers];
    }

}
