using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 10; } }

    public GameObject ThrowableObject { get; set; }
    public GameObject CustomObject { get; set; }

    public Sprite Sprite { get; set; }

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

    public float pullForce = 5f; // ������� �� ������ ����
    public float throwForce = 20f; // ������ �� ������ ����
    private bool isHoldingEnemy = false; // ���� ������ �ִ��� ����
    private Rigidbody2D heldEnemyRigidbody; // ������ �ִ� ���� Rigidbody2D
    private Transform playerTransform; // �÷��̾��� Transform

    // �÷��̾�� �浹���� �� ȣ��Ǵ� �Լ�
    public void Attack(PlayerController player)
    {
        if (isHoldingEnemy)
        {
            // �̹� ������ �ִ� ���� �����ϴ�.
            ThrowHeldEnemy();
        }
        else
        {
            // �÷��̾�� �浹���� ��, ���� ������ �մϴ�.
            playerTransform = player.transform; // �÷��̾��� Transform ���
            PullClosestEnemy(player.transform);
        }
    }

    // ���� ����� ���� ������
    private void PullClosestEnemy(Transform playerTransform)
    {
        if (!isHoldingEnemy)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = Mathf.Infinity;
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
                isHoldingEnemy = true;
                heldEnemyRigidbody = closestEnemy.GetComponent<Rigidbody2D>();
            }
        }
    }

    // ������ �ִ� ���� �����ϴ�.
    private void ThrowHeldEnemy()
    {
        if (heldEnemyRigidbody != null)
        {
            isHoldingEnemy = false;
            Vector2 throwDirection = (playerTransform.position - heldEnemyRigidbody.transform.position).normalized;
            heldEnemyRigidbody.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
            heldEnemyRigidbody = null;
        }
    }
}
