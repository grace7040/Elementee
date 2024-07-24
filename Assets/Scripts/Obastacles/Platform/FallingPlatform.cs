using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public List<GameObject> Platforms = new List<GameObject>(2);
    public float FallDelay = 0f;

    BoxCollider2D _collider;
    Rigidbody2D _leftRigid;
    Rigidbody2D _rightRigid;
    BoxCollider2D _leftCollider;
    BoxCollider2D _rightCollider;
    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _leftRigid = Platforms[0].transform.GetComponent<Rigidbody2D>();
        _rightRigid = Platforms[1].transform.GetComponent<Rigidbody2D>();
        _leftCollider = Platforms[0].transform.GetComponent<BoxCollider2D>();
        _rightCollider = Platforms[1].transform.GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collider.enabled = false;
        _leftCollider.enabled = true;
        _rightCollider.enabled = true;
        this.CallOnDelay(FallDelay, () => { _leftRigid.constraints = RigidbodyConstraints2D.None; });
        this.CallOnDelay(FallDelay, () => { _rightRigid.constraints = RigidbodyConstraints2D.None; });
    }
}
