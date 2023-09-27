using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColorBrushButton : MonoBehaviour
{
    

    public Colors color;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => DrawManager.Instance.SetBrushColor(color));
    }

}
