using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star_Run : Star
{
    public GameObject target;

    public float speed = 2f;

    private bool follow = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    private void Update()
    {
        TargetConfirm();
    }

    private void UpdateTarget()
    {
        Collider2D []colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
        // ������ ���������� ������ ���� ������ �����Ϸ��� �ݰ� �̳��� �����ִ� �ݶ��̴�����
        // ��ȯ�ϴ� �Լ�

        if (colliders.Length > 0) // �ݶ��̴��� ������ 1�� �̻��̸�
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Player") // �ݶ��̴��� �±װ� �÷��̾��
                {
                    animator.SetBool("isAwake", true);
                    StartCoroutine(star_walk());
                    StartCoroutine(Wait());
                    //follow = true;
                    target = colliders[i].gameObject; // Ÿ�� ��ġ ����
                    break;

                    Invoke("Dead", 3f);
                }
               // else target = null;
            }
        }
    }

    private void TargetConfirm()
    {

        if (follow)
        {
            float dir = target.transform.position.x - transform.position.x; //2d�̱⿡ �¿츸 ����� (��x��ġ - targetx��ġ)
            dir = (dir < 0) ? 1 : -1; //�������� dir�� x�Ÿ��� -��� -1,�ƴϸ� 1
            transform.Translate(new Vector2(dir, 0) * speed * Time.deltaTime);
        }
    }

    public void Dead()
    {
        Destroy(this.gameObject);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        // ����Ʈ �ֱ�
    //        GameManager.Instance.totalScore += score;
    //        Destroy(this.gameObject);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ����Ʈ �ֱ�
            GameManager.Instance.totalScore += score;
            Destroy(this.gameObject);
        }
    }

    IEnumerator star_walk()
    {
        yield return new WaitForSeconds(1f);
        follow = true;
        animator.SetBool("isWalk", true);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
