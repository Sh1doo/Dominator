using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera vcamOblique;
    [SerializeField] private CinemachineVirtualCamera vcamTop;

    public void InputPhaseStart()
    {
        //ポイント入力用にカメラを移動
        vcamTop.Priority = 20;
    }

    public void CountPhaseStart()
    {
        //カメラを元の位置に戻す
        vcamTop.Priority = 0;
    }

}
