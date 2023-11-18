using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSetParent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }
}
