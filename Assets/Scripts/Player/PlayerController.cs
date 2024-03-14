using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using DG.Tweening;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    private IColorState color;
    public IColorState Color
    {
        get { return color; }
        set {
            color = value;
            jumpForce = value.JumpForce; // jumpForce: player의 jump force, JumpForce : Color의 jumpforce
            damage = value.Damage;
            m_WallSliding = value.WallSliding;
            coolTime = value.CoolTime;
        }
    }

    public Colors myColor = Colors.def;
    public GameObject cam;

    [Header("Weapon")]
    public SpriteRenderer[] colorWeapons;
    public GameObject red_Weapon;
    public GameObject orange_Weapon;
    public GameObject purple_Weapon;
    public GameObject green_Weapon;
    public GameObject blue_Weapon;
    public GameObject black_Weapon;
    public GameObject yellow_WeaponEffect;
    public GameObject orange_WeaponEffect;

    [Header("ParticleSystem")]
    public ParticleSystem particleJumpUp; //Trail particles
    public ParticleSystem particleJumpDown; //Explosion particles
    public GameObject healEffect;
    public GameObject purpleEffect;

    [Header("Attack")]
    public Transform attackCheck;
    public bool isAttack = false; //attack btn input
    public float coolTime;
    public float knockBackForce = 10f;
    public bool invincible = false;
    public bool canHealOnFountain = true;
    //private bool isTimeToCheck = false; 

    [Header("Player Properties")]
    public int currentHealth;
    public SpriteRenderer faceSprite;
    [HideInInspector] public Rigidbody2D m_Rigidbody2D;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer playerSprite;

    [Header("Movement Customizing")]
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] public float m_MoveSpeed = 10f;
    [SerializeField] private float m_DashForce = 25f;
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private bool m_WallSliding = false;                         // 플레이어 벽타기 할 수 있는지 없는지

    [Header("Collision Checking")]
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_WallCheck;                             //Posicion que controla si el personaje toca una pared

    //Attack
    [HideInInspector] public bool doAttack = false; //attack input
    [HideInInspector] public bool canAttack = true;

    //Health
    [HideInInspector] public int maxHealth = 100;
    [HideInInspector] public int damage = 0;

    // weapon position
    public GameObject WeaponPosition;

    [Header("Events")]
    [Space]

    public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private float jumpForce;                          // Amount of force added when the player jumps.

    public bool m_Grounded;
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded

    private bool m_IsWall = false; //If there is a wall in front of the player
    private bool isDashing = false; //If player is dashing
    private float prevVelocityX = 0f;

    private bool limitVelOnWallJump = false; //For limit wall jump distance with low fps
    private float jumpWallDistX = 0; //Distance between player and wall
    private float jumpWallStartX = 0;
    private bool isWallSliding = false; //If player is sliding in a wall

    private bool canMove = true; //If player can move
    private bool canDash = true;

    //Flip
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    //Move
    private float limitFallSpeed = 25f; // Limit fall speed
    private Vector3 velocity = Vector3.zero;
    //-점프
    private bool canDoubleJump = true; //If player can double jump
    //-벽타기
    private bool oldWallSlidding = false; //If player is sliding in a wall in the previous frame
    private bool canCheck = false; //For check if player is wallsliding
    //로프
    private FixedJoint2D fixJoint;
    private bool isRope = false;


    // Black
    private float pullForce = 10f; // 끌어당기는 힘 조절용 변수
    private float throwForce = 15f; // 던지는 힘 조절용 변수
    public bool isHoldingEnemy = false; // 적을 가지고 있는지 여부
    private Rigidbody2D heldEnemyRigidbody; // 가지고 있는 적의 Rigidbody2D
    private GameObject Enemy;

    private void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();


        if(GameManager.Instance.sponPos != null)
            transform.position = GameManager.Instance.sponPos.position;

        // 얼굴 넣기
        faceSprite.sprite = GameManager.Instance.playerFace;

        //Health initiallize
        currentHealth = maxHealth;
        GameManager.Instance.playerMAXHP = maxHealth;
        GameManager.Instance.playerHP = maxHealth;


        ColorManager.Instance.SetColorState(Colors.def);
        //m_Rigidbody2D = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        fixJoint = GetComponent<FixedJoint2D>();

        if (OnFallEvent == null) // 이건 왜하지?
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        //TEST CODESZ
        //ColorManager.Instance.SetColorState(Colors.blue);

        ColorManager.Instance.HasBlue = true;
        ColorManager.Instance.HasRed = true;
        ColorManager.Instance.HasYellow = true;
    }

    private void Update()
    {
        if ( (Input.GetKeyDown(KeyCode.X) || isAttack) && canAttack)
        {
            doAttack = true;
            canAttack = false;
            isAttack = false;

        }

        if (myColor == Colors.black)
        {
            if (isHoldingEnemy)
            {
                Enemy.transform.localPosition = new Vector2(0, 0);
            }
        }
        else if (myColor != Colors.black)
        {
            Destroy(Enemy, 0.1f);
        }
    }

    public void AttackDown()
    {
        isAttack = true;
    }

    public void AttackUp()
    {
        isAttack = false;
    }

    private void FixedUpdate()
    {
        if (doAttack)
        {
            doAttack = false;
            Color.Attack(this);
        }

        //colliders : 닿아있는 바닥수만큼 존재, 공중에 떠있으면 0개 바닥에 닿아있으면 1개

        if (!isRope)
        {
            //땅에 닿아 있는지 판별하는 변수
            bool wasGrounded = m_Grounded;
            m_Grounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++) // 이 for문이 왜 필요하지
            {
                //gameobject = plsyer, colliders[i].gameObject = player와 접촉하고 있는 obj
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    if (!wasGrounded)
                    {
                        OnLandEvent.Invoke();
                        if (!m_IsWall && !isDashing)
                            particleJumpDown.Play();
                        canDoubleJump = true;
                        if (m_Rigidbody2D.velocity.y < 0f)
                            limitVelOnWallJump = false;
                    }
                }
            }
        }
        


        m_IsWall = false;
        if (!m_Grounded) //땅에 닿아있지 않을 때 
        {
            OnFallEvent.Invoke(); //chaingin animation -> isjumping : true
            Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < collidersWall.Length; i++)
            {
                if (collidersWall[i].gameObject != null) //벽에 닿아있다면
                {
                    isDashing = false;
                    m_IsWall = true;
                }
            }
            prevVelocityX = m_Rigidbody2D.velocity.x; // 현재 속도를 저장
        }


        if (limitVelOnWallJump) // 벽과 관련된 듯한데 일단 패스
        {
            if (m_Rigidbody2D.velocity.y < -0.5f) // 이게 몰까
                limitVelOnWallJump = false;
            jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
            if (jumpWallDistX < -0.5f && jumpWallDistX > -1f)
            {
                canMove = true;
            }
            else if (jumpWallDistX < -1f && jumpWallDistX >= -2f)
            {
                canMove = true;
                m_Rigidbody2D.velocity = new Vector2(10f * transform.localScale.x, m_Rigidbody2D.velocity.y);
            }
            else if (jumpWallDistX < -2f)
            {
                limitVelOnWallJump = false;
                m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
            }
            else if (jumpWallDistX > 0)
            {
                limitVelOnWallJump = false;
                m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
            }
        }
    }

    public void BlackPull()
    {
        StartCoroutine(PullCoroutine());
    }

    private IEnumerator PullCoroutine()
    {
        if (!isHoldingEnemy)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = 7.5f;
            Transform closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            if (closestEnemy != null)
            {
                heldEnemyRigidbody = closestEnemy.GetComponent<Rigidbody2D>();
                Enemy = closestEnemy.gameObject;

                //closestEnemy.GetComponent<Animator>().enabled = false;
                closestEnemy.GetComponent<MonsterController>().isDie = true;
                if (closestEnemy.GetComponent<MonsterController>().myColor == Colors.yellow) closestEnemy.GetComponent<M_Yellow>().voltObject.SetActive(false);
                else if (closestEnemy.GetComponent<MonsterController>().myColor == Colors.red) closestEnemy.GetComponent<M_Red>().fireObject.SetActive(false);
                closestEnemy.GetComponent<MonsterController>().enabled = false;
                closestEnemy.GetComponent<Rigidbody2D>().mass = 0.0f;
                closestEnemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                closestEnemy.GetComponent<CapsuleCollider2D>().isTrigger = true;
                //closestEnemy.AddComponent<BloodEffect>();

                float distance = Vector2.Distance(Enemy.transform.position, transform.position);

                while (!isHoldingEnemy)
                {
                    Vector2 throwDirection = (transform.position - Enemy.transform.position).normalized;
                    Enemy.transform.Translate(throwDirection * pullForce * Time.deltaTime);

                    yield return null;
                }
            }
        }
        yield return new WaitForSeconds(0f);
    }

    public void BlackThrow()
    {
        Rigidbody2D rb = Enemy.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Transform childTransform = Enemy.gameObject.transform;

            childTransform.SetParent(null);

            isHoldingEnemy = false;
            Enemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

            rb.gameObject.GetComponent<OB_VerticlaMovement>().enabled = false;

            Vector2 throwDirection = (rb.transform.position - transform.position).normalized;
            rb.velocity = throwDirection * throwForce;
            rb.gameObject.tag = "WeaponB";
            heldEnemyRigidbody = null;

            Enemy.GetComponent<MonsterController>().Die();
        }
    }

    public void SetCustomWeapon()
    {
        colorWeapons[(int)myColor].sprite = DrawManager.Instance.sprites[(int)myColor];

        // Yellow 경우, 자식들에도 sprite 할당이 필요함
        if(myColor == Colors.yellow)
        {
            for(int i = 0; i < 4; i++)
            {
                yellow_WeaponEffect.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = colorWeapons[(int)Colors.yellow].sprite;
            }
        }
    }

    public void SetBasicWeapon()
    {
        colorWeapons[(int)myColor].sprite = DrawManager.Instance.Basic_Sprites[(int)myColor];

        if (myColor == Colors.yellow)
        {
            for (int i = 0; i < 4; i++)
            {
                yellow_WeaponEffect.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = colorWeapons[(int)Colors.yellow].sprite;
            }
        }
    }

    public void ShowWeapon()
    {
        colorWeapons[(int)myColor].gameObject.SetActive(true);
    }

    public void HideWeapon()
    {
        colorWeapons[(int)myColor].gameObject.SetActive(false);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        Vector3 yellowScale = yellow_WeaponEffect.transform.localScale;
        yellowScale.x *= -1;
        yellow_WeaponEffect.transform.localScale = yellowScale;
    }

    public void Move(float move, bool jump, bool dash)
    {
        if (canMove)
        {
            if (dash && canDash && !isWallSliding)
            {
                //m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_DashForce, 0f));
                StartCoroutine(DashCooldown());
            }
            // If crouching, check to see if the character can stand up
            if (isDashing)
            {
                m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
            }
            //only control the player if grounded or airControl is turned on
            else if (m_Grounded || m_AirControl)
            {
                if (m_Rigidbody2D.velocity.y < -limitFallSpeed)
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -limitFallSpeed);
                // Move the character by finding the target velocity
                Vector3 targetVelocity = new Vector2(move * m_MoveSpeed, m_Rigidbody2D.velocity.y);
                // And then smoothing it out and applying it to the character
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight && !isWallSliding)
                {
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight && !isWallSliding)
                {
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump)
            {
                // Add a vertical force to the player.
                animator.SetBool("IsJumping", true);
                animator.SetBool("JumpUp", true);
                m_Grounded = false;
                m_Rigidbody2D.velocity = Vector2.zero;
                m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce));
                canDoubleJump = true;
                particleJumpDown.Play();
                particleJumpUp.Play();
            }
            else if (!m_Grounded && jump && canDoubleJump && !isWallSliding)
            {
                canDoubleJump = false;
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
                m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce / 1.2f));
                animator.SetBool("IsDoubleJumping", true);
            }

            //Wall Sliding

            else if (m_WallSliding && m_IsWall && !m_Grounded)
            {
                if (!oldWallSlidding && m_Rigidbody2D.velocity.y < 0 || isDashing)
                {
                    isWallSliding = true;
                    m_WallCheck.localPosition = new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
                    Flip();
                    StartCoroutine(WaitToCheck(0.1f));
                    canDoubleJump = true;
                    animator.SetBool("IsWallSliding", true);
                }
                isDashing = false;

                if (isWallSliding)
                {
                    if (move * transform.localScale.x > 0.1f)
                    {
                        StartCoroutine(WaitToEndSliding());
                    }
                    else
                    {
                        oldWallSlidding = true;
                        m_Rigidbody2D.velocity = new Vector2(-transform.localScale.x * 2, -5);
                    }
                }

                if (jump && isWallSliding)
                {
                    animator.SetBool("IsJumping", true);
                    animator.SetBool("JumpUp", true);
                    m_Rigidbody2D.velocity = new Vector2(0f, 0f);
                    m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * jumpForce * 1.2f, jumpForce));
                    jumpWallStartX = transform.position.x;
                    limitVelOnWallJump = true;
                    canDoubleJump = true;
                    isWallSliding = false;
                    animator.SetBool("IsWallSliding", false);
                    oldWallSlidding = false;
                    m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                    canMove = false;
                }
                else if (dash && canDash)
                {
                    isWallSliding = false;
                    animator.SetBool("IsWallSliding", false);
                    oldWallSlidding = false;
                    m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                    canDoubleJump = true;
                    StartCoroutine(DashCooldown());
                }
            }
            else if (isWallSliding && !m_IsWall && canCheck)
            {
                isWallSliding = false;
                animator.SetBool("IsWallSliding", false);
                oldWallSlidding = false;
                m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                canDoubleJump = true;
            }

        }
    }

    IEnumerator DashCooldown()
    {
        animator.SetBool("IsDashing", true);
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(0.1f); // dash 지속시간
        isDashing = false;
        yield return new WaitForSeconds(0.5f); // dash cooltime
        canDash = true;
    }

    IEnumerator WaitToCheck(float time)
    {
        canCheck = false;
        yield return new WaitForSeconds(time);
        canCheck = true;
    }

    IEnumerator WaitToEndSliding()
    {
        yield return new WaitForSeconds(0.1f);
        canDoubleJump = true;
        isWallSliding = false;
        animator.SetBool("IsWallSliding", false);
        oldWallSlidding = false;
        m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
    }

    public void Die()
    {
        Debug.Log("Die");
        Destroy(gameObject);

    }

    public void TakeDamage(int damage, Vector3 enemyPos)
    {
        if(!invincible)
        {
            //animation on 
            animator.SetBool("Hit", true);
            //health --
            currentHealth -= damage;
            Debug.Log($"플레이어 대미지: {damage}");

            if (currentHealth > 100)
                currentHealth = 100;

            //GameManager에 Health 저장
            GameManager.Instance.playerHP = currentHealth;
            //넉백
            Vector2 damageDir = Vector3.Normalize(transform.position - enemyPos) * 40f;
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Rigidbody2D.AddForce(damageDir * knockBackForce);

            if (currentHealth <= 0)
            {
                //GameManager.Instance.GameOver();
                StartCoroutine(WaitToDead());
                //Die();
            }
            else
            {
                invincible = true;
                playerSprite.DOFade(0.2f, 0.25f).SetLoops(4, LoopType.Yoyo);
                this.CallOnDelay(1f, () => { invincible = false; });
            }
        }

    }

    public void Heal(int health)
    {
        currentHealth += health;
        AudioManager.Instacne.PlaySFX("Heal");
        if (currentHealth > 100)
            currentHealth = 100;

        GameManager.Instance.playerHP = currentHealth;
        healEffect.SetActive(true);
        this.CallOnDelay(1f, () => { healEffect.SetActive(false); });
    }

    public void HealWithFountain(int health)
    {
        if (!canHealOnFountain) return;

        Heal(health);
        canHealOnFountain = false;
        this.CallOnDelay(1f, () => { canHealOnFountain = true; });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(collision.gameObject.GetComponent<MonsterController>().damage, collision.gameObject.transform.position);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(collision.gameObject.GetComponent<MonsterController>().damage, collision.gameObject.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rope"))
        {
            if (isRope) return;
            fixJoint.enabled = true;
            fixJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
            isRope = true;

            m_Grounded = true;

        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (myColor == Colors.black)
            {
                if (!collision.gameObject.GetComponent<MonsterController>().isActiveAndEnabled)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    collision.gameObject.GetComponent<OB_VerticlaMovement>().enabled = true;

                    Transform parentTransform = WeaponPosition.transform;
                    Transform childTransform = collision.gameObject.transform;
                    childTransform.SetParent(parentTransform);

                    isHoldingEnemy = true;
                }
            }
        }
        else if (collision.gameObject.CompareTag("EnemyWeapon"))
        {
            TakeDamage(15, collision.gameObject.transform.position);
            //Destroy(collision.gameObject, 0.1f);
        }
        else if (collision.gameObject.CompareTag("EnemyFarWeapon"))
        {
            TakeDamage(10, collision.gameObject.transform.position);
            Destroy(collision.gameObject, 0.1f);
        }
    }
    public void RopeOut()
    {
        if (!isRope) return;

        fixJoint.connectedBody = null;
        fixJoint.enabled = false;

        this.CallOnDelay(0.1f, ()=> { isRope = false; });
    }

    IEnumerator WaitToDead()
    {
        animator.SetBool("IsDead", true);
        canMove = false;
        invincible = true;
        yield return new WaitForSeconds(0.4f);
        m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        yield return new WaitForSeconds(1.1f);
        // 새롭게 씬 로드할 코드 추가

        GameManager.Instance.GameOver();
    } 

    public void PurpleAttackEffect()
    {
        StartCoroutine(Purple_Effect_Set_Active());
    }

    IEnumerator Purple_Effect_Set_Active()
    {
        Debug.Log("Purple");
        purpleEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        purpleEffect.SetActive(false);

    }

}
