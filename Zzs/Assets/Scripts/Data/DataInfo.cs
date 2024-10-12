using System;
using System.Collections.Generic;
using UnityEngine;

public static class ConnectInfo
{
    public static string ipAddress = "10.12.20.93";
    public static int Port = 7788;
}

public static class MySqlInfo
{
    public static string server = "localhost";
    public static string user = "root";
    public static string database = "UserDataBase";
    public static string password = "zzs20001114";
}

public static class MyData
{
    public static UserInfo userInfo = new UserInfo();
}
public class UserInfo
{
    public long My_id;
    public string My_name;
    public string My_phone;
    public UserType My_UserType;
}

public enum UserType
{
    Manager = 1,
    Teamer = 2,
    Broker = 3,
}

public class BrandInfo
{
    public int id;
    public string brand;
    public int sortid;

    public BrandInfo(int id,string name,int sort)
    {
        this.id = id;
        brand = name;
        sortid = sort;
    }
}

public class typeInfo
{
    public int id;
    public string type;
    public int sortid;

    public typeInfo() { }

    public typeInfo(int id,string name, int sort)
    {
        this.id = id;
        type = name;
        sortid = sort;
    }
}

public class ItemInfo
{
    public int id;
    public string name;
    public int brandId;
    public int typeId;
    public int TeamPrice;
    public int BrokerPrice;
    public int RetailPrice;
    public int TaobaoPrice;
    public string size;
    public string taste;
    public string source;
    public string desc;
    public string tip;

    public bool isDown
    {
        get
        {
            foreach (var it in DataManager.DownList)
            {
                if (it == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}



