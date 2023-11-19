using UnityEngine;
using UnityEngine.Pool;

public class PoolAble : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }
    private void OnEnable()
    {
        this.CallOnDelay(0.5f, ReleaseObject);
    }

    public void ReleaseObject()
    {
        Pool.Release(gameObject);
    }
}