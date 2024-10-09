using System;
using System.Collections.Generic;


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


public class UserManager
{
    public List<UserInfo> UserList;
}

public class UserInfo
{
    //转xml
    public string id;
    public string name;
    public string phone;
    public string UserType;

    //实际类型
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


