using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int My_id;
    public string My_name;
    public string My_phone;
    public UserType My_UserType;
}
