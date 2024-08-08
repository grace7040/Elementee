using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChargingStation : MonoBehaviour
{
    public Colors paintColor;
    public GameObject[] otherPaintObjects;
    private static object _lock = new object();
    static bool isSelected = false;
    private Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (isSelected)
                return;

            if(otherPaintObjects.Length > 0)
            {
                lock (_lock)
                {
                    isSelected = true;
                    foreach (GameObject paint in otherPaintObjects)
                        paint.SetActive(false);
                }
            }
            
            GetComponent<BoxCollider2D>().isTrigger = true;
            ColorManager.Instance.HasColor(paintColor, true);

            anim.Play("Paint", -1, 0.4f);
            this.CallOnDelay(3f, ()=> { gameObject.SetActive(false); isSelected = false; });
        }
    }
}
