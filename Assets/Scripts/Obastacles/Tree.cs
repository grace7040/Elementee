using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    int life = 5;
    public GameObject Wood;
    bool isDestroyed = false;
    bool canHit = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("WeaponB") || collision.gameObject.CompareTag("WeaponThrow") || collision.gameObject.CompareTag("WeaponOrange"))
        {
            if (!canHit || isDestroyed) return;


            life--;
            canHit = false;
            var effect = ObjectPoolManager.Instance.GetGo("Tree");
            effect.transform.position = collision.bounds.ClosestPoint(transform.position);

            this.CallOnDelay(0.1f, () => { canHit = true; });
            if (life < 1)
            {
                Instantiate(Wood, new Vector3(transform.position.x, transform.position.y -2, transform.position.z), Quaternion.identity, this.transform.parent);
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
