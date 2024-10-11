using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public  static class XMLTools
{
    //读取item XML
    public static void ReadItemXml()
    {
        XmlDocument Brand_xmlDoc = new XmlDocument();
        Addressables.LoadAssetAsync<TextAsset>("Assets/XML/Item_Brand.XML").Completed += (obj) => {
            TextAsset text = obj.Result;
            Brand_xmlDoc.LoadXml(text.ToString());

            XmlNodeList nodeList = Brand_xmlDoc.SelectSingleNode("Brand").ChildNodes;
            foreach (XmlNode child in nodeList)
            {
                int id = int.Parse(child["id"].InnerText);
                string name = child["brand"].InnerText;

                DataManager.BrandDic.Add(id, name);
            }
            Debug.Log("加载Item_Brand.XML资源完毕 品牌数量：" + DataManager.BrandDic.Count);
        };

        XmlDocument Type_xmlDoc = new XmlDocument();
        Addressables.LoadAssetAsync<TextAsset>("Assets/XML/Item_ItemType.XML").Completed += (obj) => {
            TextAsset text = obj.Result;
            Type_xmlDoc.LoadXml(text.ToString());

            XmlNodeList nodeList = Type_xmlDoc.SelectSingleNode("ItemType").ChildNodes;
            foreach (XmlNode child in nodeList)
            {
                int id = int.Parse(child["id"].InnerText);
                string name = child["type"].InnerText;

                DataManager.TypeDic.Add(id, name);
            }
            Debug.Log("加载Item_ItemType.XML资源完毕 类型数量：" + DataManager.TypeDic.Count);
        };

        XmlDocument xmlDoc = new XmlDocument();
        Addressables.LoadAssetAsync<TextAsset>("Assets/XML/Item_Item.XML").Completed += (obj) => {
            TextAsset text = obj.Result;
            xmlDoc.LoadXml(text.ToString());

            XmlNodeList nodeList = xmlDoc.SelectSingleNode("Item").ChildNodes;
            foreach (XmlNode child in nodeList)
            {
                ItemInfo curItem = new ItemInfo();
                curItem.My_id = int.Parse(child["id"].InnerText);
                curItem.My_name = child["name"].InnerText;
                curItem.My_brandId = int.Parse(child["brand"].InnerText);
                curItem.My_typeId = int.Parse(child["type"].InnerText);
                curItem.My_TeamPrice = int.Parse(child["teamPrice"].InnerText);
                curItem.My_BrokerPrice = int.Parse(child["brokerPrice"].InnerText);
                curItem.My_RetailPrice = int.Parse(child["retailPrice"].InnerText);
                curItem.My_TaobaoPrice = int.Parse(child["taobaoPrice"].InnerText);
                curItem.My_source = child["source"].InnerText;
                curItem.My_size = child["size"].InnerText;
                curItem.My_taste = child["taste"].InnerText;
                curItem.My_desc = child["desc"].InnerText;
                curItem.My_tip = child["tip"].InnerText;

                DataManager.AllItemInfos.Add(curItem);
            }
            Debug.Log("加载item xml资源完毕 产品数量：" + DataManager.AllItemInfos.Count);

            EventCenter.Broadcast(EventType.UpdateMainPanel);
        };
    }
}
