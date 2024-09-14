using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageTip 
{
    //顶部单句提示文本
    public static void showTip(string tip)
    {
        EventCenter.Broadcast<MessageBoxType, string, string, string>(EventType.UpdateMessageBox, MessageBoxType.Tip, tip, null, null);
    }

    //单个按钮自定义，标题自定义
    public static void showOneSelect(string tip,string btn_name = null)
    {
        EventCenter.Broadcast<MessageBoxType, string, string, string>(EventType.UpdateMessageBox, MessageBoxType.Button_One, tip, btn_name, null);
    }

    //2个按钮自定义，标题自定义
    public static void showTwoSelect(string tip, string btn_name1 = null, string btn_name2 = null)
    {
        EventCenter.Broadcast<MessageBoxType, string, string, string>(EventType.UpdateMessageBox, MessageBoxType.Button_two, tip, btn_name1, btn_name2);
    }
}
