using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEditor;
using UnityEngine;

// <summary>
/// 编辑器扩展 将xlsx文件转换为其他格式
/// </summary>
public class ExcelBuild : Editor
{
    /// <summary>
    /// 转换为 XML
    /// </summary>
    [MenuItem("CustomEditor/CreateUserExcelXML")]
    public static void CreateUserExcelXML()
    {
        UserManager excelManager = ExcelTool.CreateUserArrayWithExcel(ExcelConfig.excelsFolderPath + ExcelConfig.UserexcelName);


        // 文件保存路径
        string filePath = ExcelConfig.assetPath + ExcelConfig.UserInfoXML + ".XML";

        XmlDocument doc = new XmlDocument();
        //创建根节点
        XmlElement root = doc.CreateElement("UserInfos");
        // 设置根节点
        doc.AppendChild(root);

        for (int i = 0; i < excelManager.UserList.Count; i++)
        {
            // 一级子节点
            XmlElement data = doc.CreateElement("Data"+(i+1).ToString());
            // 设置和根节点的关系
            root.AppendChild(data);

            //创建数据子节点   
            XmlElement id = doc.CreateElement("id");
            XmlElement name = doc.CreateElement("name");
            XmlElement phone = doc.CreateElement("phone");
            XmlElement isManager = doc.CreateElement("isManager");

            // 设置节点间关系
            data.AppendChild(id);
            data.AppendChild(name);
            data.AppendChild(phone);
            data.AppendChild(isManager);
            // 数据
            id.InnerText = excelManager.UserList[i].id.ToString();
            name.InnerText = excelManager.UserList[i].name;
            phone.InnerText = excelManager.UserList[i].phone;
            isManager.InnerText = excelManager.UserList[i].UserType;
        }

        // 保存到本地
        doc.Save(filePath);

        AssetDatabase.Refresh();
        Debug.LogError("成功刷新user表格！共有元素个数:" + excelManager.UserList.Count);
    }

    [MenuItem("CustomEditor/CreateItemExcelXML")]
    public static void CreateItemExcelXML()
    {
        ItemManager excelManager = ExcelTool.CreasteItemArrayWithExcel(ExcelConfig.excelsFolderPath + ExcelConfig.ItemexcelName);


        // 文件保存路径
        string filePath = ExcelConfig.assetPath + ExcelConfig.ItemInfoXML + ".XML";

        XmlDocument doc = new XmlDocument();
        //创建根节点
        XmlElement root = doc.CreateElement("ItemInfos");
        // 设置根节点
        doc.AppendChild(root);

        for (int i = 0; i < excelManager.ItemList.Count; i++)
        {
            // 一级子节点
            XmlElement data = doc.CreateElement("Data" + (i + 1).ToString());
            // 设置和根节点的关系
            root.AppendChild(data);

            //创建数据子节点   
            XmlElement id = doc.CreateElement("id");
            XmlElement name = doc.CreateElement("name");
            XmlElement brand = doc.CreateElement("brand");
            XmlElement type = doc.CreateElement("type");
            XmlElement teamprice = doc.CreateElement("teamprice");
            XmlElement brokerprice = doc.CreateElement("brokerprice");
            XmlElement retailprice = doc.CreateElement("retailprice");
            XmlElement taobaoprice = doc.CreateElement("taobaoprice");
            XmlElement source = doc.CreateElement("source");
            XmlElement size = doc.CreateElement("size");
            XmlElement taste = doc.CreateElement("taste");
            XmlElement desc = doc.CreateElement("desc");
            XmlElement tip = doc.CreateElement("tip");

            // 设置节点间关系
            data.AppendChild(id);
            data.AppendChild(name);
            data.AppendChild(brand);
            data.AppendChild(type);
            data.AppendChild(teamprice);
            data.AppendChild(brokerprice);
            data.AppendChild(retailprice);
            data.AppendChild(taobaoprice);
            data.AppendChild(source);
            data.AppendChild(size);
            data.AppendChild(taste);
            data.AppendChild(desc);
            data.AppendChild(tip);

            // 数据
            id.InnerText = excelManager.ItemList[i].id;
            name.InnerText = excelManager.ItemList[i].name;
            brand.InnerText = excelManager.ItemList[i].brand;
            type.InnerText = excelManager.ItemList[i].type;
            teamprice.InnerText = excelManager.ItemList[i].TeamPrice;
            brokerprice.InnerText = excelManager.ItemList[i].BrokerPrice;
            retailprice.InnerText = excelManager.ItemList[i].RetailPrice;
            taobaoprice.InnerText = excelManager.ItemList[i].TaobaoPrice;
            source.InnerText = excelManager.ItemList[i].source;
            size.InnerText = excelManager.ItemList[i].size;
            taste.InnerText = excelManager.ItemList[i].taste;
            desc.InnerText = excelManager.ItemList[i].desc;
            tip.InnerText = excelManager.ItemList[i].tip;
        }

        // 保存到本地
        doc.Save(filePath);

        AssetDatabase.Refresh();
        Debug.LogError("成功刷新item表格！共有元素个数:" + excelManager.ItemList.Count);
    }


    /// <summary>
    /// 检查图集icon拼写错误
    /// </summary>
    [MenuItem("CustomEditor/检查图集icon拼写错误")]
    public static void CheckLittleRoot()
    {
        string path = Application.dataPath + "/Resources_Pack";
        Debug.Log("检查拼写的工作目录：" + path);

        Traverse(path);
    }

    //递归
    private static void Traverse(string path)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
       
        if(folder.GetDirectories().Length == 0)
        {
            //下边没有文件夹了
            string name = folder.Name + "_little.jpg";

            bool ishave = false;
            foreach(var file in folder.GetFiles())
            {
                if(file.Name == name)
                {
                    ishave = true;
                }
            }

            if (ishave == false)
            {
                Debug.LogError("文件夹：" + folder.FullName + "下 缺少图标icon：" + name);
            }
        }
        else
        {
            foreach(var f in folder.GetDirectories())
            {
                Traverse(f.FullName);
            }
        }
    }
}