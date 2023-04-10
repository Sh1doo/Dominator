using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    //明るい時/暗い時の色
    [SerializeField] Color lightColor;
    [SerializeField] Color darkColor;

    //ステージに適用されたマテリアル
    [SerializeField] Material[] materials;

    void Start()
    {
        SetStageLight();
    }

    //ステージを明るく
    public void SetStageLight()
    {
        for(int i = 0; i < materials.Length; ++i)
        {
            //materials[i].SetColor("_Light_Color",lightColor);
        }
    }

    //ステージを暗く
    public void SetStageDark()
    {
        for (int i = 0; i < materials.Length; ++i)
        {
            //materials[i].SetColor("_Light_Color", darkColor);
        }
    }

    
}
