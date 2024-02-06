using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackColor : MonoBehaviour, IColorState
{
    public float JumpForce { get { return 800f; } }
    public int Damage { get { return 100; } }
    public bool WallSliding { get { return false; } }
    public float CoolTime { get { return 0.5f; } }

    #region variables
    public float pullForce = 2.0f; // ������� �� ������ ����
    public float throwForce = 15f; // ������ �� ������ ����
    public bool isHoldingEnemy = false; // ���� ������ �ִ��� ����
    private Rigidbody2D heldEnemyRigidbody; // ������ �ִ� ���� Rigidbody2D
    private Transform playerTransform; // �÷��̾��� Transform
    private GameObject Enemy;
    #endregion

    public void Attack(PlayerController player)
    {
        player.CallOnDelay(CoolTime, () => { player.canAttack = true; });

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
        }

        if (isHoldingEnemy)
        {
            ThrowHeldEnemy();
            player.canAttack = false;
            AudioManager.Instacne.PlaySFX("BlackRelease");
        }
        else
        {
            playerTransform = player.transform;
            PullClosestEnemy(playerTransform);
            AudioManager.Instacne.PlaySFX("Black");
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
                heldEnemyRigidbody = closestEnemy.GetComponent<Rigidbody2D>();
                Enemy = closestEnemy.gameObject;

                closestEnemy.GetComponent<Animator>().enabled = false;
                closestEnemy.GetComponent<MonsterController>().enabled = false;
                closestEnemy.GetComponent<Rigidbody2D>().mass = 0.1f;
                closestEnemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                closestEnemy.GetComponent<CapsuleCollider2D>().isTrigger = true;
                closestEnemy.AddComponent<BloodEffect>();               

                Vector2 throwDirection = (playerTransform.position - heldEnemyRigidbody.transform.position).normalized;
                heldEnemyRigidbody.AddForce(throwDirection * pullForce, ForceMode2D.Impulse);
            }
        }
    }

    // ������ �ִ� ���� ����
    private void ThrowHeldEnemy()
    {
        Rigidbody2D rb = Enemy.AddComponent<Rigidbody2D>();

        if (rb != null)
        {
            // ���� ��ü�� Transform ���
            Transform childTransform = Enemy.gameObject.transform;

            // �θ�-�ڽ� ���� ����
            childTransform.SetParent(null);

            isHoldingEnemy = false;
            Enemy.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

            rb.gameObject.GetComponent<OB_VerticlaMovement>().enabled = false;

            Vector2 throwDirection = (rb.transform.position - playerTransform.position).normalized;
            rb.velocity = throwDirection * throwForce;
            rb.gameObject.tag = "WeaponB"; // �±� ����
            heldEnemyRigidbody = null;

            // ���� ���� �� ȹ��
            SetPlayerColor(Enemy.GetComponent<MonsterController>().myColor);

            Destroy(Enemy.gameObject, 2.0f);
        }
    }

    public void SetPlayerColor(Colors mon_color)
    {
        switch (mon_color)
        {
            case Colors.def:
                break;
            case Colors.red:
                ColorManager.Instance.HasRed = true;
                break;
            case Colors.blue:
                ColorManager.Instance.HasBlue = true;
                break;
            case Colors.yellow:
                ColorManager.Instance.HasYellow = true;
                break;
        }
    }
}
