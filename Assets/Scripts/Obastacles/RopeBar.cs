using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeBar : MonoBehaviour
{
    public GameObject ropeLine;
    public GameObject bar;
    public int ropeCnt;
    public FixedJoint2D exJoint;
    FixedJoint2D currentJoint;

    private void Start()
    {
        for (int i = 0; i < ropeCnt; i++)
        {
            currentJoint = Instantiate(ropeLine, transform).GetComponent<FixedJoint2D>();
            currentJoint.connectedBody = exJoint.GetComponent<Rigidbody2D>();

            exJoint = currentJoint;

        }
        currentJoint = Instantiate(bar, transform).GetComponent<FixedJoint2D>();
        currentJoint.connectedBody = exJoint.GetComponent<Rigidbody2D>();
        currentJoint.GetComponent<Rigidbody2D>().mass = 10;
    }
}
