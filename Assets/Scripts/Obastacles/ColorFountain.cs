using UnityEngine;

public class ColorFountain : MonoBehaviour
{
    public Colors fountainColor;

    BoxCollider2D _boxcollider;
    int _damage = 10;
    int _healAmount = 10;
    PlayerController _player;

    private void Start()
    {
        _boxcollider = GetComponent<BoxCollider2D>();
        GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetColor(fountainColor);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player = collision.gameObject.GetComponent<PlayerController>();
            if (_player.myColor == fountainColor)
                _player.HealByFountain(_healAmount);
            else
                _player.TakeDamage(_damage, this.transform.position);

        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<MonsterController>().MyColor != fountainColor)
            {
                collision.gameObject.GetComponent<MonsterController>().TakeDamage(_damage, this.transform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var blood = ObjectPoolManager.Instance.GetColorBlood(fountainColor);
            blood.transform.position = transform.position;
        }
    }


}