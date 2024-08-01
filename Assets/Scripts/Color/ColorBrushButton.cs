using UnityEngine;
using UnityEngine.UI;


public class ColorBrushButton : MonoBehaviour
{
    public Colors color;

    Button _button;

    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => DrawManager.Instance.SetBrushColor(color));
    }

}
