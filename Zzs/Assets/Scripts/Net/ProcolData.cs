using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//协议号
public enum ProcolCode
{
    Code_Login_req = 100001,//登录请求
    Code_Login_rst = 100002,//登录返回

    Code_Register_req = 200001,//注册请求
    Code__Register_rst = 200002,//注册返回

    Code_Item_ChangeState_req = 300001,//商品上/下架请求
    Code_Item_ChangeState_rst = 300002,//商品上/下架返回
}

#region 登录数据
public enum Login_state
{
    logined = 1,
    unlogin = 2
}

public class LoginReq
{
    public string Name;
    public string Phone;
    public DateTime Time;
}

public class LoginRst
{
    public LoginCode StateCode;
    public UserType userType;
    public int uid;
    public List<int> DownList;
}

public enum LoginCode
{
    //10001 登录成功
    //10002 注册成功
    //20001 密码错误
    //20002 未注册
    //20003 该用户已注册
    Login_Success = 10001,
    Login_Fail_PasswordError = 20001,
    Login_Fail_UnLogin = 20002,

    Register_Success = 10002,
    Register_Fail_isHave = 20003,
}
#endregion

#region 注册用户
public class RegiesterUserReq
{
    public string name;
    public string phone;
    public UserType type;

    public RegiesterUserReq(string name,string phone,UserType type)
    {
        this.name = name;
        this.phone = phone;
        this.type = type;
    }
}

public class RegiesterUserRst
{
    public LoginCode StateCode;
    public long uid;
}
#endregion

#region 商品上下架
public class ItemChangeStateReq
{
    public int id;
    public bool isDown;//true 上架，false 下架
}

public class ItemChangeStateRst
{
    public int id;
    public bool isDown;//true 上架，false 下架
    public bool isSuccess;
}
#endregion
