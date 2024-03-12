using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPost : MonoBehaviour
{
    Rigidbody2D rigid;
    FrictionJoint2D  fj;
    public GameObject obj;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball")){
            fj = collision.gameObject.AddComponent<FrictionJoint2D>();
            fj.connectedBody = rigid;
            fj.autoConfigureConnectedAnchor = false;
            fj.connectedAnchor = new Vector2(0, 0);
            fj.maxForce = 100;
            fj.maxTorque = 5;
            collision.GetComponent<CapsuleCollider2D>().isTrigger = true;
            Instantiate(obj, transform);
        }
    }
}
