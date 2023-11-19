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
        // 중점과 반지름으로 가상의 원을 생성해 추출하려는 반경 이내에 들어와있는 콜라이더들을
        // 반환하는 함수

        if (colliders.Length > 0) // 콜라이더의 개수가 1개 이상이면
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Player") // 콜라이더의 태그가 플레이어면
                {
                    print("감지");
                    follow = true;
                    target = colliders[i].gameObject; // 타겟 위치 저장
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
        //    print("따라와");   
        //    Vector2 direction = transform.position - target.position;
        //    transform.Translate(direction.normalized * speed * Time.deltaTime);
        //}

        if (follow)
        {
            float dir = target.transform.position.x - transform.position.x; //2d이기에 좌우만 빼면됨 (내x위치 - targetx위치)
            dir = (dir < 0) ? 1 : -1; //방향조절 dir의 x거리가 -라면 -1,아니면 1
            transform.Translate(new Vector2(dir, 0) * speed * Time.deltaTime);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 이펙트 넣기
            GameManager.Instance.totalScore += score;
            Destroy(this.gameObject);
        }
    }
}
