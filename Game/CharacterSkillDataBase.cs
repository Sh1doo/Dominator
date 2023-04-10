using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSkillDataBase", menuName = "ScriptableObjects/CharacterSkillDataBase")]
public class CharacterSkillDataBase : ScriptableObject
{
    public List<CharacterSkillData> characterSkillDatas;
}

[System.Serializable]
public class CharacterSkillData
{
    public string skillName;
    public int skillID;
    public int skillCT;
}