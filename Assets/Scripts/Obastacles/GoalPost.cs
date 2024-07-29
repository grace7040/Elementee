using UnityEngine;

public class GoalPost : MonoBehaviour
{
    public GameObject obj;

    Rigidbody2D _rigid;
    FrictionJoint2D  _frictionJoint;
    
    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball")){
            _frictionJoint = collision.gameObject.AddComponent<FrictionJoint2D>();
            _frictionJoint.connectedBody = _rigid;
            _frictionJoint.autoConfigureConnectedAnchor = false;
            _frictionJoint.connectedAnchor = new Vector2(0, 0);
            _frictionJoint.maxForce = 100;
            _frictionJoint.maxTorque = 5;
            collision.GetComponent<CapsuleCollider2D>().isTrigger = true;
            Instantiate(obj, transform);
        }
    }
}
