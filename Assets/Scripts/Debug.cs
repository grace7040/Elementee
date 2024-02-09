using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


//빌드 시에는 Debug.Log() 호출되지 않고 에디터에서만 호출되도록 하는 래핑 함수입니다.
public static class Debug
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object message)
        => UnityEngine.Debug.Log(message);

    public static void LogError(object message)
        => UnityEngine.Debug.LogError(message);

}
