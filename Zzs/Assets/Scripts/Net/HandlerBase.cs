using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有handler处理服务端的结果 都注册一个协议号区间
//继承该类即可
public abstract class HandlerBase
{
    public abstract void Handler(long ProtocolNumber, string jsonStr);
}


public static class ProctoolInfo
{
    public static Dictionary<KeyValuePair<long, long>, HandlerBase> ProctoolDic = new Dictionary<KeyValuePair<long, long>, HandlerBase>()
    {
        { new KeyValuePair<long, long>(100001, 199999),new StartHandler() },
        { new KeyValuePair<long, long>(200001, 299999),new MainHandler() }
    };
}
