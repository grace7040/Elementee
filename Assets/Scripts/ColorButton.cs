using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ColorButton : MonoBehaviour
{
    public enum Colors
    {
        def, red
    }

    public Colors color;

    ColorState buttonColor;

    private Button button;
    public TMP_Text text;

    private void Start()
    {
        SetButtonColor();

        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            ColorManager.Instance.SetColorState(buttonColor);
            text.text = buttonColor.ToString();
        });
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
