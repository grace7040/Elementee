using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generator : MonoBehaviour
{
    public int count;
    public string objectName;
    public GameObject Object;

    [HideInInspector]
    public TextMeshPro CountTxt;
    public TextMeshPro NameTxt;


    int randomIdx;

    private void Start()
    {
        CountTxt.text = count.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("WeaponB") || collision.gameObject.CompareTag("WeaponThrow") || collision.gameObject.CompareTag("WeaponOrange"))
        {
            if(count > 0)
            {
                count--;
                CountTxt.text = count.ToString();
                this.CallOnDelay(0.5f, () => {
                    
                    Instantiate(Object, new Vector3(transform.position.x, transform.position.y -1f, transform.position.z), Quaternion.identity, this.transform.parent);
                });  
            }
        }
    }
}
