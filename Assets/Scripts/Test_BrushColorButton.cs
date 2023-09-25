using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_BrushColorButton : MonoBehaviour
{
    public GameObject BrushObject;

    public void SetRedBrush()
    {
        BrushObject.GetComponent<PaintBrush>().SetBrushColor(Colors.red);
    }
    
    public void SetBlackBrush()
    {
        BrushObject.GetComponent<PaintBrush>().SetBrushColor(Colors.def);
    }
}
