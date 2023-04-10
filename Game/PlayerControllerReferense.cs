using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControllerReferense : MonoBehaviour
{
    public GameObject tileInfoPop;
    public TextMeshProUGUI[] tileInfoText;

    public Image residueImage;
    public TextMeshProUGUI residueText;

    public PlayerSkillManager playerSkillManager;
    public GameCanvasManager gameCanvasManager;
    public MusicManager musicManager;
}
