using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public ParticleSystem leafParticle;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (transform.position.x - col.transform.position.x > 0) 
        {
            GetComponent<Animator>().Play("MovingGrassL");
        }
        else 
        {
            GetComponent<Animator>().Play("MovingGrassR");
        }
        if (col.gameObject.CompareTag("Player")) AudioManager.Instacne.PlaySFX("GrassPass");
    }

    public void ApplyDamage()
    {
        var leaf = ObjectPoolManager.Instance.GetGo("LeafCut");
        leaf.transform.position = transform.position;

        Destroy(gameObject);
    }
}