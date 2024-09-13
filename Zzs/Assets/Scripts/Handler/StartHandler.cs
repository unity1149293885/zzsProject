using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHandler : HandlerBase
{
    public override void Handler(long ProtocolNumber, string jsonStr)
    {
        Debug.Log("客户端MainHandler执行事件 id:" + ProtocolNumber);
        switch (ProtocolNumber)
        {
            case 100002:
                LoginRst rst = JsonConvert.DeserializeObject<LoginRst>(jsonStr);

                EventCenter.Broadcast<LoginCode>(EventType.UpdateLoginState, rst.StateCode);
                break;
        }
    }
}
