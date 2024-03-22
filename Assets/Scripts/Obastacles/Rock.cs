using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    int life = 3;
    bool isDestroyed = false;
    bool canHit = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("WeaponB") || collision.gameObject.CompareTag("WeaponThrow") || collision.gameObject.CompareTag("WeaponOrange"))
        {
            if (!canHit || isDestroyed) return;


            life--;
            canHit = false;
            var effect = ObjectPoolManager.Instance.GetGo("Rock");
            effect.transform.position = collision.bounds.ClosestPoint(transform.position);

            this.CallOnDelay(0.1f, () => { canHit = true; });
            if (life < 1)
            {
                DestroyObject();
                return;
            }
        }
    }

    void DestroyObject()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }
}
