using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star_Run : Star
{
    public GameObject target;

    public float speed = 2f;

    private bool follow = false;

    // Start is called before the first frame update
    void Start()
    {
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
                    print("����");
                    follow = true;
                    target = colliders[i].gameObject; // Ÿ�� ��ġ ����
                    break;
                }
               // else target = null;
            }
        }
    }

    private void TargetConfirm()
    {
        //if (follow)
        //{
        //    print("�����");   
        //    Vector2 direction = transform.position - target.position;
        //    transform.Translate(direction.normalized * speed * Time.deltaTime);
        //}

        if (follow)
        {
            float dir = target.transform.position.x - transform.position.x; //2d�̱⿡ �¿츸 ����� (��x��ġ - targetx��ġ)
            dir = (dir < 0) ? 1 : -1; //�������� dir�� x�Ÿ��� -��� -1,�ƴϸ� 1
            transform.Translate(new Vector2(dir, 0) * speed * Time.deltaTime);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ����Ʈ �ֱ�
            GameManager.Instance.totalScore += score;
            Destroy(this.gameObject);
        }
    }
}
