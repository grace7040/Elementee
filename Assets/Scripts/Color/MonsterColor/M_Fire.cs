using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Fire : MonoBehaviour
{
    void Start() {}

    void Update() {}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Map")
        {
            Destroy(gameObject);
        }
    }
}
