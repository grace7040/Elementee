using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static void CallOnDelay(this MonoBehaviour behaviour, float delay, Action onComplete)
    {
        behaviour.StartCoroutine(CallOnDelay(delay, onComplete));
    }

    static IEnumerator CallOnDelay(float delay, Action onComplete)
    {
        yield return new WaitForSeconds(delay);
        onComplete?.Invoke();
    }
}
