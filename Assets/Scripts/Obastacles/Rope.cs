using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public GameObject ropeLine;
    public int ropeCnt;
    public FixedJoint2D exJoint;
    FixedJoint2D currentJoint;

    private void Start()
    {
        for(int i = 0; i< ropeCnt; i++)
        {
            currentJoint = Instantiate(ropeLine, transform).GetComponent<FixedJoint2D>();
            currentJoint.connectedBody = exJoint.GetComponent<Rigidbody2D>();

            exJoint = currentJoint;

            if (i == ropeCnt - 1)
            {
                currentJoint.GetComponent<Rigidbody2D>().mass = 10;
                currentJoint.GetComponent<SpriteRenderer>().enabled = false;
                currentJoint.tag = "Rope";
            }
        }
    }
}
