using UnityEngine;
using UnityEngine.Events;

public abstract class CutIn : MonoBehaviour
{

    public enum Type
    {
        SpecialTile,

        Skill00,
    }

    public abstract void PlayCutIn(UnityAction callback);
}