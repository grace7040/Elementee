using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_ChangeScale : MonoBehaviour
{
    GameObject _body;
    bool _decrease = true;
    float _decreseSpeed;


    public int Speed = 8;
    public float minX = 0;
    public float maxX = 1;

    // Start is called before the first frame update
    void Start()
    {
        _body = this.gameObject;
        _decreseSpeed = Speed * 0.001f;

    }

    // Update is called once per frame
    void Update()
    {


        if (_decrease)
        {
            _body.transform.localScale = new Vector3(_body.transform.localScale.x - _decreseSpeed
                                                , _body.transform.localScale.y
                                                , _body.transform.localScale.z);
        }
        else
        {
            _body.transform.localScale = new Vector3(_body.transform.localScale.x + _decreseSpeed
                                                    , _body.transform.localScale.y
                                                    , _body.transform.localScale.z);
        }

        if (_body.GetComponent<Transform>().localScale.x > maxX) _decrease = true;

        if (_body.GetComponent<Transform>().localScale.x < minX) _decrease = false;
    }
}
