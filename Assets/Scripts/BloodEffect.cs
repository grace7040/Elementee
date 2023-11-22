using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Untagged"))
        {
            //Debug.Log("Ãæµ¹: "+collision.name);
            AudioManager.Instacne.PlaySFX("PaintPop");
            var bulletGo = ObjectPoolManager.Instance.GetGo();
            bulletGo.transform.position = collision.bounds.ClosestPoint(transform.position);
        }
        
    }

}
