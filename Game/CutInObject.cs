using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CutInObject : MonoBehaviour
{
    //カットインの種類
    public CutIn.Type type;

    public abstract void CheckTermsAndPlay(UnityAction callback);

}
