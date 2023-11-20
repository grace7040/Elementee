using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Fire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            // ���Ϳ� �÷��̾��� ��ġ ���̸� ����մϴ�.
            float distance = player.position.x - transform.position.x;

            // �÷��̾ ������ ���ʿ� ������ �¿츦 �������ϴ�.
            if (distance < 0f)
            {
                sprite.flipX = true;
            }
            // �÷��̾ ������ �����ʿ� ������ �¿츦 ������ �ʽ��ϴ�.
            else if (distance > 0f)
            {
                sprite.flipX = false;
            }
        }

        Destroy(gameObject, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
