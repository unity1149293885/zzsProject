using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelpPanel : MonoBehaviour
{
    public Button btn_userManage;
    public Button btn_itemManage;
    public Button btn_quitLogin;
    public Button btn_close;
    void Start()
    {
        btn_userManage.onClick.AddListener(OpenUserManage);
        btn_itemManage.onClick.AddListener(OpenitemManage);
        btn_quitLogin.onClick.AddListener(quitlogin);
        btn_close.onClick.AddListener(close);

        if(MyData.userInfo.My_UserType == UserType.Manager)
        {
            btn_userManage.gameObject.SetActive(true);
            btn_itemManage.gameObject.SetActive(true);
        }
        else if (MyData.userInfo.My_UserType == UserType.Teamer)
        {
            btn_itemManage.gameObject.SetActive(true);
        }
    }

    public void OpenUserManage()
    {
        PanelManager.OpenPanel(OpenPanelType.RegiesterUser);
    }

    public void OpenitemManage()
    {
        
    }

    public void quitlogin()
    {
        SceneManager.LoadScene(0);
    }

    public void close()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
