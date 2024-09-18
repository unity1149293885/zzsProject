using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatHandler : HandlerBase
{
    public override void Handler(long ProtocolNumber, string jsonStr)
    {
        //Debug.Log("客户端BattleHandler执行事件 id:" + ProtocolNumber);
        //switch (ProtocolNumber)
        //{
        //    case 100004:
        //        Animal ani = JsonConvert.DeserializeObject<Animal>(jsonStr);

        //        Debug.Log("动物价值：" + ani.price);
        //        break;
        //    case 100006:
        //        People peo = JsonConvert.DeserializeObject<People>(jsonStr);
        //        foreach (var t in peo.friends)
        //        {
        //            Debug.Log("key：" + t.Key + " value:" + t.Value);
        //        }
        //        break;
        //}
    }
}
