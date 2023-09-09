using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ColorState color;
    public ColorState Color
    {
        get { return color; }
        set { color = value; }
    }


    private void Start()
    {
        Color = new DefaultColor();
    }

    private void Update()
    {

        Debug.Log($"Color: {Color.ToString()}, jumpForce: {Color.JumpForce}");
        Color.Attack();
    }

}
