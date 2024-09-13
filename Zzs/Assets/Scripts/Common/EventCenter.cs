using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter 
{
    private static Dictionary<EventType, Delegate> m_EventTable = new Dictionary<EventType, Delegate>();
    
    //添加监听(无参数）
    public static void AddListener(EventType type,Callback callback)
    {
        if (!m_EventTable.ContainsKey(type))
        {
            m_EventTable.Add(type, null);
        }

        var d = m_EventTable[type];
        if ( d != null && callback.GetType()!=d.GetType())
        {
            Debug.LogError("添加监听失败：添加的事件类型，和当前注册码的类型不一致！");
            return; 
        }
        m_EventTable[type] = (Callback)m_EventTable[type] + callback;
    }

    //添加监听(1参数）
    public static void AddListener<T>(EventType type, Callback<T> callback)
    {
        if (!m_EventTable.ContainsKey(type))
        {
            m_EventTable.Add(type, null);
        }

        var d = m_EventTable[type];
        if (d != null && callback.GetType() != d.GetType())
        {
            Debug.LogError("添加监听失败：添加的事件类型，和当前注册码的类型不一致！");
            return;
        }
        m_EventTable[type] = (Callback<T>)m_EventTable[type] + callback;
    }
    //添加监听(2参数）
    public static void AddListener<T,X>(EventType type, Callback<T,X> callback)
    {
        if (!m_EventTable.ContainsKey(type))
        {
            m_EventTable.Add(type, null);
        }

        var d = m_EventTable[type];
        if (d != null && callback.GetType() != d.GetType())
        {
            Debug.LogError("添加监听失败：添加的事件类型，和当前注册码的类型不一致！");
            return;
        }
        m_EventTable[type] = (Callback<T, X>)m_EventTable[type] + callback;
    }
    //添加监听(3参数）
    public static void AddListener<T,X,Y>(EventType type, Callback<T,X,Y> callback)
    {
        if (!m_EventTable.ContainsKey(type))
        {
            m_EventTable.Add(type, null);
        }

        var d = m_EventTable[type];
        if (d != null && callback.GetType() != d.GetType())
        {
            Debug.LogError("添加监听失败：添加的事件类型，和当前注册码的类型不一致！");
            return;
        }
        m_EventTable[type] = (Callback< T, X, Y>)m_EventTable[type] + callback;
    }

    //移除监听
    public static void RemoveListener(EventType type, Callback callback)
    {
        if (!m_EventTable.ContainsKey(type))
        {
            Debug.LogError("移除监听失败：注册表中没有该注册码");
        }
        else
        {
            var d = m_EventTable[type];
            if (d == null)
            {
                Debug.LogError("移除监听失败：注册表中该注册码,对应的事件为空");
            }
            else
            {
                if (d.GetType()!= callback.GetType())
                {
                    Debug.LogError("移除监听失败：移除的事件类型，和当前注册码的类型不一致！");
                }
            }
            m_EventTable[type] = (Callback)m_EventTable[type] - callback;
        }
    }

    //移除监听(1参数）
    public static void RemoveListener<T>(EventType type, Callback<T> callback)
    {
        if (!m_EventTable.ContainsKey(type))
        {
            Debug.LogError("移除监听失败：注册表中没有该注册码");
        }
        else
        {
            var d = m_EventTable[type];
            if (d == null)
            {
                Debug.LogError("移除监听失败：注册表中该注册码,对应的事件为空");
            }
            else
            {
                if (d.GetType() != callback.GetType())
                {
                    Debug.LogError("移除监听失败：移除的事件类型，和当前注册码的类型不一致！");
                }
            }
            m_EventTable[type] = (Callback<T>)m_EventTable[type] - callback;
        }
    }
    //移除监听(2参数）
    public static void RemoveListener<T, X>(EventType type, Callback<T, X> callback)
    {
        if (!m_EventTable.ContainsKey(type))
        {
            Debug.LogError("移除监听失败：注册表中没有该注册码");
        }
        else
        {
            var d = m_EventTable[type];
            if (d == null)
            {
                Debug.LogError("移除监听失败：注册表中该注册码,对应的事件为空");
            }
            else
            {
                if (d.GetType() != callback.GetType())
                {
                    Debug.LogError("移除监听失败：移除的事件类型，和当前注册码的类型不一致！");
                }
            }
            m_EventTable[type] = (Callback<T, X>)m_EventTable[type] - callback;
        }
    }
    //移除监听(3参数）
    public static void RemoveListener<T, X ,Y>(EventType type, Callback<T, X, Y> callback)
    {
        if (!m_EventTable.ContainsKey(type))
        {
            Debug.LogError("移除监听失败：注册表中没有该注册码");
        }
        else
        {
            var d = m_EventTable[type];
            if (d == null)
            {
                Debug.LogError("移除监听失败：注册表中该注册码,对应的事件为空");
            }
            else
            {
                if (d.GetType() != callback.GetType())
                {
                    Debug.LogError("移除监听失败：移除的事件类型，和当前注册码的类型不一致！");
                }
            }
            m_EventTable[type] = (Callback<T, X, Y>)m_EventTable[type] - callback;
        }
    }

    //广播事件
    public static void Broadcast(EventType type)
    {
        var de = m_EventTable[type];

        Delegate d;
        if (m_EventTable.TryGetValue(type, out d))
        {
            Callback callback = d as Callback;
            if (callback !=  null)
            {
                callback();
                Debug.Log("无参广播 时间码：" + type);
            }
            else
            {
                Debug.LogError(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", type));
            }
        }
    }
    //广播事件（1参数）
    public static void Broadcast<T>(EventType type,T t)
    {
        var de = m_EventTable[type];

        Delegate d;
        if (m_EventTable.TryGetValue(type, out d))
        {
            Callback<T> callback = d as Callback<T>;
            if (callback != null)
            {
                callback(t);
                Debug.Log("1参广播 时间码：" + type);
            }
            else
            {
                Debug.LogError(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", type));
            }
        }
    }

    //广播事件（2参数）
    public static void Broadcast<T,X>(EventType type, T t,X x)
    {
        var de = m_EventTable[type];

        Delegate d;
        if (m_EventTable.TryGetValue(type, out d))
        {
            Callback<T,X> callback = d as Callback<T,X>;
            if (callback != null)
            {
                callback(t,x);
                Debug.Log("2参广播 时间码：" + type);
            }
            else
            {
                Debug.LogError(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", type));
            }
        }
    }

    //广播事件（3参数）
    public static void Broadcast<T,X,Y>(EventType type, T t,X x,Y y)
    {
        var de = m_EventTable[type];

        Delegate d;
        if (m_EventTable.TryGetValue(type, out d))
        {
            Callback<T, X, Y> callback = d as Callback<T, X, Y>;
            if (callback != null)
            {
                callback(t,x,y);
                Debug.Log("3参广播 时间码：" + type);
            }
            else
            {
                Debug.LogError(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", type));
            }
        }
    }

}
