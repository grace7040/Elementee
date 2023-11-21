using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public bool ObjectPoolManager_Ready;
    public bool UI_InGame_Ready;
    bool IsActived;


    private static ObjectManager instance = null;
    public static ObjectManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        ObjectPoolManager_Ready = false;
        UI_InGame_Ready = false;
        IsActived = false;

        //StartCoroutine(nameof(CheckReady));
    }

    public GameObject Objects;
    private void Update()
    {
        if (!ObjectPoolManager_Ready) return;
        if (!UI_InGame_Ready) return;

        if (!IsActived) SetActivebjects();
    }

    void SetActivebjects()
    {
        IsActived = true;
        Objects.SetActive(true);
    }

    //IEnumerator CheckReady()
    //{
    //    while (!IsActived)
    //    {
    //        if(!ObjectPoolManager_Ready || !UI_InGame_Ready)
    //        yield return null;
    //    }
    //    StopCoroutine(nameof(CheckReady));
    //}

}
