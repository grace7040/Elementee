using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects); // Dictionary 에 추가

        // T 에 속하는 오브젝트들을 Dictionary의 Value인 objects 배열의 원소들에 하나하나 추가
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            //if (objects[i] == null)
            //    Debug.Log($"Failed to bind({names[i]})");
        }
    }


    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        
        

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action; // 혹시나 이미 있을까봐 빼줌
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action; // 혹시나 이미 있을까봐 빼줌
                evt.OnDragHandler += action;
                break;
            case Define.UIEvent.DownClick:
                evt.OnDownHandler -= action; // 혹시나 이미 있을까봐 빼줌
                evt.OnDownHandler += action;
                break;
            case Define.UIEvent.UpClick:
                evt.OnUPHandler -= action; // 혹시나 이미 있을까봐 빼줌
                evt.OnUPHandler += action;
                break;

        }
    }


    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); } // 오브젝트로서 가져오기
    protected Text GetText(int idx) { return Get<Text>(idx); } // Text로서 가져오기
    protected Button GetButton(int idx) { return Get<Button>(idx); } // Button로서 가져오기
    protected Image GetImage(int idx) { return Get<Image>(idx); } // Image로서 가져오기

    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)

    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
            case Define.UIEvent.DownClick:
                evt.OnDownHandler -= action; // 혹시나 이미 있을까봐 빼줌
                evt.OnDownHandler += action;
                break;
            case Define.UIEvent.UpClick:
                evt.OnUPHandler -= action; // 혹시나 이미 있을까봐 빼줌
                evt.OnUPHandler += action;
                break;

        }
    }
}