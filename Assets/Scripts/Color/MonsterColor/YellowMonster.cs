using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class YellowMonster : MonsterController
{
    public GameObject VoltObject;

    protected override void Start()
    {
        base.Start();
        stateMachine.ChangeState(new IdleState(this));
    }

    protected override void Update()
    {
        if (IsDie) return;
        base.Update();
    }

    public override void Attack()
    {
        StartCoroutine(Charge());

        VoltObject.SetActive(true);
        this.CallOnDelay(1f, () => VoltObject.SetActive(false));
    }

    protected IEnumerator Charge()
    {
        Rb.velocity = Vector2.zero;
        dir = new Vector2(Player.position.x - transform.position.x, 0);

        yield return new WaitForSeconds(1.2f);

        StartCoroutine(Dash(0.4f));
    }

    IEnumerator Dash(float duration)
    {
        float startTime = Time.time;

        dir.y = 0;
        dir.Normalize();

        while (Time.time - startTime < duration)
        {
            Vector2 movement = 7f * Time.deltaTime * dir;
            transform.Translate(movement);

            yield return null;
        }
        yield return new WaitForSeconds(0.8f);
    }
}