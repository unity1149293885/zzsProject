using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
public class ItemDetailPanel : MonoBehaviour
{
    public GameObject root;
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
    private int MaxCount = -1;

    public UserType userType;

    private static ItemDetailPanel instance;

    private Sprite sprite;

    public static ItemDetailPanel Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        Instance = this;
        root.SetActive(false);
        Big_bg.SetActive(false);
        Big_Accros_bg.SetActive(false);

        Button_Return.onClick.AddListener(delegate { CloseDetailPanel(); }) ;
        Button_Left.onClick.AddListener(delegate { Cilck_left(); });
        Button_Right.onClick.AddListener(delegate { Cilck_right(); });

        Main_Image.gameObject.GetComponent<Button>().onClick.AddListener(delegate { Open_Picture(); });

        Big_Image.gameObject.GetComponent<Button>().onClick.AddListener(delegate { Close_Picture(); });
        Big_Accros_Image.gameObject.GetComponent<Button>().onClick.AddListener(delegate { Close_Picture(); });

        userType = MyData.userInfo.My_UserType;

        
    }
    

    public void UpdateItemDetailPanel(ItemInfo iteminfo)
    {
        Debug.Log("打开物品详情，物品id："+iteminfo.My_id);
        this.root.SetActive(true);
        this.itemInfo = iteminfo;

        MaxCount = UIResourceLoadManager.Instance.GetIconCount(iteminfo);

        Name_value.text = iteminfo.My_name;
        Brand_value.text = iteminfo.My_brand.ToString();
        Type_value.text = iteminfo.My_type.ToString();
        Price_value.text = iteminfo.My_BrokerPrice.ToString();
        Taobao_value.text = iteminfo.My_TaobaoPrice.ToString();
       
        if (userType == UserType.Mamager) { 

            Source_value.text = iteminfo.My_source;
        }
        else
        {
            Source_value.text = "铁汁";
        }
        Size_value.text = iteminfo.My_size;
        Taste_value.text = iteminfo.My_taste;
        Desc_value.text = iteminfo.My_desc;

        ShowPic(iteminfo);
    }

    public void ShowPic(ItemInfo info)
    {
        string path = UIResourceLoadManager.Instance.GetSpritePath(info, Pic_Index);
        Debug.Log(path);
        Addressables.LoadAssetAsync<Sprite>(path).Completed += (obj) =>
        {
            sprite = obj.Result;
            Main_Image.sprite = sprite;
        };
    }

    public void CloseDetailPanel() 
    {
        Pic_Index = 0;
        this.root.SetActive(false);
    }
    public void Cilck_left()
    {
       //Debug.LogError("上一张 当前数量："+ this.itemInfo.IconList.Count);
        Pic_Index--;
        if (Pic_Index < 0)
        {
            Pic_Index = MaxCount - 1;
        }
        ShowPic(itemInfo);
    }

    public void Cilck_right()
    {
        //Debug.LogError("下一张 当前数量："+ this.itemInfo.IconList.Count);
        Pic_Index++;
        if (Pic_Index >= MaxCount)
        {
            Pic_Index = 0;
        }
        ShowPic(itemInfo);
    }

    public void Open_Picture()
    {
        Big_Image.gameObject.SetActive(true);
        var width = sprite.rect.width;
        var height = sprite.rect.height;

        if (width > height)
        {
            Big_bg.SetActive(false);
            Big_Accros_bg.SetActive(true);

            Big_Accros_Image.sprite = sprite;
            Big_Accros_Image.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }

        else
        {
            Big_bg.SetActive(true);
            Big_Accros_bg.SetActive(false);

            Big_Image.sprite = sprite;
            Big_Image.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }
    }
    public void Close_Picture()
    {
        Big_bg.SetActive(false);
        Big_Accros_bg.SetActive(false);
    }

}
