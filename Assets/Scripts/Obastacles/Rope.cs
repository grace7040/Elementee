using UnityEngine;

public class Rope : MonoBehaviour
{
    public GameObject ropeLine;
    public int ropeCnt;
    public FixedJoint2D exJoint;

    FixedJoint2D _currentJoint;

    private void Start()
    {
        for(int i = 0; i< ropeCnt; i++)
        {
            _currentJoint = Instantiate(ropeLine, transform).GetComponent<FixedJoint2D>();
            _currentJoint.connectedBody = exJoint.GetComponent<Rigidbody2D>();

            exJoint = _currentJoint;

            if (i == ropeCnt - 1)
            {
                _currentJoint.GetComponent<Rigidbody2D>().mass = 10;
                _currentJoint.GetComponent<SpriteRenderer>().enabled = false;
                _currentJoint.tag = "Rope";
            }
        }
    }
}
