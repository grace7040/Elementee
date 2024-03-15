using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearInteract : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grass"))
            collision.gameObject.SendMessage("ApplyDamage");
    }
}
