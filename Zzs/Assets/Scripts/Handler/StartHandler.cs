using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHandler : HandlerBase
{
    public override void Handler(long ProtocolNumber, string jsonStr)
    {
        Debug.Log("客户端StartHandler执行事件 id:" + ProtocolNumber);
        switch (ProtocolNumber)
        {
            case (int)ProcolCode.Code_Login_rst:
                LoginRst login_rst = JsonConvert.DeserializeObject<LoginRst>(jsonStr);

                EventCenter.Broadcast<LoginCode>(EventType.UpdateLoginState, login_rst.StateCode);

                MyData.userInfo.My_UserType = login_rst.userType;
                //Debug.Log("当前登录账号 账号类型：" + login_rst.userType);
                break;
            
        }
    }
}
