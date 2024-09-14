using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MessageBoxType
{
    Tip=1,
    Button_One = 2,
    Button_two = 3,
}

public class MessageBox : MonoBehaviour
{
    public Text text;
    public GameObject image;

    public Text tip;

    public Button btn1;
    public Button btn2;
    public Text btn1_txt;
    public Text btn2_txt;

    private void Awake()
    {
        btn1.onClick.AddListener(OnClickClose);

        EventCenter.AddListener<MessageBoxType, string,string,string>(EventType.UpdateMessageBox, OpenMessage);
    }

    public void OpenMessage(MessageBoxType type,string str,string btn_text1 = null, string btn_text2 = null)
    {
        switch (type) {
            case MessageBoxType.Tip:
                text.gameObject.SetActive(false);
                image.SetActive(false);
                StartCoroutine(DisappearTip(str));
                break;
            case MessageBoxType.Button_One:
                text.text = str;
                text.gameObject.SetActive(true);
                image.SetActive(true);
                tip.gameObject.SetActive(false);

                btn1_txt.text = btn_text1;

                btn2.gameObject.SetActive(false);
                break;
            case MessageBoxType.Button_two:
                text.text = str;
                text.gameObject.SetActive(true);
                image.SetActive(true);
                tip.gameObject.SetActive(false);

                btn1_txt.text = btn_text1;
                btn2_txt.text = btn_text2;

                btn2.gameObject.SetActive(true);
                break;
        }
    }

    public IEnumerator DisappearTip(string str)
    {
        tip.text = str;
        tip.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        tip.gameObject.SetActive(false);
    }

    public void OnClickClose()
    {
        image.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<MessageBoxType, string, string, string>(EventType.UpdateMessageBox, OpenMessage);
    }
}
