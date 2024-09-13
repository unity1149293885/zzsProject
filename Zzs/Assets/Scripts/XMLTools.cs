using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class XMLTools : MonoBehaviour
{
    public static XMLTools Instance;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(transform.gameObject);
    }
    //读取user表
    public void ReadUserXml()
    {
        string path;
#if UNITY_ANDROID
        path = "jar:file://" + Application.dataPath + "!assets/" + "userconfig";
#endif

#if UNITY_EDITOR
        path = Path.Combine(Application.streamingAssetsPath, "userconfig");
#endif

#if UNITY_STANDALONE_WIN
        path = Path.Combine(Application.streamingAssetsPath, "userconfig");
#endif
        var ab = AssetBundle.LoadFromFile(path);

        if (ab == null)
        {

            Debug.LogError("ab=null 路径："+ path);
        }
        XmlDocument xmlDoc = new XmlDocument();

        TextAsset text = ab.LoadAsset<TextAsset>("UserConfig.XML"); //泛型加载
        if (text == null)
        {
            Debug.LogError("读取UserConfig.XML失败 请检查目录");
            return;
        }
        xmlDoc.LoadXml(text.ToString());

        //获取全部子节点;
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("UserInfos").ChildNodes;
        foreach (XmlNode child in nodeList)
        {
            int id = int.Parse(child["id"].InnerText);

            UserInfo curUser = new UserInfo();
            curUser.My_id = id;
            curUser.My_name = child["name"].InnerText;
            curUser.My_phone = child["phone"].InnerText;
            switch (child["isManager"].InnerText)
            {
                case "0":
                    //管理者
                    curUser.My_UserType = UserType.Mamager;
                    break;
                case "1":
                    //团队成员
                    curUser.My_UserType = UserType.Teamer;
                    break;
                case "2":
                    //代理层
                    curUser.My_UserType = UserType.Broker;
                    break;
            }

            if (DataManager.Instance.AllUserInfos == null)
            {
                DataManager.Instance.AllUserInfos = new Dictionary<int, UserInfo>();
            }
            DataManager.Instance.AllUserInfos.Add(id, curUser);
        }
        Debug.Log("加载user xml资源完毕 user数量："+ DataManager.Instance.AllUserInfos.Count);
    }

    //读取item XML
    public void ReadItemXml()
    {
        string path;
#if UNITY_ANDROID
        path = "jar:file://" + Application.dataPath + "!assets/" + "itemconfig";
#endif

#if UNITY_EDITOR
        path = Path.Combine(Application.streamingAssetsPath, "itemconfig");
#endif

#if UNITY_STANDALONE_WIN
        path = Path.Combine(Application.streamingAssetsPath, "itemconfig");
#endif
        var ab = AssetBundle.LoadFromFile(path);

        XmlDocument xmlDoc = new XmlDocument();
        if (ab == null)
        {
            Debug.LogError("读取ab包资源失败");
            return;
        }
        TextAsset text= ab.LoadAsset<TextAsset>("ItemConfig.XML"); //泛型加载
        if (text == null)
        {
            Debug.LogError("读取ItemConfig.XML失败 请检查目录");
            return;
        }
        xmlDoc.LoadXml(text.ToString());

        //获取全部子节点;
        string icon_path;
#if UNITY_ANDROID
        icon_path = "jar:file://" + Application.dataPath + "!assets/" + "icon_sprite";
#endif

#if UNITY_EDITOR
        icon_path = Path.Combine(Application.streamingAssetsPath, "icon_sprite");
#endif

#if UNITY_STANDALONE_WIN
        icon_path = Path.Combine(Application.streamingAssetsPath, "icon_sprite");
#endif
        AssetBundle assetBundle = AssetBundle.LoadFromFile(icon_path);

        if (assetBundle == null)
        {
            Debug.LogError("ab包获取失败，路径：" + icon_path);
            return;
        }

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("ItemInfos").ChildNodes;
        foreach (XmlNode child in nodeList)
        {
            ItemInfo curItem = new ItemInfo();
            curItem.My_id = int.Parse(child["id"].InnerText);
            curItem.My_name = child["name"].InnerText; 
            foreach (object o in Enum.GetValues(typeof(Brand)))
            {
                if (o.ToString() == child["brand"].InnerText)
                {
                    curItem.My_brand = (Brand)o;
                }
            }
            curItem.My_type = (ItemType)int.Parse(child["type"].InnerText);
            curItem.My_TeamPrice = int.Parse(child["teamprice"].InnerText);
            curItem.My_BrokerPrice = int.Parse(child["brokerprice"].InnerText);
            curItem.My_RetailPrice = int.Parse(child["retailprice"].InnerText);
            curItem.My_TaobaoPrice = int.Parse(child["taobaoprice"].InnerText);
            curItem.My_source = child["source"].InnerText;
            curItem.My_size = child["size"].InnerText;
            curItem.My_taste = child["taste"].InnerText;
            curItem.My_desc = child["desc"].InnerText;
            curItem.My_tip = child["tip"].InnerText;

            curItem.GetAllIconName(assetBundle, curItem.My_type, curItem.My_name);

            if (DataManager.Instance.AllItemInfos == null)
            {
                DataManager.Instance.AllItemInfos = new List<ItemInfo>();
            }
            DataManager.Instance.AllItemInfos.Add(curItem);
        }
        Debug.Log("加载item xml资源完毕 item数量：" + DataManager.Instance.AllItemInfos.Count);
    }
    
}
