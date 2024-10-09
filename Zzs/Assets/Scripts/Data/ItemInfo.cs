using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.U2D;
using System.IO;

public enum ItemType
{
    全部 = 0,
    蛋白粉 = 1,
    增肌粉 = 2,
    肌酸 = 3,
    氮泵 = 4,
    谷氨酰胺 = 5,
    支链氨基酸 = 6,
    促睾 = 7,
    护具 = 8,
    印度 = 9,
    电子烟 = 10,
    证书 = 11,
    套餐 = 12,
    其他 = 13,
    左旋 = 14,
    减脂 = 15,
    维生素 = 16,
    鱼油 = 17,
    补剂功能 = 18
}


public enum Brand
{
    as奥都赛斯 = 0,
    康比特 = 1,
    肌肉科技 = 2,
    gat = 3,
    肌肉公爵 = 4,
    埃姆特 = 5,
    sst怪兽 = 6,
    轮子哥 = 7,
    肌肉梦想 = 8,
    证书 = 10,
    肌械时代 = 11,
    未来之星 = 12,
    恒美适 = 13,
    巅峰蓝魔 = 14,
    堕落天使 = 15,
    颅骨实验室 = 16,
    氧气能量 = 17,
    诺特兰德 = 18,
    印度 = 19,
    紫光=20
}
public static class ItemManager
{
    public static List<ItemInfo> ItemList;

    public static List<int> DownList;//下架产品

    public static void DownItem(int id)
    {
        if (DownList.Contains(id))
        {
            Debug.LogError("下架数组中已有该id：" + id);
            return;
        }
        DownList.Add(id);
    }
    public static void UpItem(int id)
    {
        if (!DownList.Contains(id))
        {
            Debug.LogError("下架数组中没有该id：" + id);
            return;
        }
        DownList.Remove(id);
    }

}
public struct IconInfo
{
    public Sprite sprite;
    public Vector2 size;
}
public class ItemInfo
{
    public string id;
    public string name;
    public string brand;
    public string type;
    public string TeamPrice;
    public string BrokerPrice;
    public string RetailPrice;
    public string TaobaoPrice;
    public string size;
    public string taste;
    public string source;
    public string desc;
    public string tip;

    public int My_id;
    public string My_name;
    public Brand My_brand;
    public ItemType My_type;
    public int My_TeamPrice;
    public int My_BrokerPrice;
    public int My_RetailPrice;
    public int My_TaobaoPrice;
    public string My_size;
    public string My_taste;
    public string My_source;
    public string My_desc;
    public string My_tip;

    public bool isDown
    {
        get
        {
           foreach(var it in ItemManager.DownList)
           {
                if (it == My_id)
                {
                    return true;
                }
           }
            return false;
        }
    }
}
