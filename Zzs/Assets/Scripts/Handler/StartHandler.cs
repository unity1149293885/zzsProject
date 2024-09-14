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
                LoginRst login_rst = JsonConvert.DeserializeObject<LoginRst>(jsonStr);

                EventCenter.Broadcast<LoginCode>(EventType.UpdateLoginState, login_rst.StateCode);
                break;
            case 100004:
                RegiesterUserRst reg_rst = JsonConvert.DeserializeObject<RegiesterUserRst>(jsonStr);

                EventCenter.Broadcast<LoginCode>(EventType.UpdateLoginState, reg_rst.StateCode);
                break;
        }
    }
}
