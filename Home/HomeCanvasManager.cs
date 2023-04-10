using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomeCanvasManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerNameText;

    public void OnCompleteFetchUserData()
    {
        playerNameText.text = PlayerData.PlayerName;
    }

}
