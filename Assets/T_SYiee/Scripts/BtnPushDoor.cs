using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BtnPushDoor : MonoBehaviour
{
    public Transform Door;
    public Transform DoorOpen;
    public Transform BtnClicked;

    private Vector3 OriginalDoor;
    private Vector3 OriginalBtn;

    private bool isPush = false;

    private void Start()
    {
        OriginalBtn = gameObject.transform.position;
        OriginalDoor = Door.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPush == false)
        {
            isPush = true;
            gameObject.transform.DOMove(BtnClicked.position, 0.2f);
            Door.DOMove(DoorOpen.position, 2f);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.transform.DOMove(OriginalBtn, 0.2f);
            Door.DOMove(OriginalDoor, 2f);
            isPush = false;

        }
    }


}
