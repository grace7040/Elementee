using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;
    public static bool shuttingDown = false;
    public static T Instance
    {
        get
        {
            if (shuttingDown)
            {
                return null;
            }

            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent(typeof(T)) as T;
                    obj.name = typeof(T).ToString();

                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }
    private void OnDestroy()
    {
        shuttingDown = true;
    }
}
