using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfigData
{
    public static int MaxPlayers = 1;
    public static int MaxTurn = 10;
    public static double MaxInputPhaseTime = 30;
    public static int DefaultResidue = 10;

    public static int WindmillBonus = 1;

    public static float CountPhaseSetColorInterval = 1f;

    //タイルが保持する最大プレイヤー数（不変）
    public static int MaxPointArrayLength = 4;

    //スキル関係（のちにSkillのScriptableオブジェクトに統合するかも）
    public static int MineLifeTurn = 2;
}
