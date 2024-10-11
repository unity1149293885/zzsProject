using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public static class DataManager
{
    public static Dictionary<int, UserInfo> AllUserInfos = new Dictionary<int, UserInfo>();

    public static UserInfo MyUserInfo;

    public static List<ItemInfo> AllItemInfos = new List<ItemInfo>();//运行时的总item数据
    public static Dictionary<int, string> BrandDic = new Dictionary<int, string>();//品牌数据
    public static Dictionary<int, string> TypeDic = new Dictionary<int, string>();//产品类型数据

    public static List<int> DownList;//下架产品数组
    //下架产品
    public static void DownItem(int id)
    {
        if (DownList.Contains(id))
        {
            Debug.LogError("下架数组中已有该id：" + id);
            return;
        }
        DownList.Add(id);
    }
    //上架产品
    public static void UpItem(int id)
    {
        if (!DownList.Contains(id))
        {
            Debug.LogError("下架数组中没有该id：" + id);
            return;
        }
        DownList.Remove(id);
    }

    //通过代理价筛选
    public static List<ItemInfo> GetBrokerPriceRangeItemList(List<ItemInfo> List, int low,int high)
    {
        List<ItemInfo> ans = new List<ItemInfo>();
        for(int i = 0; i < List.Count; i++)
        {
            if (List[i].My_BrokerPrice >= low && List[i].My_BrokerPrice <= high)
            {
                ans.Add(List[i]);
            }
        }

        return ans;
    }

    //通过产品类型筛选
    public static List<ItemInfo> GetTypeRangeItemList(List<ItemInfo> List, int typeId)
    {
        List<ItemInfo> ans = new List<ItemInfo>();
        for (int i = 0; i < List.Count; i++)
        {
            if (List[i].My_typeId == typeId)
            {
                ans.Add(List[i]);
            }
        }

        return ans;
    }

    //通过产品品牌筛选
    public static List<ItemInfo> GetBrandRangeItemList(List<ItemInfo> List,int brandId)
    {
        List<ItemInfo> ans = new List<ItemInfo>();
        for (int i = 0; i < List.Count; i++)
        {
            if (List[i].My_brandId == brandId)
            {
                ans.Add(List[i]);
            }
        }

        return ans;
    }

    //通过itemid 得到指定的ItemInfo
    public static ItemInfo GetItemInfoById(int id)
    {
        for(int i=0;i< AllItemInfos.Count; i++)
        {
            if (id == AllItemInfos[i].My_id) return AllItemInfos[i];
        }
        Debug.LogError("物品id为: " + id + "的物品 不存在表里 请检查！");
        return null;
    }
}


public struct brandInfo
{
    public string id;
    public string name;
}

public struct typeInfo
{
    public string id;
    public string name;
}
public static class ExcelTools
{
   
}

