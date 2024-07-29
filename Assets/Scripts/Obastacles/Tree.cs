using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject Wood;

    int _life = 5;
    bool _isDestroyed = false;
    bool _canHit = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("WeaponB") || collision.gameObject.CompareTag("WeaponThrow") || collision.gameObject.CompareTag("WeaponOrange"))
        {
            if (!_canHit || _isDestroyed) return;

            _life--;
            _canHit = false;
            var effect = ObjectPoolManager.Instance.GetGo("Tree");
            effect.transform.position = collision.bounds.ClosestPoint(transform.position);

            this.CallOnDelay(0.1f, () => { _canHit = true; });
            if (_life < 1)
            {
                Instantiate(Wood, new Vector3(transform.position.x, transform.position.y -2, transform.position.z), Quaternion.identity, this.transform.parent);
                DestroyObject();
                return;
            }
        }
    }

    void DestroyObject()
    {
        _isDestroyed = true;
        Destroy(gameObject);
    }
}
