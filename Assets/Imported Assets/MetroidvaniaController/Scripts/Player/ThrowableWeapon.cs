using UnityEngine;

public class ThrowableWeapon : PoolAble
{
    [SerializeField] 
    Colors WeaponColor;
    
    [HideInInspector]
    public Vector2 ThrowDirection;

    bool _isReleased;
    float _speed = 20f;
    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = GameManager.Instance.CurrentWeaponSpriteList[(int)WeaponColor];
    }

    void OnEnable()
    {
        _isReleased = false;
        //SetWeaponSprite();
        this.CallOnDelay(3f, ReleaseThrowableWeapon);
    }

    void FixedUpdate()
    {
        transform.Translate(_speed * Time.deltaTime * ThrowDirection);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            ReleaseThrowableWeapon();
        }
    }

    public void Throw(Vector3 playerPosition, float playerLocalScaleX)
    {
        _spriteRenderer.flipX = playerLocalScaleX < 0;
        transform.SetPositionAndRotation(playerPosition + new Vector3(playerLocalScaleX * 0.5f, 0.2f), Quaternion.identity);
        ThrowDirection = new Vector2(playerLocalScaleX, 0);
    }

    //void SetWeaponSprite()
    //{
    //    if(ColorManager.Instance.IsUsingBasicWeapon)
    //    {
    //        _spriteRenderer.sprite = DrawManager.Instance.BasicWeapon[(int)WeaponColor];
    //    }
    //    else
    //    {
    //        _spriteRenderer.sprite = DrawManager.Instance.WeaponCanvas[(int)WeaponColor];
    //    }
    //}

    void ReleaseThrowableWeapon()
    {
        if (_isReleased) return;

        ReleaseObject();
    }
}
