using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


//���� �ÿ��� Debug.Log() ȣ����� �ʰ� �����Ϳ����� ȣ��ǵ��� �ϴ� ���� �Լ��Դϴ�.
public static class Debug
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object message)
        => UnityEngine.Debug.Log(message);

    public static void LogError(object message)
        => UnityEngine.Debug.LogError(message);

}
