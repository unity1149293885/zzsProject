using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemDetailPanel : MonoBehaviour
{
    private ItemInfo itemInfo;

    public Text Name_value;
    public Text Brand_value;
    public Text Type_value;
    public Text Price_value;
    public Text Taobao_value;
    public Text Source_value;
    public Text Size_value;
    public Text Taste_value;
    public Text Desc_value;

    public Image Main_Image;

    public Button Button_Return;
    public Button Button_Left;
    public Button Button_Right;

    public GameObject Big_bg;
    public Image Big_Image;
    public GameObject Big_Accros_bg;
    public Image Big_Accros_Image;

    private int Pic_Index = 0;

    public static ItemDetailPanel Instance;

    public UserType userType;
    private void Awake()
    {
        Instance = this;
        

        Big_bg.SetActive(false);
        Big_Accros_bg.SetActive(false);

        Button_Return.onClick.AddListener(delegate { CloseDetailPanel(); }) ;
        Button_Left.onClick.AddListener(delegate { Cilck_left(); });
        Button_Right.onClick.AddListener(delegate { Cilck_right(); });

        Main_Image.gameObject.GetComponent<Button>().onClick.AddListener(delegate { Open_Picture(); });

        Big_Image.gameObject.GetComponent<Button>().onClick.AddListener(delegate { Close_Picture(); });
        Big_Accros_Image.gameObject.GetComponent<Button>().onClick.AddListener(delegate { Close_Picture(); });
    }

    public void UpdateItemDetailPanel(ItemInfo iteminfo)
    {
        //Debug.LogError(iteminfo.My_id);
        this.itemInfo = iteminfo;
        userType = StartPanel.Instance.GetCurUserType();

        Name_value.text = iteminfo.My_name;
        Brand_value.text = iteminfo.My_brand.ToString();
        Type_value.text = iteminfo.My_type.ToString();
        Price_value.text = iteminfo.My_BrokerPrice.ToString();
        Taobao_value.text = iteminfo.My_TaobaoPrice.ToString();
       
        if (userType == UserType.Mamager) { 

            Source_value.gameObject.SetActive(true);
            Source_value.text = iteminfo.My_source;
        }
        else
        {
             Source_value.gameObject.SetActive(false);
        }
        Size_value.text = iteminfo.My_size;
        Taste_value.text = iteminfo.My_taste;
        Desc_value.text = iteminfo.My_desc;

        Main_Image.sprite = iteminfo.IconList[Pic_Index].sprite;
        Main_Image.GetComponent<RectTransform>().sizeDelta = iteminfo.IconList[Pic_Index].size;

        gameObject.SetActive(true);
    }

    public void CloseDetailPanel() 
    {
        Pic_Index = 0;
        gameObject.SetActive(false);
    }
    public void Cilck_left()
    {
       //Debug.LogError("上一张 当前数量："+ this.itemInfo.IconList.Count);
        Pic_Index--;
        if (Pic_Index < 0)
        {
            Pic_Index = this.itemInfo.IconList.Count - 1;
        }
        UpdateItemDetailPanel(itemInfo);
    }

    public void Cilck_right()
    {
        //Debug.LogError("下一张 当前数量："+ this.itemInfo.IconList.Count);
        Pic_Index++;
        if (Pic_Index >= this.itemInfo.IconList.Count)
        {
            Pic_Index = 0;
        }
        UpdateItemDetailPanel(itemInfo);
    }

    public void Open_Picture()
    {
        Big_Image.gameObject.SetActive(true);


        if (itemInfo.IconList[Pic_Index].size.x > itemInfo.IconList[Pic_Index].size.y)
        {
            Big_bg.SetActive(false);
            Big_Accros_bg.SetActive(true);

            Big_Accros_Image.sprite = itemInfo.IconList[Pic_Index].sprite;
            Big_Accros_Image.GetComponent<RectTransform>().sizeDelta = itemInfo.IconList[Pic_Index].size;
        }

        else
        {
            Big_bg.SetActive(true);
            Big_Accros_bg.SetActive(false);
            Big_Image.sprite = itemInfo.IconList[Pic_Index].sprite;
            Big_Image.GetComponent<RectTransform>().sizeDelta = itemInfo.IconList[Pic_Index].size;
        }
    }
    public void Close_Picture()
    {
        Big_bg.SetActive(false);
        Big_Accros_bg.SetActive(false);
    }
}
