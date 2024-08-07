using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.CurrentCoin++;
            GameManager.Instance.UIGame.UpdateCoinUI();

            AudioManager.Instance.PlaySFX("Star");
            Destroy(this.gameObject);
        }
    }

}
