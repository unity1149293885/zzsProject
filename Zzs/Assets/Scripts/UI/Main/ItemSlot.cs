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

    private void Awake()
    {
        buton_Click.onClick.AddListener(ClickSlot);
    }

    public async void InitItemSlot(ItemInfo iteminfo)
    {
        text_name.text = iteminfo.My_name;
        text_price.text = iteminfo.My_BrokerPrice.ToString();
        image_icon.sprite = UIResourceLoadManager.Instance.LoadSprite("LittleIcon", iteminfo.My_name + "_little");

        this.iteminfo = iteminfo;
        bool state = false;
        if(iteminfo.My_tip != "0")
        {
            state = true;
        }
        update_tip(state, iteminfo.My_tip);
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
