using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultCanvasManager : MonoBehaviour
{

    [SerializeField] private TileManager tileManager;

    [SerializeField] Image[] playerColor;
    [SerializeField] TextMeshProUGUI[] playerNameText;
    [SerializeField] TextMeshProUGUI[] resultTilesCountText;

    private int[] result = new int[GameConfigData.MaxPlayers];

    public void SetResultData()
    {
        result = tileManager.CheckOwner();
        List<Pair> list = new List<Pair>(new Pair[] {
            new Pair(0,result[0]),
            new Pair(1,result[1]),
            new Pair(2,result[2]),
            new Pair(3,result[3]),
        }) ;
        list.Sort(Pair.CompairPairSecond);
        list.Reverse();

        //名前を設定する
        for (int i = 0; i < GameConfigData.MaxPlayers; ++i)
        {
            playerNameText[list[i].a].text = (string)GameData.UserData[list[i].a]["PlayerName"];
        }

        for (int i = 0; i < GameConfigData.MaxPlayers; ++i)
        {
            resultTilesCountText[i].text = list[i].b.ToString();
            resultTilesCountText[i].color = TileColor.getColor(list[i].a);
            playerColor[i].color = TileColor.getColor(list[i].a);
        }
    }
}
