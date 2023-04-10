using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_00 : Skill
{

    [SerializeField] private TileManager tileManager;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerSkillManager playerSkillManager;
    [SerializeField] private GameCanvasManager gameCanvasManager;

    //対象タイル選択フラグ
    private bool select = false;
    //暫定選択中のタイル
    private int lockRayTile = -1;

    void Update()
    {
        if (select)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                GameObject selectedTile = hit.collider.gameObject;
                int id = selectedTile.GetComponent<TileData>().id;

                //対象タイル選択
                if (Input.GetMouseButtonUp(0))
                {
                    playerSkillManager.CheckSkillCutIn_00(GameData.UserId, id);

                    GameData.Mine -= 1;
                    gameCanvasManager.SetWrench(GameData.Mine);

                    EndSkill();
                }

                //別のタイルが初めて選択されたときだけ
                if (id != lockRayTile)
                {
                    //前のタイルをExitして
                    if (lockRayTile != -1)
                    {
                        tileManager.MouseExitedTile(lockRayTile);
                        tileManager.SetDarkColor(lockRayTile);
                    }
                    //今のタイルにEnterする
                    tileManager.MouseEnteredTile(id);
                    tileManager.SetLightColor(id);
                    lockRayTile = id;
                }

            }
        }
    }

    //スキル実行
    public override void DoSkill()
    {
        //スキル発動準備
        tileManager.SetDarkColorAll();
        stageManager.SetStageDark();

        playerController.setCanInput(false);
        select = true;
    }

    //スキル終了
    private void EndSkill()
    {
        tileManager.SetLightColorAll();
        stageManager.SetStageLight();

        playerController.setCanInput(true);
        select = false;
    }
}
