using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.MapCoin++;
            GameManager.Instance.UIGame.UpdateCoinUI();

            AudioManager.Instacne.PlaySFX("Star");
            Destroy(this.gameObject);
        }
    }

}
