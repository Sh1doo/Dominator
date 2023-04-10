using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillDataManager : MonoBehaviour
{
    [SerializeField] private CharacterSkillDataBase characterSkillDataBase;

    public CharacterSkillData GetCharacterSkillData(int skillID)
    {
        return characterSkillDataBase.characterSkillDatas[skillID];
    }

}