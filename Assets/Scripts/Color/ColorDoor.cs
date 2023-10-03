using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDoor : MonoBehaviour
{
    public Colors colors;
    BoxCollider2D boxcollider;

    IColorState doorColor;

    public int force = 100;

    private void Start()
    {
        SetDoorColor();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    void SetDoorColor()
    {
        switch (colors)
        {
            case Colors.def:
                doorColor = new DefaultColor();
                break;

            case Colors.red:
                doorColor = new RedColor();
                break;

            case Colors.yellow:
                doorColor = new YellowColor();
                break;

            case Colors.blue:
                doorColor = new BlueColor();
                break;

            case Colors.orange:
                doorColor = new OrangeColor();
                break;

            case Colors.green:
                doorColor = new GreenColor();
                break;

            case Colors.purple:
                doorColor = new PurpleColor();
                break;

            case Colors.black:
                doorColor = new BlackColor();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>().Color == doorColor)
        {
            boxcollider.enabled = false;
        }
        else
        {
            collision.rigidbody.AddForce((collision.transform.position - transform.position)*force, ForceMode.Impulse) ;
        }
    }

}
