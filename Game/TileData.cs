using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileData : MonoBehaviour
{

    public int id;
    public int[] point = { 0, 0, 0, 0 };        //このタイルのポイント情報
    public int owner = -1;
    private int lastOwner = -1;

    private int[] roundPoint = { 0, 0, 0, 0 };   //1ターンでの各プレイヤーの入力点数
    private Material material;

    [SerializeField] private TileDOTween tileDOTween;
    //[SerializeField] private ParticleSystem particle;

    //public RectTransform mainCanvas; //Textをタイルの位置に表示させるために必要

    void Start()
    {
        material = this.GetComponent<Renderer>().material;
    }

    public void SetRoundPoint(int user, int point)
    {
        //roundPointに反映
        roundPoint[user] = point;
    }

    public void SetPoint(int user)
    {
        //userが何もこのタイルに入力していない
        if (!isChangedPoint(user)) return;

        //Pointに反映
        point[user] += roundPoint[user];
        SetColor();

        //roundPointはpointに追加するたびにResetする
        roundPoint[user] = 0;
    }

    //オーナーを設定し色を変更する
    public void SetColor()
    {
        //タイルの色を変更
        SetOwner();

        if (lastOwner != owner)
        {
            tileDOTween.Rotate();
            material.SetColor("_Color", TileColor.getColor(owner));
            lastOwner = owner;
        } 
        else
        {
            //色が変更するわけじゃないけど点数は入れた
            tileDOTween.Rotate();
            //material.SetColor("_Color", TileColor.getColor(owner));
            //lastOwner = owner;
        }
    }

    //明るい色に変更
    public void SetLightColor()
    {
        material.SetColor("_Color", TileColor.getColor(owner));
    }

    //暗い色に変更
    public void SetDarkColor()
    {
        material.SetColor("_Color", TileColor.getDarkColor(owner));
    }

    private void SetOwner()
    {
        //タイルの所有者を返す、最大ポイント入力者が複数いる場合はタイルは誰のものにもならない
        int maxPointPlayer = 0;
        int maxPointPlayerCount = 0;
        for(int i = 0; i < point.Length; ++i)
        {
            if (point.Max() == point[i])
            {
                maxPointPlayerCount++;
                maxPointPlayer = i;
            } 
        }

        if (maxPointPlayerCount == 1) owner = maxPointPlayer;
        else owner = -1;
    }

    /*
    private void ShowPoint()
    {
        //テキストエフェクト
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        GameObject pointText = GameObject.Instantiate(pointTextPrefab, mainCanvas);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas, screenPos, null, out Vector2 localPos);
        pointText.GetComponent<RectTransform>().localPosition = localPos;
    }
    */

    public bool isChangedPoint(int user)
    {
        //UserのroudPointが変化しているか
        if (roundPoint[user] == 0) return false;
        else return true;
    }

}
