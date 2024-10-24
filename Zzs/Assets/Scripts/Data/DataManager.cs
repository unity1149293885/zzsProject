﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public static class DataManager
{
    public static UserInfo MyUserInfo;

    public static Dictionary<int, UserInfo> AllUserInfos = new Dictionary<int, UserInfo>();//玩家数据
    public static List<ItemInfo> AllItemInfos = new List<ItemInfo>();//运行时的总item数据
    public static Dictionary<int, BrandInfo> BrandDic = new Dictionary<int, BrandInfo>();//品牌数据
    public static Dictionary<int, typeInfo> TypeDic = new Dictionary<int, typeInfo>();//产品类型数据

    public static Dictionary<string, int> PicDic = new Dictionary<string, int>();//key 资源名字，value资源数量

    /// <summary>
    /// 排序产品种类序列
    /// </summary>
    public static void SortType()
    {
        Dictionary<int, typeInfo> ans = new Dictionary<int, typeInfo>();

        int sortIndex = 1;
        while (true)
        {
            bool ishave = false;
            foreach(var it in TypeDic)
            {
                if (it.Value.sortid == sortIndex)
                {
                    ans.Add(it.Key, it.Value);

                    ishave = true;
                }
            }

            if(ishave == false)
            {
                break;
            }
            sortIndex++;
        }

        TypeDic = ans;
    }

    /// <summary>
    /// 排序产品
    /// </summary>
    public static void SortItem()
    {
        AllItemInfos.Sort(new ItemComparer());

        //foreach(var it in AllItemInfos)
        //{
        //    Debug.LogError(it.name);
        //}
    }

    class ItemComparer : Comparer<ItemInfo>
    {
        public override int Compare(ItemInfo a, ItemInfo b)
        {
            if(BrandDic.Count == 0 || TypeDic.Count == 0)
            {
                Debug.LogError("数据异常！");
                return 1;
            }
            int brand_a_sort = BrandDic[a.brandId].sortid;
            int type_a_sort = TypeDic[a.typeId].sortid;

            int brand_b_sort = BrandDic[b.brandId].sortid;
            int type_b_sort = TypeDic[b.typeId].sortid;

            int sort_a = brand_a_sort + type_a_sort;
            int sort_b = brand_b_sort + type_b_sort;

            return sort_a > sort_b ? 1 : -1;
        }
    }

    /// <summary>
    /// 获取第几个选项的数据
    /// </summary>
    /// <param name="index"></param>
    public static typeInfo gettypeInfoByIndex(int index)
    {
        int curindex = 0;
        foreach(var it in TypeDic)
        {
            if(index == curindex)
            {
                return it.Value;
            }
            
            curindex++;
        }
        Debug.LogError("没有这个选项：" + index);
        return null;
    }

    public static List<int> DownList = new List<int>();//下架产品数组
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
            if (List[i].BrokerPrice >= low && List[i].BrokerPrice <= high)
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
            if (List[i].typeId == typeId)
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
            if (List[i].brandId == brandId)
            {
                ans.Add(List[i]);
            }
        }

        return ans;
    }

    //本地读表登录
    public static void Native_LoginGame(LoginReq data,bool isNewUser)
    {
        //直接登录无需校验 | 或者是游客登录
        if (GameConfig.isDirectLogin || isNewUser == true)
        {
            //登录成功 模拟网络返回
            LoginRst currst = new LoginRst();
            currst.StateCode = LoginCode.Login_Success;

            if (isNewUser == true)
            {
                //游客登录
                MyData.userInfo.UserType = UserType.NewUser;
                MyData.userInfo.pic_name = "专属水映";
            }
            else
            {
                MyData.userInfo.UserType = UserType.Broker;
                MyData.userInfo.pic_name = "测试";
            }

            EventCenter.Broadcast<LoginCode>(EventType.UpdateLoginState, currst.StateCode);
            return;
        }
        LoginRst rst = new LoginRst();
        foreach (var info in AllUserInfos)
        {
            if(info.Value.name == data.Name && info.Value.phone == data.Phone)
            {
                //登录成功 模拟网络返回
                rst.StateCode = LoginCode.Login_Success;

                MyData.userInfo.UserType = info.Value.UserType;
                MyData.userInfo.pic_name = info.Value.pic_name;

                EventCenter.Broadcast<LoginCode>(EventType.UpdateLoginState, rst.StateCode);
                return;
            }
        }
        //登录失败
        rst.StateCode = LoginCode.Login_Fail_UnLogin;

        EventCenter.Broadcast<LoginCode>(EventType.UpdateLoginState, rst.StateCode);
    }
}


