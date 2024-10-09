using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Э���
public enum ProcolCode
{
    Code_Login_req = 100001,//��¼����
    Code_Login_rst = 100002,//��¼����

    Code_Register_req = 200001,//ע������
    Code__Register_rst = 200002,//ע�᷵��

    Code_Item_ChangeState_req = 300001,//��Ʒ��/�¼�����
    Code_Item_ChangeState_rst = 300002,//��Ʒ��/�¼ܷ���
}

#region ��¼����
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
    //10001 ��¼�ɹ�
    //10002 ע��ɹ�
    //20001 �������
    //20002 δע��
    //20003 ���û���ע��
    Login_Success = 10001,
    Login_Fail_PasswordError = 20001,
    Login_Fail_UnLogin = 20002,

    Register_Success = 10002,
    Register_Fail_isHave = 20003,
}
#endregion

#region ע���û�
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

#region ��Ʒ���¼�
public class ItemChangeStateReq
{
    public int id;
    public bool isDown;//true �ϼܣ�false �¼�
}

public class ItemChangeStateRst
{
    public int id;
    public bool isDown;//true �ϼܣ�false �¼�
    public bool isSuccess;
}
#endregion
