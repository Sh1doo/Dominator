using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SkillObject : MonoBehaviour
{
    //スキル使用者
    public int invoker;

    //スキルの発動(カットインや効果の適用)
    public abstract void SkillInvoke(UnityAction delete, UnityAction callback);
}
