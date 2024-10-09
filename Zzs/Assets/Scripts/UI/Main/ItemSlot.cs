using System.Collections;
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
        text_name.text = iteminfo.My_name;
        image_icon.sprite = UIResourceLoadManager.Instance.LoadSprite("LittleIcon", iteminfo.My_name + "_little");

        this.iteminfo = iteminfo;
        bool state = false;
        if(iteminfo.My_tip != "0")
        {
            state = true;
        }

        update_price();

        update_tip(state, iteminfo.My_tip);

        DownTip.SetActive(iteminfo.isDown);
    }

    public void update_price()
    {
        if (MyData.userInfo.My_UserType == UserType.Teamer || MyData.userInfo.My_UserType == UserType.Manager)
        {
            text_price.text = iteminfo.My_TeamPrice.ToString();
        }
        else if (MyData.userInfo.My_UserType == UserType.Broker)
        {
            text_price.text = iteminfo.My_BrokerPrice.ToString();
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
