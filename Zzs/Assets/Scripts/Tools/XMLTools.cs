using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public  static class XMLTools
{
    //读取item XML
    public static async Task ReadItemXml()
    {
        await LoadBrand();

        await Task.Delay(500);//延迟0.5s

        await LoadType();

        await Task.Delay(500);//延迟0.5s

        await LoadItem();

        await LoadPic();
    }

    public static async Task LoadPic()
    {
        XmlDocument Pic_xmlDoc = new XmlDocument();
        Addressables.LoadAssetAsync<TextAsset>("PicCount.XML").Completed += (obj) => {
            TextAsset text = obj.Result;
            Pic_xmlDoc.LoadXml(text.ToString());

            XmlNodeList nodeList = Pic_xmlDoc.SelectSingleNode("Root").ChildNodes;
            foreach (XmlNode child in nodeList)
            {
                DataManager.PicDic.Add(child.Name,int.Parse(child.InnerText));
            }
            Debug.Log("加载PicCount.XML资源完毕 文件夹数量：" + DataManager.PicDic.Count);
        };
    }
    public static async Task LoadBrand()
    {
        XmlDocument Brand_xmlDoc = new XmlDocument();
        Addressables.LoadAssetAsync<TextAsset>("Item_Brand.XML").Completed += (obj) => {
            TextAsset text = obj.Result;
            Brand_xmlDoc.LoadXml(text.ToString());

            XmlNodeList nodeList = Brand_xmlDoc.SelectSingleNode("Brand").ChildNodes;
            foreach (XmlNode child in nodeList)
            {
                int id = int.Parse(child["id"].InnerText);
                string name = child["brand"].InnerText;
                int sort = int.Parse(child["sort"].InnerText);

                DataManager.BrandDic.Add(id, new BrandInfo(id, name, sort));
            }
            Debug.Log("加载Item_Brand.XML资源完毕 品牌数量：" + DataManager.BrandDic.Count);
        };
    }

    public static async Task LoadType()
    {
        XmlDocument Type_xmlDoc = new XmlDocument();
        Addressables.LoadAssetAsync<TextAsset>("Item_ItemType.XML").Completed += (obj) => {
            TextAsset text = obj.Result;
            Type_xmlDoc.LoadXml(text.ToString());

            XmlNodeList nodeList = Type_xmlDoc.SelectSingleNode("ItemType").ChildNodes;
            foreach (XmlNode child in nodeList)
            {
                int id = int.Parse(child["id"].InnerText);
                string name = child["type"].InnerText;
                int sort = int.Parse(child["sort"].InnerText);

                DataManager.TypeDic.Add(id, new typeInfo(id, name, sort));
            }

            DataManager.SortType();
            Debug.Log("加载Item_ItemType.XML资源完毕 类型数量：" + DataManager.TypeDic.Count);

            EventCenter.Broadcast(EventType.LoadedItemType);
        };
    }

    public static async Task LoadItem()
    {
        XmlDocument xmlDoc = new XmlDocument();
        Addressables.LoadAssetAsync<TextAsset>("Item_Item.XML").Completed += (obj) => {
            TextAsset text = obj.Result;
            xmlDoc.LoadXml(text.ToString());

            XmlNodeList nodeList = xmlDoc.SelectSingleNode("Item").ChildNodes;
            foreach (XmlNode child in nodeList)
            {
                ItemInfo curItem = new ItemInfo();
                curItem.id = int.Parse(child["id"].InnerText);
                curItem.name = child["name"].InnerText;
                curItem.brandId = int.Parse(child["brand"].InnerText);
                curItem.typeId = int.Parse(child["type"].InnerText);
                curItem.TeamPrice = int.Parse(child["teamPrice"].InnerText);
                curItem.BrokerPrice = int.Parse(child["brokerPrice"].InnerText);
                curItem.RetailPrice = int.Parse(child["retailPrice"].InnerText);
                curItem.TaobaoPrice = int.Parse(child["taobaoPrice"].InnerText);
                curItem.source = child["source"].InnerText;
                curItem.size = child["size"].InnerText;
                curItem.taste = child["taste"].InnerText;
                curItem.desc = child["desc"].InnerText;
                curItem.tip = child["tip"].InnerText;

                DataManager.AllItemInfos.Add(curItem);
            }

            DataManager.SortItem();
            Debug.Log("加载item xml资源完毕 产品数量：" + DataManager.AllItemInfos.Count);

            EventCenter.Broadcast(EventType.UpdateMainPanel);
        };
    }
}
