using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 15; } }
    public bool WallSliding { get { return false; } }

    ////Temporal Setting : Red Color Attack -> Throw obj
    //public void Attack(PlayerController player)
    //{
    //    GameObject throwableWeapon = Instantiate(Resources.Load("Projectile"),
    //        player.transform.position + new Vector3(player.transform.localScale.x * 0.5f, -0.2f),
    //        Quaternion.identity) as GameObject;
    //    throwableWeapon.GetComponent<SpriteRenderer>().sprite = this.sprite;
    //    Vector2 direction = new Vector2(player.transform.localScale.x, 0);
    //    throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
    //    throwableWeapon.name = "ThrowableWeapon";
    //}

    public float pullForce = 2.0f; // 끌어당기는 힘 조절용 변수
    public float throwForce = 15f; // 던지는 힘 조절용 변수
    public bool isHoldingEnemy = false; // 적을 가지고 있는지 여부
    private Rigidbody2D heldEnemyRigidbody; // 가지고 있는 적의 Rigidbody2D
    private Transform playerTransform; // 플레이어의 Transform
    private GameObject Enemy;

    public void Attack(PlayerController player)
    {
        player.canAttack = true;
        foreach (Transform child in player.transform)
        {
            if (child.name == "WeaponPosition")
            {
                foreach (Transform child_ in child.transform)
                {
                    if (child_.name.Contains("M_"))
                    {
                        isHoldingEnemy = true;
                    }
                }
            }
            //Debug.Log(isHoldingEnemy);
        }

        Debug.Log(player.canAttack);
        if (isHoldingEnemy)
        {
            Debug.Log("던짐");
            ThrowHeldEnemy();
        }
        else
        {
            Debug.Log("당김");
            playerTransform = player.transform;
            PullClosestEnemy(playerTransform);
            //player.canAttack = true;
        }
    }

    // 가장 가까운 적을 끌어당김
    private void PullClosestEnemy(Transform playerTransform)
    {
        if (!isHoldingEnemy)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = 7.5f;
            Transform closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(playerTransform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            if (closestEnemy != null)
            {
                //isHoldingEnemy = true; // 타이밍이 문제
                heldEnemyRigidbody = closestEnemy.GetComponent<Rigidbody2D>();
                //closestEnemy.GetComponent<CapsuleCollider2D>().enabled = false;
                Enemy = closestEnemy.gameObject;

                closestEnemy.GetComponent<Animator>().enabled = false;
                closestEnemy.GetComponent<MonsterController>().enabled = false;
                closestEnemy.GetComponent<Rigidbody2D>().mass = 0.1f;
                closestEnemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                closestEnemy.GetComponent<CapsuleCollider2D>().isTrigger = true;
                closestEnemy.AddComponent<BloodEffect>();
                //heldEnemyRigidbody.isKinematic = true;
                //heldEnemyRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

                //Debug.Log(playerTransform.position);
                //Enemy.transform.Translate(playerTransform.position * pullForce * Time.deltaTime);
                //Debug.Log(Enemy.transform.position);

                //StartCoroutine(Pull());

                //IEnumerator Pull()
                //{
                //    Vector2 throwDirection = (playerTransform.position - heldEnemyRigidbody.transform.position).normalized;
                //    heldEnemyRigidbody.AddForce(throwDirection * pullForce, ForceMode2D.Impulse);

                //    yield return null;
                //}

                Vector2 throwDirection = (playerTransform.position - heldEnemyRigidbody.transform.position).normalized;
                heldEnemyRigidbody.AddForce(throwDirection * pullForce, ForceMode2D.Impulse);
            }
        }
    }

    // 가지고 있는 적을 던집니다.
    private void ThrowHeldEnemy()
    {
        Rigidbody2D rb = Enemy.AddComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 하위 객체(child)의 Transform을 얻어옵니다.
            Transform childTransform = Enemy.gameObject.transform;

            //childTransform.GetComponent<CapsuleCollider2D>().enabled = true;

            // 부모-자식 관계를 해제합니다.
            childTransform.SetParent(null);

            isHoldingEnemy = false;
            //Enemy.GetComponent<CapsuleCollider2D>().isTrigger = false;
            Enemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            //Enemy.GetComponent<Rigidbody2D>().mass = 0.0f;
            //rb.velocity= Vector2.zero;

            rb.gameObject.GetComponent<OB_VerticlaMovement>().enabled = false;

            Vector2 throwDirection = (rb.transform.position - playerTransform.position).normalized;
            rb.velocity = throwDirection * throwForce;
            rb.gameObject.tag = "WeaponB"; // 태그 변경
            heldEnemyRigidbody = null;

            Destroy(Enemy.gameObject, 3.0f);
        }
        //if (heldEnemyRigidbody != null)
        //{
        //    isHoldingEnemy = false;
        //    Vector2 throwDirection = (heldEnemyRigidbody.transform.position - playerTransform.position).normalized;
        //    heldEnemyRigidbody.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        //    heldEnemyRigidbody = null;
        //}
    }
}
