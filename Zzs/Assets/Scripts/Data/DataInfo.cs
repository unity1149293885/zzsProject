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

public class Animal
{
    public int id;
    public string name;
    public List<int> SkillIdList;
    public int price;
}

public class People
{
    public string name;
    public Dictionary<string, string> friends;
}



public enum UserType
{
    Mamager,
    Teamer,
    Broker,
}


