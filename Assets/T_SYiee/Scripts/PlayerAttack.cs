using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [Header("Weapon")]
    public SpriteRenderer[] ColorWeapons;
    public GameObject RedWeapon;
    public GameObject OrangeWeapon;
    public GameObject PurpleWeapon;
    public GameObject GreenWeapon;
    public GameObject BlueWeapon;
    public GameObject BlackWeapon;

    [Header("AttackEffect")]
    public GameObject YellowAttackEffect;
    public GameObject OrangeAttackEffect;
    public GameObject OrangeAttackWeaponCo;
    public GameObject PurpleAttackEffect;

    //Attack
    [HideInInspector] public bool CanAttack = true;

    // Black
    public GameObject WeaponPosition;
    bool _isHoldingEnemy = false;
    GameObject _enemy;
    Transform _closestEnemy;
    float _pullForce = 15f;
    float _throwForce = 15f;

    PlayerController _playerController;
    Rigidbody2D _rigidbody;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        ColorManager.Instance.InitPlayerAttack(this, OnOrangeAttacked, OnYellowAttacked, OnBlackAttacked, OnSetBlackColor);
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    public void AttackDown()
    {
        if (!CanAttack) return;

        _playerController.ColorState.Attack(transform.position, transform.localScale.x);
        if (_playerController.MyColor == Colors.Black) 
        {
            return;
        }

        CanAttack = false;
        this.CallOnDelay(_playerController.ColorState.CoolTime, () => { CanAttack = true; });   // ::TODO:: 노랑일 경우 예외처리 해야함
    }

    public void AttackUp()
    {
        //isAttack = false;
    }

    public void SetCustomWeapon()
    {
        ColorWeapons[(int)_playerController.MyColor].sprite = DrawManager.Instance.WeaponCanvas[(int)_playerController.MyColor];

        // Yellow 경우, 자식들에도 sprite 할당이 필요함
        if (_playerController.MyColor == Colors.Yellow)
        {
            for (int i = 0; i < 4; i++)
            {
                YellowAttackEffect.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = ColorWeapons[(int)Colors.Yellow].sprite;
            }
        }
    }

    public void SetBasicWeapon()
    {
        ColorWeapons[(int)_playerController.MyColor].sprite = DrawManager.Instance.BasicWeapon[(int)_playerController.MyColor];

        if (_playerController.MyColor == Colors.Yellow)
        {
            for (int i = 0; i < 4; i++)
            {
                YellowAttackEffect.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = ColorWeapons[(int)Colors.Yellow].sprite;
            }
        }
    }

    public void OnPurpleAttacked()
    {
        StartCoroutine(PurpleAttackEffectCo());
    }

    IEnumerator PurpleAttackEffectCo()
    {
        PurpleAttackEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        PurpleAttackEffect.SetActive(false);
    }

    void OnOrangeAttacked(float durationTime)
    {

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(new Vector2(0f, 500f));

        gameObject.layer = (int)Layer.OrangeAttack; // layer 변경으로 충돌 처리 막음
        OrangeAttackEffect.SetActive(true);

        this.CallOnDelay(0.1f, () =>
        {
            OrangeAttackWeaponCo.SetActive(true);
        });

        var originalMoveSpeed = _playerController.MoveSpeed;
        _playerController.MoveSpeed = 20f;

        this.CallOnDelay(durationTime, () =>
        {
            OrangeAttackEffect.SetActive(false);
            OrangeAttackWeaponCo.SetActive(false);
            gameObject.layer = (int)Layer.Player;
            _playerController.MoveSpeed = originalMoveSpeed;
        });
    }

    void OnYellowAttacked()
    {
        YellowAttackEffect.SetActive(true);
        YellowAttackEffect.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ColorWeapons[(int)Colors.Yellow].sprite;
    }

    void OnBlackAttacked()
    {
        if (_isHoldingEnemy)
        {
            BlackThrow();
            AudioManager.Instance.PlaySFX("BlackRelease");
        }
        else
        {
            BlackPull();
            AudioManager.Instance.PlaySFX("Black");
        }
    }

    void OnSetBlackColor()
    {
        StartCoroutine(OnSetBlackColorCo());
    }
    IEnumerator OnSetBlackColorCo()
    {

        while (true)
        {
            if (_playerController.MyColor == Colors.Black)
            {

                if (_isHoldingEnemy)
                {
                    // _enemy.transform.localPosition = new Vector2(0, 0);
                }
            }
            else if (_playerController.MyColor != Colors.Black)
            {
                Destroy(_enemy, 0.1f);
                break;
            }

            yield return 1;
        }
    }

    public void BlackPull()
    {
        StartCoroutine(PullCoroutine());
    }

    private IEnumerator PullCoroutine()
    {
        if (!_isHoldingEnemy && CanAttack)
        {
            FindClosestEnemy();

            if (_closestEnemy != null)
            {
                CanAttack = false;
                StartCoroutine(CheckNearbyEnemiesCoroutine());

                _enemy = _closestEnemy.gameObject;
                _enemy.transform.GetChild(0).Find("HPBar").gameObject.SetActive(false); // ::FIX:: delete Find() method
                _enemy.SendMessage("PulledByBlack");
                //closestEnemy.AddComponent<BloodEffect>();

                float distance = Vector2.Distance(_enemy.transform.position, transform.position);

                while (!_isHoldingEnemy)
                {
                    Vector2 throwDirection = (transform.position - _enemy.transform.position).normalized;
                    _enemy.transform.Translate(_pullForce * Time.deltaTime * throwDirection);

                    yield return null;
                }
            }
        }
        yield return new WaitForSeconds(0f);
    }

    private void FindClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 7.5f);
        float closestDistance = 7.5f;
        _closestEnemy = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                MonsterController enemyController = collider.GetComponent<MonsterController>();

                if (enemyController != null && !enemyController.IsDie)
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        _closestEnemy = collider.transform;
                    }
                }
            }
        }
    }


    public void BlackThrow()
    {
        Rigidbody2D _enemyRigidbody = _enemy.GetComponent<Rigidbody2D>();

        if (_enemyRigidbody != null)
        {
            Transform childTransform = _enemy.gameObject.transform;

            childTransform.SetParent(null);

            _isHoldingEnemy = false;
            _enemyRigidbody.gravityScale = 0.0f;

            Vector2 throwDirection = (_enemyRigidbody.transform.position - transform.position).normalized;
            _enemyRigidbody.velocity = throwDirection * _throwForce;
            _enemyRigidbody.gameObject.tag = "WeaponB";

            _enemy.GetComponent<MonsterController>().Die();
        }
    }

    private IEnumerator CheckNearbyEnemiesCoroutine()
    {
        while (!_isHoldingEnemy)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    HandleEnemyCollision(collider);
                    if (_isHoldingEnemy)
                    {
                        yield break;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleEnemyCollision(collision);
    }

    private void HandleEnemyCollision(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (_playerController.MyColor == Colors.Black)
            {
                if (!collision.gameObject.GetComponent<MonsterController>().isActiveAndEnabled)
                {
                    Rigidbody2D enemyRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                    enemyRigidbody.velocity = Vector2.zero;
                    enemyRigidbody.isKinematic = true;

                    Transform parentTransform = WeaponPosition.transform;
                    Transform childTransform = collision.gameObject.transform;
                    childTransform.SetParent(parentTransform);
                    childTransform.localPosition = Vector2.zero;

                    _isHoldingEnemy = true;
                    CanAttack = true;
                }
            }
        }
    }
}