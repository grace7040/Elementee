using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour
{
    public GameObject[] randomObjects;
    bool isUsed = false;
    int randomIdx;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isUsed) return;
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("WeaponB"))
        {
            isUsed = true;
            randomIdx = Random.Range(0, randomObjects.Length);
            Instantiate(randomObjects[randomIdx], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
