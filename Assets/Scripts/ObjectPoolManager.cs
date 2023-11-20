using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [System.Serializable]
    private class ObjectInfo
    {
        // 오브젝트 이름
        public string objectName;
        // 오브젝트 풀에서 관리할 오브젝트
        public GameObject perfab;
        // 몇개를 미리 생성 해놓을건지
        public int count;
    }

    string currentColorName = null;
    // 오브젝트풀 매니저 준비 완료표시
    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // 생성할 오브젝트의 key값지정을 위한 변수
    private string objectName;

    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // 오브젝트풀에서 오브젝트를 새로 생성할때 사용할 딕셔너리
    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        IsReady = false;

        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            if (goDic.ContainsKey(objectInfos[idx].objectName))
            {
                Debug.LogFormat("{0} 이미 등록된 오브젝트입니다.", objectInfos[idx].objectName);
                return;
            }

            goDic.Add(objectInfos[idx].objectName, objectInfos[idx].perfab);
            ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

            // 미리 오브젝트 생성 해놓기
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                CreatePooledItem().GetComponent<PoolAble>();
                PoolAble poolAbleGo = CreatePooledItem().GetComponent<PoolAble>();
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);
            }
        }

        Debug.Log("오브젝트풀링 준비 완료");
        IsReady = true;
    }

    // 생성
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(goDic[objectName]);
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
            Debug.LogFormat("{0} 오브젝트풀에 등록되지 않은 오브젝트입니다.", goName);
            return null;
        }
        Debug.Log(goName);
        return ojbectPoolDic[goName].Get();
    }

    public GameObject GetGo()
    {
        objectName = currentColorName;
        if (goDic.ContainsKey(currentColorName) == false)
        {
            Debug.LogFormat("{0} 오브젝트풀에 등록되지 않은 오브젝트입니다.", currentColorName);
            return null;
        }
        Debug.Log(currentColorName);
        return ojbectPoolDic[currentColorName].Get();
    }

    public void SetColorName(Colors color)
    {
        switch (color)
        {
            case Colors.def:
                Debug.Log("Blood Effect 색을 설정하세요.");
                break;
            case Colors.red:
                currentColorName = "RedBlood";
                break;
            case Colors.yellow:
                currentColorName = "YellowBlood";
                break;
            case Colors.blue:
                currentColorName = "BlueBlood";
                break;
            case Colors.orange:
                currentColorName = "OrangeBlood";
                break;
            case Colors.green:
                currentColorName = "GreenBlood";
                break;
            case Colors.purple:
                currentColorName = "PurpleBlood";
                break;
            case Colors.black:
                currentColorName = "BlackBlood";
                break;
        }
    }
}