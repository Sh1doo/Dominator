using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SimpleRawImageButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{

	#region Private Serializable Field

	[SerializeField] Color defaultColor;
	[SerializeField] Color selectColor;
	[SerializeField] UnityEvent clickAction;

    #endregion

    #region Private Fields

    RawImage image;

    #endregion

    #region Public Serializable Field
    #endregion

    #region Public Fields
    #endregion

    #region MonoBehaviour CallBacks

    void Start()
	{
        image = GetComponent<RawImage>();

        image.color = defaultColor;
	}

    #endregion

    #region Public Methods

    public void OnPointerClick(PointerEventData eventData) //クリックされたとき
    {
        clickAction.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData) //ポインターが乗ったとき
    {
        image.color = selectColor;
    }

    public void OnPointerExit(PointerEventData eventData) //ポインターが離れたとき
    {
        image.color = defaultColor;
    }

    #endregion

    #region Private Methods
    #endregion

}
