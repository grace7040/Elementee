using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEffect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<MonsterController>().SetOnDieByGreenPlayer();
        }
    }
}
