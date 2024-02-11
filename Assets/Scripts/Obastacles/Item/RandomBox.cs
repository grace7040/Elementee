using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour
{
    public GameObject[] randomObjects;
    private Animator anim;

    bool isUsed = false;
    int randomIdx;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isUsed) return;
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("WeaponB") || collision.gameObject.CompareTag("WeaponThrow")) 
        {
            isUsed = true;
            randomIdx = Random.Range(0, randomObjects.Length);

            anim.Play("RandomBox", -1, 0f);

            this.CallOnDelay(1.5f, () => { 
                Instantiate(randomObjects[randomIdx], transform.position, Quaternion.identity, this.transform.parent);
                Destroy(gameObject);
            });
        }
    }
}
