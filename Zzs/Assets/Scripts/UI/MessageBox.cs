using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text text;
    public Button button;
    public GameObject image;

    private void Awake()
    {
        button.onClick.AddListener(OnClickClose);

        EventCenter.AddListener<string>(EventType.UpdateMessageBox, OpenMessage);
    }

    public void OpenMessage(string str)
    {
        text.text = str;
        image.SetActive(true);
    }

    public void OnClickClose()
    {
        image.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventType.UpdateMessageBox, OpenMessage);
    }
}
