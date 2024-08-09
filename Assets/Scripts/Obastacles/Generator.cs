using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generator : MonoBehaviour
{
    public int count;
    public float posY = -1f;
    public string objectName;
    public GameObject Object;

    [HideInInspector]
    public TextMeshPro CountTxt;
    public TextMeshPro NameTxt;


    int randomIdx;
    bool isGenerating;

    private void Start()
    {
        CountTxt.text = count.ToString();
        NameTxt.text = objectName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isGenerating) return;

        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("WeaponB") || collision.gameObject.CompareTag("WeaponThrow") || collision.gameObject.CompareTag("WeaponOrange") || collision.gameObject.CompareTag("WeaponYellow"))
        {
            if(count > 0)
            {
                AudioManager.Instance.PlaySFX("Generator");
                isGenerating = true;
                count--;
                CountTxt.text = count.ToString();
                this.CallOnDelay(0.5f, () => {
                    
                    Instantiate(Object, new Vector3(transform.position.x, transform.position.y+ posY, transform.position.z), Quaternion.identity, this.transform.parent);
                    isGenerating = false;
                });  
            }
        }
    }
}
