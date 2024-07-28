using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour
{
    public GameObject[] randomObjects;
    public float posY = 2f;
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
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("WeaponB") || collision.gameObject.CompareTag("WeaponThrow") || collision.gameObject.CompareTag("WeaponOrange") || collision.gameObject.CompareTag("WeaponYellow")) 
        {
            isUsed = true;
            randomIdx = Random.Range(0, randomObjects.Length);

            anim.Play("RandomBox", -1, 0f);

            this.CallOnDelay(1f, () => { 
                Instantiate(randomObjects[randomIdx], new Vector3(transform.position.x, transform.position.y + posY, transform.position.z), Quaternion.identity, this.transform.parent);
                Destroy(gameObject, 0.5f);
            });
        }
    }
}
