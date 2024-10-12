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
            //��/�¼ܷ���
            case (int)ProcolCode.Code_Item_ChangeState_rst:

                ItemChangeStateRst rst = JsonConvert.DeserializeObject<ItemChangeStateRst>(jsonStr);

                if (rst.isSuccess)
                {
                    //���½���
                    if (rst.isDown)
                    {
                        DataManager.DownItem(rst.id);
                    }
                    else
                    {
                        DataManager.UpItem(rst.id);
                    }

                    //���½���
                    EventCenter.Broadcast<bool>(EventType.ChangeItemState, rst.isDown);
                }
                else
                {
                    Debug.LogError("����!!");
                }


                break;
        }
    }

}
