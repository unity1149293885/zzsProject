using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DataManager
{
    public static Dictionary<int, UserInfo> AllUserInfos = new Dictionary<int, UserInfo>();

    public static UserInfo MyUserInfo;

    public static List<ItemInfo> AllItemInfos = new List<ItemInfo>();
    
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
    public static List<ItemInfo> GetTypeRangeItemList(List<ItemInfo> List, ItemType type)
    {
        List<ItemInfo> ans = new List<ItemInfo>();
        for (int i = 0; i < List.Count; i++)
        {
            if (List[i].My_type == type)
            {
                ans.Add(List[i]);
            }
        }

        return ans;
    }

    //通过产品品牌筛选
    public static List<ItemInfo> GetBrandRangeItemList(List<ItemInfo> List,Brand brand)
    {
        List<ItemInfo> ans = new List<ItemInfo>();
        for (int i = 0; i < List.Count; i++)
        {
            if (List[i].My_brand == brand)
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
