using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BtnPushDoor : MonoBehaviour
{
    public Transform Door;
    public Transform DoorOpen;

    private Vector3 OriginalDoor;

    private bool isPush = false;

    private void Start()
    {
        OriginalDoor = Door.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Btn") && isPush == false)
        {
            isPush = true;
            Door.DOMove(DoorOpen.position, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Btn"))
        {
            Door.DOMove(OriginalDoor, 1f);
            isPush = false;
        }
    }


}
