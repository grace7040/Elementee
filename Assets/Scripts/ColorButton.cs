using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColorButton : MonoBehaviour
{
    

    public Colors color;

    ColorState buttonColor;

    private Button button;

    private void Start()
    {
        SetButtonColor();

        button = GetComponent<Button>();
        button.onClick.AddListener(() => ColorManager.Instance.SetColorState(buttonColor));
    }

    void SetButtonColor()
    {
        switch(color)
        {
            case Colors.def:
                buttonColor = new DefaultColor();
                break;

            case Colors.red:
                buttonColor = new RedColor();
                break;
        }
    }
}
