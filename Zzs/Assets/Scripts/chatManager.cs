using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json;



public class chatManager : MonoBehaviour
{
    public Text chat_text;
    public InputField name_input;
    public InputField content_input;
    public Button btn;
    public Button login_btn;
    public Button getinfo_btn;

    private string content;
    private string name;
    private byte[] data = new byte[1024];

    private string message;
    
    void Start()
    {
        name_input.onValueChanged.AddListener(changeName);
        content_input.onValueChanged.AddListener(changeContent);
        btn.onClick.AddListener(ClickSend);
        //login_btn.onClick.AddListener(LoginAction);
        getinfo_btn.onClick.AddListener(GetInfo);

    }
    

    public void changeName(string Name)
    {
        this.name = Name;
    }

    public void changeContent(string Content)
    {
        this.content = Content;
    }

    public void ClickSend()
    {
        //SendtoServer(100003, content_input.text);
        //Animal dog = new Animal();
        //dog.id = 1001;
        //dog.name = "狗";
        //List<int> skillList = new List<int>(); ;
        //skillList.Add(123);
        //skillList.Add(324);
        //skillList.Add(17);

        //dog.SkillIdList = skillList;

        //NetManager.SendtoServer<Animal>(200001, dog);

        //content_input.text = "";

        //Debug.Log("客户端-》服务端请求登录：发送文本" + content_input.text);
    }

    public void GetInfo()
    {
        //People MrWang = new People();
        //MrWang.name = "王线";

        //NetManager.SendtoServer<People>(200003, MrWang);
    }

    // Update is called once per frame
    void Update()
    {
        if (message != "" && message!=null)
        {
            Debug.LogError("更新");
            chat_text.text += "\n" + message;
            message = "";
        }
    }
}
