using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        public string objectName;
        public GameObject perfab;
        public int count;
    }

    string currentColorName = null;

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // 생성할 오브젝트의 key값지정을 위한 변수
    private string objectName;

    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // 오브젝트풀에서 오브젝트를 새로 생성할때 사용할 딕셔너리
    private readonly Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();

    private static ObjectPoolManager instance = null;
    public static ObjectPoolManager Instance
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
            //SetColorName(Colors.def);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            if (goDic.ContainsKey(objectInfos[idx].objectName))
            {
                Debug.Log($"{objectInfos[idx].objectName} 이미 등록된 오브젝트입니다.");
                yield return null;
            }

            goDic.Add(objectInfos[idx].objectName, objectInfos[idx].perfab);
            ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

            // 미리 오브젝트 생성
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                CreatePooledItem().GetComponent<PoolAble>();
                PoolAble poolAbleGo = CreatePooledItem().GetComponent<PoolAble>();
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);
            }

            yield return 1;
        }

        //Debug.Log("오브젝트풀링 준비 완료");
    }

    // 생성
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(goDic[objectName], this.transform.position, Quaternion.identity);
        //Debug.Log($"생성: {poolGo.name}");
        poolGo.GetComponent<PoolAble>().Pool = ojbectPoolDic[objectName];
        poolGo.transform.SetParent(this.transform);
        return poolGo;
    }

    // 대여
    private void OnTakeFromPool(GameObject poolGo)
    {
        //Debug.Log($"대여: {poolGo.name}");
        poolGo.SetActive(true);
    }

    // 반환
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
        //Debug.Log($"반환: {poolGo.name}");
    }

    // 삭제
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    public GameObject GetGo(string goName)
    {
        objectName = goName;

        if (goDic.ContainsKey(goName) == false)
        {
            Debug.Log($"<{goName}> 은 오브젝트풀에 등록되지 않은 오브젝트입니다.");
            return null;
        }
        //Debug.Log(goName);
        return ojbectPoolDic[goName].Get();
    }


    public GameObject GetCurrentColorBlood()
    {
        objectName = currentColorName;
        if (goDic.ContainsKey(currentColorName) == false)
        {
            Debug.Log($"<{currentColorName}> 은 오브젝트풀에 등록되지 않은 오브젝트입니다.");
            return null;
        }
        //Debug.Log(currentColorName);
        return ojbectPoolDic[currentColorName].Get();
    }

    public GameObject GetColorBlood(Colors color)
    {
        string name = GetBloodNameByColor(color);
        return ojbectPoolDic[name].Get();
    }

    public void SetColorName(Colors color)
    {
        currentColorName = GetBloodNameByColor(color);
    }

    public string GetBloodNameByColor(Colors color)
    {
        var colorName = new StringBuilder();
        colorName.Append(color.ToString() + "Blood");

        return colorName.ToString();
    }
}