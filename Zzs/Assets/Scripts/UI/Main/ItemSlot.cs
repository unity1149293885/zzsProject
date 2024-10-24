﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemInfo iteminfo;

    public Image image_icon;
    public Text text_name;
    public Text text_price;
    public Button buton_Click;

    public GameObject Tipobj;
    public Text tip_text;

    public GameObject DownTip; 

    private void Awake()
    {
        buton_Click.onClick.AddListener(ClickSlot);
    }
    public async void InitItemSlot(ItemInfo iteminfo)
    {
        text_name.text = iteminfo.name;
        image_icon.sprite = UIResourceLoadManager.Instance.LoadSprite("LittleIcon", iteminfo.name + "_little");

        this.iteminfo = iteminfo;
        bool state = false;
        if(iteminfo.tip != "0")
        {
            state = true;
        }

        update_price();

        update_tip(state, iteminfo.tip);

        DownTip.SetActive(iteminfo.isDown);

    }

    public void update_price()
    {
        
        if (MyData.userInfo.UserType == UserType.Teamer || MyData.userInfo.UserType == UserType.Manager)
        {
            text_price.text = iteminfo.TeamPrice.ToString();
        }
        else if (MyData.userInfo.UserType == UserType.Broker)
        {
            if (this.iteminfo.isDown)
            {
                text_price.text = "已下架";
            }
            text_price.text = iteminfo.BrokerPrice.ToString();
        }
        else if (MyData.userInfo.UserType == UserType.NewUser)
        {
            //对游客隐藏价格
            text_price.text = "加入代理看底价";
        }
    }

    public void update_tip(bool state,string content)
    {
        Tipobj.SetActive(state);
        tip_text.text = content;
    }

    public void ClickSlot()
    {
        PanelManager.OpenDetailPanel(this.iteminfo);
    }
}
