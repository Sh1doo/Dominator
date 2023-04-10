using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private CharacterSkillDataManager skillDataManager;

    [SerializeField] private Image playerColor;
    [SerializeField] private Image residueBar;
    [SerializeField] private TextMeshProUGUI residueText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI wrenchText;
    [SerializeField] private TextMeshProUGUI tilesText;

    [SerializeField] private GameCanvasFooterDOTween footerDOTween;

    private int lookingPlayer = 0;
    private bool colorButtonActivation = true;

    //ゲーム開始時アニメーション
    public void DoAnimation_GameStartInit()
    {
        lookingPlayer = GameData.UserId;
        footerDOTween.DoAnimation_GameStartInit();
        SetColor(TileColor.getColor(GameData.UserId));

        timerText.text = skillDataManager.GetCharacterSkillData(GameData.SkillId).skillCT.ToString();
    }

    //色ボタンがクリックされたとき
    public void OnClickColorButton()
    {
        if (colorButtonActivation)
        {
            lookingPlayer = (lookingPlayer + 1) % GameConfigData.MaxPlayers;
            StartCoroutine(footerDOTween.TextFlowNameAndRank(GameData.UserData[lookingPlayer]["PlayerName"].ToString(), "Platinum III", EndTextFlow));
            SetColor(TileColor.getColor(lookingPlayer));
            SetTiles(tileManager.CheckOwner());

            colorButtonActivation = false;
        }
    }

    //テキスト反映アニメーション終了
    private void EndTextFlow()
    {
        colorButtonActivation = true;
    }

    //Setter
    public void SetColor(Color color)
    {
        playerColor.color = color;
    }

    public void SetTimer(int timer)
    {
        timerText.text = timer.ToString();
    }

    public void SetWrench(int wrench)
    {
        wrenchText.text = wrench.ToString();
    }

    public void SetResidue(int residue)
    {
        residueText.text = residue.ToString();
        residueBar.fillAmount = (float)residue / GameData.MaxResidue;
    }

    public void SetTiles(int[] tileCount)
    {
        tilesText.text = tileCount[lookingPlayer].ToString();
    }

}
