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

    public float pullForce = 2.0f; // ������� �� ������ ����
    public float throwForce = 15f; // ������ �� ������ ����
    public bool isHoldingEnemy = false; // ���� ������ �ִ��� ����
    private Rigidbody2D heldEnemyRigidbody; // ������ �ִ� ���� Rigidbody2D
    private Transform playerTransform; // �÷��̾��� Transform
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
            Debug.Log("����");
            ThrowHeldEnemy();
        }
        else
        {
            Debug.Log("���");
            playerTransform = player.transform;
            PullClosestEnemy(playerTransform);
            //player.canAttack = true;
        }
    }

    // ���� ����� ���� ������
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
                //isHoldingEnemy = true; // Ÿ�̹��� ����
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

    // ������ �ִ� ���� �����ϴ�.
    private void ThrowHeldEnemy()
    {
        Rigidbody2D rb = Enemy.AddComponent<Rigidbody2D>();
        if (rb != null)
        {
            // ���� ��ü(child)�� Transform�� ���ɴϴ�.
            Transform childTransform = Enemy.gameObject.transform;

            //childTransform.GetComponent<CapsuleCollider2D>().enabled = true;

            // �θ�-�ڽ� ���踦 �����մϴ�.
            childTransform.SetParent(null);

            isHoldingEnemy = false;
            //Enemy.GetComponent<CapsuleCollider2D>().isTrigger = false;
            Enemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            //Enemy.GetComponent<Rigidbody2D>().mass = 0.0f;
            //rb.velocity= Vector2.zero;

            rb.gameObject.GetComponent<OB_VerticlaMovement>().enabled = false;

            Vector2 throwDirection = (rb.transform.position - playerTransform.position).normalized;
            rb.velocity = throwDirection * throwForce;
            rb.gameObject.tag = "WeaponB"; // �±� ����
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
