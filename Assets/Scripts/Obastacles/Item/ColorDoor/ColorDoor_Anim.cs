using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDoor_Anim : MonoBehaviour
{
    public GameObject ColorDoor;
    public bool L_Door;
    private Animator anim;


    private void Start()
    {
        anim = ColorDoor.GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // �÷��̾�� �ٸ� ���� ��츸 ����
            if (collision.gameObject.GetComponent<PlayerController>().myColor != ColorDoor.GetComponent<ColorDoor>().doorColor)
            {
                // �� �ݶ��̴� ���ֱ�
                ColorDoor.GetComponent<ColorDoor>().boxcollider.enabled = true;

                if (L_Door)
                    anim.Play("Door_L", -1, 0.2f);
                else
                    anim.Play("Door_R", -1, 0.2f);
            }  
        }
    }



}
