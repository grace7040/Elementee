using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    class ObjectInfo
    {
        public string ObjectName;
        public GameObject Prefab;
        public int Count;
    }

    string currentColorName = null;

    [SerializeField] ObjectInfo[] _objectInfos = null;

    // 생성할 오브젝트의 key값지정을 위한 변수
    string _objectName;

    // 오브젝트풀들을 관리할 딕셔너리
    Dictionary<string, IObjectPool<GameObject>> _poolDict = new();

    // 오브젝트풀에서 오브젝트를 새로 생성할때 사용할 딕셔너리
    readonly Dictionary<string, GameObject> _gameObjectDict = new();

    static ObjectPoolManager _instance = null;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (null == _instance)
            {
                return null;
            }
            return _instance;
        }
    }


    void Awake()
    {
        if (null == _instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        for (int idx = 0; idx < _objectInfos.Length; idx++)
        {
            var pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPooledItem, true, _objectInfos[idx].Count, _objectInfos[idx].Count);

            if (_gameObjectDict.ContainsKey(_objectInfos[idx].ObjectName))
            {
                Debug.Log($"{_objectInfos[idx].ObjectName} 이미 등록된 오브젝트입니다.");
                yield return null;
            }

            _gameObjectDict.Add(_objectInfos[idx].ObjectName, _objectInfos[idx].Prefab);
            _poolDict.Add(_objectInfos[idx].ObjectName, pool);

            // 미리 오브젝트 생성
            for (int i = 0; i < _objectInfos[idx].Count; i++)
            {
                _objectName = _objectInfos[idx].ObjectName;
                var pooledItem = CreatePooledItem().GetComponent<PoolAble>();
                pooledItem.Pool.Release(pooledItem.gameObject);
            }

            yield return 3;
        }

        //Debug.Log("오브젝트풀링 준비 완료");
    }

    // 생성
    GameObject CreatePooledItem()
    {
        var pooledItem = Instantiate(_gameObjectDict[_objectName], this.transform.position, Quaternion.identity, this.transform);
        pooledItem.GetComponent<PoolAble>().Pool = _poolDict[_objectName];
        return pooledItem;
    }

    // 대여
    void OnTakeFromPool(GameObject pooledItem)
    {
        pooledItem.SetActive(true);
    }

    // 반환
    void OnReturnedToPool(GameObject pooledItem)
    {
        pooledItem.SetActive(false);
    }

    // 삭제
    void OnDestroyPooledItem(GameObject pooledItem)
    {
        Destroy(pooledItem);
    }

    public GameObject GetGameObject(string objectName)
    {
        _objectName = objectName;

        if (_gameObjectDict.ContainsKey(objectName) == false)
        {
            Debug.Log($"<{objectName}> 은 오브젝트풀에 등록되지 않은 오브젝트입니다.");
            return null;
        }

        return _poolDict[objectName].Get();
    }


    public GameObject GetCurrentColorBlood()
    {
        _objectName = currentColorName;
        if (_gameObjectDict.ContainsKey(currentColorName) == false)
        {
            Debug.Log($"<{currentColorName}> 은 오브젝트풀에 등록되지 않은 오브젝트입니다.");
            return null;
        }

        return _poolDict[currentColorName].Get();
    }

    public GameObject GetColorBlood(Colors color)
    {
        var bloodName = GetBloodNameByColor(color);
        return _poolDict[bloodName].Get();
    }

    public void SetColorName(Colors color)
    {
        currentColorName = GetBloodNameByColor(color);
    }

    string GetBloodNameByColor(Colors color)
    {
        var colorName = new StringBuilder();
        colorName.Append(color.ToString() + "Blood");

        return colorName.ToString();
    }
}