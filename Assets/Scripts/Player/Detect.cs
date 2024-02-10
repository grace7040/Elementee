using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{
    public GameObject WeaponPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!collision.gameObject.GetComponent<MonsterController>().isActiveAndEnabled)
            {
                // collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collision.gameObject.GetComponent<OB_VerticlaMovement>().enabled = true;

                Transform parentTransform = WeaponPosition.transform;
                Transform childTransform = collision.gameObject.transform;
                childTransform.SetParent(parentTransform);

                Destroy(collision.gameObject.GetComponent<Rigidbody2D>(), 0.1f);
            }
        }
    }
}
