using UnityEngine;

public class Grass : MonoBehaviour
{
    public ParticleSystem leafParticle;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.x - collision.transform.position.x > 0) 
        {
            GetComponent<Animator>().Play("MovingGrassL");
        }
        else 
        {
            GetComponent<Animator>().Play("MovingGrassR");
        }
        if (collision.gameObject.CompareTag("Player")) AudioManager.Instacne.PlaySFX("GrassPass");

        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("WeaponB") || collision.gameObject.CompareTag("WeaponThrow") || collision.gameObject.CompareTag("WeaponOrange") || collision.gameObject.CompareTag("WeaponYellow"))
        {
            ApplyDamage();
        }
    }

    public void ApplyDamage()
    {
        var leaf = ObjectPoolManager.Instance.GetGameObject("LeafCut");
        leaf.transform.position = transform.position;

        Destroy(gameObject);
    }
}