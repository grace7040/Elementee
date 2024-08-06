using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instacne.PlaySFX("CheckPoint");
            GameManager.Instance.RevivalPos = this.transform;
            //DataManager.Instance.JsonSave();
        }
    }
}