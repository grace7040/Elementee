using UnityEngine;
using UnityEngine.Pool;

public class PoolAble : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }
    //bool isReleased;
    //private void OnEnable()
    //{
    //    isReleased = false;
    //}

    public void ReleaseObject()
    {
        //if (isReleased) return;

        Pool.Release(gameObject);
        //isReleased = true;
    }
}