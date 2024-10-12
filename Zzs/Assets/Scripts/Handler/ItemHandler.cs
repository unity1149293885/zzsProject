using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler :  HandlerBase
{
    public override void Handler(long ProtocolNumber, string jsonStr)
    {
        switch (ProtocolNumber)
        {
            //上/下架返回
            case (int)ProcolCode.Code_Item_ChangeState_rst:

                ItemChangeStateRst rst = JsonConvert.DeserializeObject<ItemChangeStateRst>(jsonStr);

                if (rst.isSuccess)
                {
                    //更新界面
                    if (rst.isDown)
                    {
                        DataManager.DownItem(rst.id);
                    }
                    else
                    {
                        DataManager.UpItem(rst.id);
                    }

                    //更新界面
                    EventCenter.Broadcast<bool>(EventType.ChangeItemState, rst.isDown);
                }
                else
                {
                    Debug.LogError("请检查!!");
                }


                break;
        }
    }

}
