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
    public int My_brandId;
    public int My_typeId;
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
            foreach (var it in DataManager.DownList)
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



