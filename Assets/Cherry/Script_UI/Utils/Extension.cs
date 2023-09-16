using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
	// 확장 메서드
	public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
	{
		return Util.GetOrAddComponent<T>(go);
	}

	// 확장메서드
	public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
	{
		UI_Base.BindEvent(go, action, type);
	}
}