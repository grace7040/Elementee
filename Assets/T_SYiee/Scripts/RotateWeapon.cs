using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour
{

    public GameObject center;
    public float speed = 50.0f;

    void Update()
    {
        //Debug.Log("rotate");
        transform.RotateAround(center.transform.position, Vector3.forward, speed * Time.deltaTime);
    }

}
