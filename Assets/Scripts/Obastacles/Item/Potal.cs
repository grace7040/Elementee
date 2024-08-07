using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    // Start is called before the first frame update\
    bool IsClear = false;
    void Awake()
    {
        GameManager.Instance.CurrentPotal = this.gameObject;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsClear)
        {
            if (collision.CompareTag("Player"))
            {
                // Sound
                AudioManager.Instance.PlaySFX("Star");
                IsClear = true;
                Invoke("EndGame", 2f);
            }
        }
        
    }

    public void EndGame()
    {
        GameManager.Instance.GameWin();
    }

}
