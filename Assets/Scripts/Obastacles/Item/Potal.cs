using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instance.Potal = this.gameObject;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Sound
            AudioManager.Instacne.PlaySFX("Star");

            Invoke("EndGame", 2f);
        }
    }

    public void EndGame()
    {
        GameManager.Instance.GameWin();
    }

}
