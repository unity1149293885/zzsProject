using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegiesterPanel : MonoBehaviour
{
    public Button CloseRegiester_Btn;
    public Button Regiester_Btn;
    public InputField Input_Regiestername;
    public InputField Input_Regiesterphone;
    public InputField Input_Regiestertype;
    // Start is called before the first frame update
    void Start()
    {
        CloseRegiester_Btn.onClick.AddListener(close_Regiester);
        Regiester_Btn.onClick.AddListener(RegiesterUser);

        EventCenter.AddListener<LoginCode>(EventType.UpdateLoginState, UpdateState);
    }
    
    public void close_Regiester()
    {
        gameObject.SetActive(false);
    }

    public void RegiesterUser()
    {
        string name = Input_Regiestername.text;
        string phone = Input_Regiesterphone.text;
        if (name == "" || phone == "")
        {
            EventCenter.Broadcast<string>(EventType.UpdateMessageBox, "名字或电话号不能为空");
            return;
        }
        int type = int.Parse(Input_Regiestertype.text);
        var req = new RegiesterUserReq(name, phone, (UserType)type);
        NetManager.SendtoServer<RegiesterUserReq>((int)ProcolCode.Code_Register_req, req);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<LoginCode>(EventType.UpdateLoginState, UpdateState);
    }

    //传入状态码
    public void UpdateState(LoginCode StateCode)
    {
        if (StateCode == LoginCode.Register_Success)
        {
            MessageTip.showTip("注册成功");

            Input_Regiestername.text = "";
            Input_Regiesterphone.text = "";
            Input_Regiestertype.text = "";
        }
        else
        {
            switch (StateCode)
            {
                case LoginCode.Register_Fail_isHave:
                    MessageTip.showOneSelect("注册失败：该名字已注册", "重试");
                    break;
            }
        }
    }
}
