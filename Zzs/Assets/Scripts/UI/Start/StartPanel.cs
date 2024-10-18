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

    // 加载进度
    float loadPro = 0;

    // 用以接受异步加载的返回值
    AsyncOperation AsyncOp = null;

    public UserType userType;
    

    private void Awake()
    {
        Application.targetFrameRate = 60;

        loadText.SetActive(false);
        UpdateGroup.SetActive(false);

    }
    private void Start()
    {
        //事件监听
        Button_login.onClick.AddListener(ClickStart);

        EventCenter.AddListener<LoginCode>(EventType.UpdateLoginState, UpdateState);
    }
    private void Update()
    {
        if (AsyncOp != null)//如果已经开始加载
        {
            loadPro = AsyncOp.progress; //获取加载进度,此处特别注意:加载场景的progress值最大为0.9!!!
        }
        if (loadPro >= 0.9f)//因为progress值最大为0.9,所以我们需要强制将其等于1
        {
            loadPro = 1;
        }
        slider.value = Mathf.Lerp(slider.value, loadPro, 1 * Time.deltaTime);//滑动块的value以插值的方式紧跟进度值
        if (slider.value > 0.99f)
        {
            slider.value = 1;
            AsyncOp.allowSceneActivation = true;
            UpdateGroup.SetActive(false);
            loadText.SetActive(true);
        }
        text.text = "加载进度：" + (int)(slider.value * 100) + "%";
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

        NetManager.SendtoServer<LoginReq>((int)ProcolCode.Code_Login_req, data, callback);
    }
    

    //传入状态码
    public void UpdateState(LoginCode StateCode)
    {
        if (StateCode == LoginCode.Login_Success)
        {
            UpdateGroup.SetActive(true);

            MyData.userInfo.My_name = Input_name.name;
            MyData.userInfo.My_phone = Input_phone.name;


            //进入新场景
            AsyncOp = SceneManager.LoadSceneAsync("mainScene", LoadSceneMode.Single);//异步加载场景名为"Demo Valley"的场景,LoadSceneMode.Single表示不保留现有场景
            AsyncOp.allowSceneActivation = false;//allowSceneActivation =true表示场景加载完成后自动跳转,经测,此值默认为true

            return;
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
            }
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<LoginCode>(EventType.UpdateLoginState, UpdateState);
    }

    

    public UserType GetCurUserType()
    {
        return userType;
    }
}
