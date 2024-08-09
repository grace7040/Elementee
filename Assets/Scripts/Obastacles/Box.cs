using UnityEngine;

public class Box : MonoBehaviour
{
    int _life = 2;
    bool _isDestroyed = false;
    bool _canHit = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("WeaponB") || collision.gameObject.CompareTag("WeaponThrow") || collision.gameObject.CompareTag("WeaponOrange") || collision.gameObject.CompareTag("WeaponYellow"))
        {
            if (!_canHit || _isDestroyed) return;


            _life--;
            _canHit = false;
            var effect = ObjectPoolManager.Instance.GetGameObject("Box");
            effect.transform.position = collision.bounds.ClosestPoint(transform.position);

            AudioManager.Instance.PlaySFX("Box");

            this.CallOnDelay(0.1f, () => { _canHit = true; });
            if (_life < 1)
            {
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
