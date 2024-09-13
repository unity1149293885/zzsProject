using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class MainPanel : MonoBehaviour
{
    public GameObject MessageBox;
    public GameObject Grid;
    public GameObject ItemSlotPrefab;
    public List<ItemSlot> ItemInfos;

    public static MainPanel Instance;

    //ui部分
    public InputField lowPrice;
    public InputField highPirce;

    public Dropdown TypeDropDown;
    public InputField BrandInput;

    public Button sift_Button;

    public GameObject Gird;

    //当前界面中显示的格子 共有多少
    int curCount = -1;

    public UserType userType;

    public GameObject ItemDetailPanel;


    private void Awake()
    {
        Instance = this;

        ItemDetailPanel.SetActive(false);

        XMLTools.Instance.ReadItemXml();

        sift_Button.onClick.AddListener(UpdateMainPanel);

        userType = StartPanel.Instance.GetCurUserType();

        InitMainPanel();
    }

    private void Start()
    {
        UpdateMainPanel();
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
    }

    public void UpdateMainPanel()
    {
        List<ItemInfo> list = DataManager.Instance.AllItemInfos;

        if (list.Count == 0)
        {
            Debug.LogError("item数量只有一个");
            MessageBox.SetActive(true);
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

        List<ItemInfo> ItemInfosList = null;

        //通过价格筛选
        ItemInfosList = DataManager.Instance.GetBrokerPriceRangeItemList(list, low, high);

        //通过种类筛选
        if (TypeDropDown.value != 0)
        {
            var it = ItemInfosList;
            ItemInfosList = DataManager.Instance.GetTypeRangeItemList(it, (ItemType)(TypeDropDown.value));
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
                
            ItemInfosList = DataManager.Instance.GetBrandRangeItemList(it, curBrand);
        }
        curCount = ItemInfosList.Count;

        if ( ItemInfosList.Count > ItemInfos.Count)
        {
            //当前格子不够
            int cur = ItemInfosList.Count - ItemInfos.Count;
            for(int i=0 ;i< ItemInfos.Count; i++)
            {
                ItemInfos[i].gameObject.SetActive(true);

                int price = ItemInfosList[i].My_BrokerPrice;
                if (ItemInfosList[i].IconList.Count == 0)
                {
                    Debug.LogError(ItemInfosList[i].My_name + "没有对应icon! 请添加");
                    continue;
                }
                ItemInfos[i].InitItemSlot(ItemInfosList[i]);
            }

            for (int i = ItemInfos.Count; i < ItemInfosList.Count; i++)
            {
                GameObject t = GameObject.Instantiate(ItemSlotPrefab, Grid.transform);
                ItemInfos.Add(t.GetComponent<ItemSlot>());

                int price = ItemInfosList[i].My_BrokerPrice;
                if (ItemInfos[i] == null)
                {
                    Debug.LogError("ItemInfos[i]为空 请检查");
                }
                if (ItemInfosList[i] == null)
                {
                    Debug.LogError("ItemInfosList[i]为空 请检查");
                }
                if (ItemInfosList[i].IconList.Count == 0)
                {
                    Debug.LogError(ItemInfosList[i].My_name + "没有对应icon! 请添加");
                    continue;
                }
                ItemInfos[i].InitItemSlot(ItemInfosList[i]);
            }
        }
        else
        {
            //格子太多 需要隐藏
            for(int i= 0;i< ItemInfosList.Count; i++)
            {
                ItemInfos[i].gameObject.SetActive(true);
                int price = ItemInfosList[i].My_BrokerPrice;
                if (ItemInfosList[i].IconList.Count == 0)
                {
                    Debug.LogError(ItemInfosList[i].My_name + "没有对应icon! 请添加");
                    continue;
                }
                ItemInfos[i].InitItemSlot(ItemInfosList[i]);
            }
            for(int i = ItemInfosList.Count;i< ItemInfos.Count; i++)
            {
                ItemInfos[i].gameObject.SetActive(false);
            }
        }
    }
}
