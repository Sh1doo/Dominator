using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    [SerializeField] Canvas[] canvas;

    public void SetCanvas(CanvasName canvasName)
    {
        for(int i = 0; i < canvas.Length; ++i)
        {
            if (i == (int)canvasName) canvas[i].enabled = true;
            else canvas[i].enabled = false;
        }
    }

    [Banzan.Lib.Utility.EnumAction(typeof(CanvasName))]
    public void SetCanvasForInspector(int canvasName)
    {
        for (int i = 0; i < canvas.Length; ++i)
        {
            if (i == canvasName) canvas[i].enabled = true;
            else canvas[i].enabled = false;
        }
    }

    public void SetMultiCanvas(CanvasName[] canvasNames)
    {
        for (int i = 0; i < canvas.Length; ++i)
        {
            for(int j = 0; j < canvasNames.Length; ++j)
            {
                if (i == (int)canvasNames[j])
                {
                    canvas[i].enabled = true;
                    break;
                }
                else canvas[i].enabled = false;
            }
        }
    }
}
