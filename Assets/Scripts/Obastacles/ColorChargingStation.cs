using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChargingStation : MonoBehaviour
{
    public Colors paintColor;
    public GameObject[] otherPaintObjects;
    private static object _lock = new object();
    static bool isSelected = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (isSelected)
                return;

            lock (_lock)
            {
                isSelected = true;
                foreach (GameObject paint in otherPaintObjects)
                    paint.SetActive(false);
            }
               
            GetComponent<BoxCollider2D>().isTrigger = true;

            switch (paintColor)
            {
                case Colors.red:
                    ColorManager.Instance.HasRed = true;
                    break;
                case Colors.blue:
                    ColorManager.Instance.HasBlue = true;
                    break;
                case Colors.yellow:
                    ColorManager.Instance.HasYellow = true;
                    break;
            }

            this.CallOnDelay(3f, ()=> { gameObject.SetActive(false); isSelected = false; });
        }
    }
}
