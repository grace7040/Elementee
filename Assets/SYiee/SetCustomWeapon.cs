using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCustomWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = DrawManager.Instance.DrawbleObject.GetComponent<SpriteRenderer>().sprite;
    }

}
