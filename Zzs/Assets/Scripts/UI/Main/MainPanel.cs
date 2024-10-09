using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;
using Tools;

public class MainPanel : MonoBehaviour
{
    public List<ItemSlot> ItemInfos;
    
    //ui部分
    public InputField lowPrice;
    public InputField highPirce;

    public Dropdown TypeDropDown;
    public InputField BrandInput;

    public Button sift_Button;
    public Button help_Button;
    public GameObject helpPanel;

    //当前界面中显示的格子 共有多少
    int curCount = -1;

    public RecycleView recycleView;

    protected List<ItemInfo> ItemInfosList;

    private void Awake()
    {
        XMLTools.ReadItemXml();

        sift_Button.onClick.AddListener(UpdateMainPanel);

        InitMainPanel();

        EventCenter.AddListener<bool>(EventType.ChangeItemState, UpdateState);
    }

    public void NormalCallBack(GameObject cell, int index)
    {
        if (index + 1 >= ItemInfosList.Count)
        {
            return;
        }
        ItemSlot slot = cell.GetComponent<ItemSlot>();
        slot.InitItemSlot(ItemInfosList[index + 1]);

    }
    private void Start()
    {
        ItemInfosList = DataManager.AllItemInfos;

        recycleView.Init(NormalCallBack);

        EventCenter.AddListener(EventType.UpdateMainPanel, UpdateMainPanel);
    }

    public void InitMainPanel()
    {
        TypeDropDown.ClearOptions();

        List<OptionData> options = new List<OptionData>();
        foreach (ItemType aType in Enum.GetValues(typeof(ItemType)))
        {
            options.Add(new OptionData(aType.ToString()));
        }
        TypeDropDown.AddOptions(options);

        help_Button.onClick.AddListener(delegate { helpPanel.gameObject.SetActive(true); });
    }

    public void UpdateState(bool isDown)
    {
        UpdateMainPanel();
    }

    public void UpdateMainPanel()
    {
        List<ItemInfo> list = DataManager.AllItemInfos;

        if (list.Count == 0)
        {
            MessageTip.showTip("产品数量错误！");
            return;
        }

        int low = -1;
        if (lowPrice.text == "")
        {
            low = 0;
        }
        else
        {
            low = int.Parse(lowPrice.text);
        }

        int high = -1;
        if (highPirce.text == "")
        {
            high = 9999;
        }
        else
        {
            high = int.Parse(highPirce.text);
        }
        
        //通过价格筛选
        ItemInfosList = DataManager.GetBrokerPriceRangeItemList(list, low, high);

        //通过种类筛选
        if (TypeDropDown.value != 0)
        {
            var it = ItemInfosList;
            ItemInfosList = DataManager.GetTypeRangeItemList(it, (ItemType)(TypeDropDown.value));
        }

        //通过品牌筛选
        if (BrandInput.text != "" && BrandInput.text != "输入产品关键字")
        {
            var it = ItemInfosList;
                
            string str = BrandInput.text;

            Brand curBrand = Brand.gat;

            foreach (object o in Enum.GetValues(typeof(Brand)))
            {
                string cur = o.ToString();

                if (cur.Contains(str))
                {
                    curBrand = (Brand)o;
                }
            }
                
            ItemInfosList = DataManager.GetBrandRangeItemList(it, curBrand);
        }
        recycleView.ShowList(ItemInfosList.Count);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<bool>(EventType.ChangeItemState, UpdateState);
    }
}
