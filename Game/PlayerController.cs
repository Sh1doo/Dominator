using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public TileManager tileManager;

    [SerializeField] private PlayerControllerReferense referense;

    private int residue = GameConfigData.DefaultResidue;
    private bool canInput = false;
    private int lockRayTile = -1;

    private PhotonView photonView;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();   
    }

    void Update()
    {
        if (canInput)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {   
                GameObject selectedTile = hit.collider.gameObject;
                int id = selectedTile.GetComponent<TileData>().id;

                //ポイント追加
                if (Input.GetMouseButtonDown(0) && residue > 0)
                {
                    referense.musicManager.PlaySE(MusicManager.Music.SE_Wood);

                    tileManager.MouseClicked(id);
                    tileManager.AddThisRoundPoint(id, 1);
                    setResidue(residue - 1);
                }

                //別のタイルが初めて選択されたときだけ
                if (id != lockRayTile)
                {
                    //前のタイルをExitして
                    if (lockRayTile != -1)
                    {
                        tileManager.MouseExitedTile(lockRayTile);
                    }
                    //今のタイルにEnterする
                    //referense.musicManager.PlaySE(MusicManager.Music.SE_Wood);

                    tileManager.MouseEnteredTile(id);
                    lockRayTile = id;
                }

            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            referense.tileInfoPop.SetActive(true);
            RightClickAction();
        } 
        else if (Input.GetMouseButtonUp(1))
        {
            referense.tileInfoPop.SetActive(false);
        }
    }

    public void InputPhaseStart()
    {
        //入力フェーズ開始
        setResidue(GameData.MaxResidue);

        referense.playerSkillManager.InputPhaseStart(GameData.SkillId);

        canInput = true;
    }

    public void InputPhaseEnd()
    {
        canInput = false;

        tileManager.MouseExitedTile(lockRayTile);
    }

    //スキルボタンクリック時
    public void OnClickSkillButton()
    {
        if (canInput)
        {
            referense.playerSkillManager.UseSkill(GameData.SkillId);
        }
    }

    public void setResidue(int residue)
    {
        //残りポイントを設定する
        this.residue = residue;

        //Canvasに反映
        referense.gameCanvasManager.SetResidue(residue);
    }

    public int getResidue()
    {
        return residue;
    }

    private void RightClickAction()
    {
        //タイル情報を表示
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            GameObject selectedTile = hit.collider.gameObject;

            for(int i = 0; i < 4; ++i)
            {
                referense.tileInfoText[i].text = selectedTile.GetComponent<TileData>().point[i].ToString();
            }
        }
    }

    //Setter
    public void setCanInput(bool flag)
    {
        canInput = flag;
    }

}
