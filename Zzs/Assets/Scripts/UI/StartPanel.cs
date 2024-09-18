using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartPanel : MonoBehaviour
{
    public Button Button_login;
    public InputField Input_name;
    public InputField Input_phone;

    public Slider slider;//滑动条
    public Text text;//文本
    public GameObject UpdateGroup;
    public GameObject loadText;

    public GameObject LoadGroup;

    public Button OpenRegiester_Btn;
    public Button CloseRegiester_Btn;
    public GameObject RegiesterGroup;
    public Button Regiester_Btn;
    public InputField Input_Regiestername;
    public InputField Input_Regiesterphone;
    public InputField Input_Regiestertype;

    // 加载进度
    float loadPro = 0;

    // 用以接受异步加载的返回值
    AsyncOperation AsyncOp = null;

    public UserType userType;
    

    private void Awake()
    {
        //Application.targetFrameRate = 60;

        //loadText.SetActive(false);
    }
    private void Start()
    {
        //事件监听
        Button_login.onClick.AddListener(ClickStart);
        OpenRegiester_Btn.onClick.AddListener(open_Regiester);
        CloseRegiester_Btn.onClick.AddListener(close_Regiester);
        Regiester_Btn.onClick.AddListener(RegiesterUser);
        

        EventCenter.AddListener<LoginCode>(EventType.UpdateLoginState, UpdateState);
    }
    private void Update()
    {
        //if (AsyncOp != null)//如果已经开始加载
        //{
        //    loadPro = AsyncOp.progress; //获取加载进度,此处特别注意:加载场景的progress值最大为0.9!!!
        //}
        //if (loadPro >= 0.9f)//因为progress值最大为0.9,所以我们需要强制将其等于1
        //{
        //    loadPro = 1;
        //}
        //slider.value = Mathf.Lerp(slider.value, loadPro, 1 * Time.deltaTime);//滑动块的value以插值的方式紧跟进度值
        //if (slider.value > 0.99f)
        //{
        //    slider.value = 1;
        //    AsyncOp.allowSceneActivation = true;
        //    UpdateGroup.SetActive(false);
        //    loadText.SetActive(true);
        //}
        //text.text = string.Format("{0:F0}%", slider.value * 100);//文本中以百分比的格式显示加载进度
    }


    public void ClickStart()
    {
        LoginReq data = new LoginReq();
        data.Name = Input_name.text;
        data.Phone = Input_phone.text;

        data.Time = DateTime.Now;

        if (Input_name.text == "" || Input_phone.text == "")
        {
            EventCenter.Broadcast<string>(EventType.UpdateMessageBox, "名字或电话号不能为空");
            return;
        }

        AsyncCallback callback = ar =>
        {

        };

        NetManager.SendtoServer<LoginReq>(100001, data, callback);
    }
    

    //传入状态码
    public void UpdateState(LoginCode StateCode)
    {
        if (StateCode == LoginCode.Login_Success)
        {
            //进入新场景
            //LoadGroup.SetActive(true);
            //AsyncOp = SceneManager.LoadSceneAsync("mainScene", LoadSceneMode.Single);//异步加载场景名为"Demo Valley"的场景,LoadSceneMode.Single表示不保留现有场景
            //AsyncOp.allowSceneActivation = false;//allowSceneActivation =true表示场景加载完成后自动跳转,经测,此值默认为true

            MyData.userInfo.My_name = Input_name.name;
            MyData.userInfo.My_phone = Input_phone.name;

            SceneManager.LoadScene(1);
            return;
        }
        else if (StateCode == LoginCode.Register_Success)
        {
            MessageTip.showTip("注册成功");
        }
        else
        {
            switch (StateCode) {
                case LoginCode.Login_Fail_PasswordError:
                    MessageTip.showOneSelect("登录失败：名字对应手机号校验失败 请检查！", "重试");
                    break;
                case LoginCode.Login_Fail_UnLogin:
                    MessageTip.showOneSelect("登录失败：未注册 请联系管理员！！", "重试");
                    break;
                case LoginCode.Register_Fail_isHave:
                    MessageTip.showOneSelect("注册失败：该名字已注册", "重试");
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<LoginCode>(EventType.UpdateLoginState, UpdateState);
    }

    #region 添加玩家
    public void open_Regiester()
    {
        RegiesterGroup.gameObject.SetActive(true);
    }

    public void close_Regiester()
    {
        RegiesterGroup.gameObject.SetActive(false);
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
        NetManager.SendtoServer<RegiesterUserReq>(100003, req);
    }
    #endregion

    public UserType GetCurUserType()
    {
        return userType;
    }
}
