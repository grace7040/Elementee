using UnityEngine;

public class RopeBar : MonoBehaviour
{
    public GameObject ropeLine;
    public GameObject bar;
    public int ropeCnt;
    public FixedJoint2D exJoint;

    FixedJoint2D _currentJoint;

    private void Start()
    {
        for (int i = 0; i < ropeCnt; i++)
        {
            _currentJoint = Instantiate(ropeLine, transform).GetComponent<FixedJoint2D>();
            _currentJoint.connectedBody = exJoint.GetComponent<Rigidbody2D>();

            exJoint = _currentJoint;

        }
        _currentJoint = Instantiate(bar, transform).GetComponent<FixedJoint2D>();
        _currentJoint.connectedBody = exJoint.GetComponent<Rigidbody2D>();
        _currentJoint.GetComponent<Rigidbody2D>().mass = 10;
    }
}
