using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHandler : HandlerBase
{
    public override void Handler(long ProtocolNumber, string jsonStr)
    {
        Debug.Log("�ͻ���MainHandlerִ���¼� id:" + ProtocolNumber);
        switch (ProtocolNumber)
        {
            case (int)ProcolCode.Code__Register_rst:
                RegiesterUserRst reg_rst = JsonConvert.DeserializeObject<RegiesterUserRst>(jsonStr);

                EventCenter.Broadcast<LoginCode>(EventType.UpdateLoginState, reg_rst.StateCode);
                break;
        }
    }


}
