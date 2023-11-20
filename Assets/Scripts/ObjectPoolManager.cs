using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [System.Serializable]
    private class ObjectInfo
    {
        // ������Ʈ �̸�
        public string objectName;
        // ������Ʈ Ǯ���� ������ ������Ʈ
        public GameObject perfab;
        // ��� �̸� ���� �س�������
        public int count;
    }

    string currentColorName = null;
    // ������ƮǮ �Ŵ��� �غ� �Ϸ�ǥ��
    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // ������ ������Ʈ�� key�������� ���� ����
    private string objectName;

    // ������ƮǮ���� ������ ��ųʸ�
    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // ������ƮǮ���� ������Ʈ�� ���� �����Ҷ� ����� ��ųʸ�
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
                Debug.LogFormat("{0} �̹� ��ϵ� ������Ʈ�Դϴ�.", objectInfos[idx].objectName);
                return;
            }

            goDic.Add(objectInfos[idx].objectName, objectInfos[idx].perfab);
            ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

            // �̸� ������Ʈ ���� �س���
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                CreatePooledItem().GetComponent<PoolAble>();
                PoolAble poolAbleGo = CreatePooledItem().GetComponent<PoolAble>();
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);
            }
        }

        Debug.Log("������ƮǮ�� �غ� �Ϸ�");
        IsReady = true;
    }

    // ����
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(goDic[objectName]);
        //Debug.Log($"����: {poolGo.name}");
        poolGo.GetComponent<PoolAble>().Pool = ojbectPoolDic[objectName];
        poolGo.transform.SetParent(this.transform);
        return poolGo;
    }

    // �뿩
    private void OnTakeFromPool(GameObject poolGo)
    {
        //Debug.Log($"�뿩: {poolGo.name}");
        poolGo.SetActive(true);
    }

    // ��ȯ
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
        //Debug.Log($"��ȯ: {poolGo.name}");
    }

    // ����
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    public GameObject GetGo(string goName)
    {
        objectName = goName;

        if (goDic.ContainsKey(goName) == false)
        {
            Debug.LogFormat("{0} ������ƮǮ�� ��ϵ��� ���� ������Ʈ�Դϴ�.", goName);
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
            Debug.LogFormat("{0} ������ƮǮ�� ��ϵ��� ���� ������Ʈ�Դϴ�.", currentColorName);
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
                Debug.Log("Blood Effect ���� �����ϼ���.");
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