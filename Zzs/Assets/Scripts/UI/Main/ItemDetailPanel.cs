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

    public Button Button_ChangeState;
    public Text Text_State;

    private int Pic_Index = 0;
    private int MaxCount = -1;

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
        Button_ChangeState.onClick.AddListener(delegate { Click_ChangeState(); });

        EventCenter.AddListener<bool>(EventType.ChangeItemState, UpdateStateInfo);
    }

    public void UpdateItemDetailPanel(ItemInfo iteminfo)
    {
        Debug.Log("打开物品详情，物品id：" + iteminfo.id);
        this.root.SetActive(true);
        this.itemInfo = iteminfo;

        MaxCount = DataManager.PicDic[iteminfo.name];

        update_Text(iteminfo);

        ShowPic(iteminfo);

        UpdateStateInfo(iteminfo.isDown);

        UpdateBtnState(Pic_Index);
    }

    public void UpdateStateInfo(bool isDown)
    {
        Text_State.text = isDown ? "上架" : "下架";
    }

    public void update_Text(ItemInfo iteminfo)
    {
        Name_value.text = iteminfo.name;
        Brand_value.text = DataManager.BrandDic[iteminfo.brandId].ToString();
        Type_value.text = DataManager.TypeDic[iteminfo.typeId].ToString();
        Taobao_value.text = iteminfo.TaobaoPrice.ToString();

        if (MyData.userInfo.My_UserType == UserType.Teamer || MyData.userInfo.My_UserType == UserType.Manager)
        {
            Price_value.text = iteminfo.TeamPrice.ToString();
            Source_value.text = iteminfo.source;
            Button_ChangeState.gameObject.SetActive(true);
        }
        else if (MyData.userInfo.My_UserType == UserType.Broker)
        {
            Price_value.text = iteminfo.BrokerPrice.ToString();
            Source_value.text = "铁汁";
            Button_ChangeState.gameObject.SetActive(false);
        }

        Size_value.text = iteminfo.size;
        Taste_value.text = iteminfo.taste;
        Desc_value.text = iteminfo.desc;
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

    private void UpdateBtnState(int index)
    {
        Button_Left.gameObject.SetActive(true);
        Button_Right.gameObject.SetActive(true);
        if (index == 0)
        {
            Button_Left.gameObject.SetActive(false);
        }
        if(index == MaxCount - 1)
        {
            Button_Right.gameObject.SetActive(false);
        }
        
        if(index != 0 && index != MaxCount - 1)
        {
            Button_Left.gameObject.SetActive(true);
            Button_Right.gameObject.SetActive(true);
        }
    }

    public void Cilck_left()
    {
        Pic_Index--;
        
        ShowPic(itemInfo);

        UpdateBtnState(Pic_Index);
    }

    public void Cilck_right()
    {
        Pic_Index++;
       
        ShowPic(itemInfo);

        UpdateBtnState(Pic_Index);
    }

    public void Click_ChangeState()
    {
        ItemChangeStateReq req = new ItemChangeStateReq();
        req.id = itemInfo.id;
        req.isDown = !itemInfo.isDown;

        NetManager.SendtoServer<ItemChangeStateReq>((int)ProcolCode.Code_Item_ChangeState_req, req);
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

    private void OnDestroy()
    {
        EventCenter.RemoveListener<bool>(EventType.ChangeItemState, UpdateStateInfo);
    }

}
