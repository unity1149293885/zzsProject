using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Data;
using Excel;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using UnityEditor;
using System;

public static class CheckPicRes
{
    public static XmlDocument doc = null;

    public static XmlElement root = null;
    //完整存放地址+名字+后缀
    public static string XmlPath = ExcelConfig.XMLExportPath + "PicCount" + ".XML";

    public static string path = Application.dataPath + "/Resources_Pack";

    public delegate void MyCallBack(DirectoryInfo folder);

    /// <summary>
    /// 检查图集icon拼写错误
    /// </summary>
    [MenuItem("Zzs工具/资源/icon拼写错误检查&&生成资源XML")]
    public static void CheckLittleRoot()
    {
        Debug.Log("检查拼写的工作目录：" + path);

        //创建节点
        doc = new XmlDocument();
        root = doc.CreateElement("Root");
        doc.AppendChild(root);

        Traverse(path, (folder) => {
            //创建节点
            XmlElement data = doc.CreateElement(folder.Name);

            root.AppendChild(data);
            data.InnerText = (folder.GetFiles("*.jpg").Length - 1).ToString();
        });

        doc.Save(XmlPath);
        AssetDatabase.Refresh();

        Debug.Log("资源信息XML生成成功！：");
    }

    //递归
    public static void Traverse(string path, MyCallBack callback = null)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
        if (folder.GetDirectories().Length == 0)
        {
            //下边没有文件夹了
            string name = folder.Name + "_little.jpg";

            bool ishave = false;
            foreach (var file in folder.GetFiles())
            {
                if (file.Name == name)
                {
                    ishave = true;
                }
            }

            if (ishave == false)
            {
                Debug.LogError("文件夹：" + folder.FullName + "下 缺少图标icon：" + name);
            }
            else
            {
                if (callback != null)
                {
                    callback(folder);
                }
            }
        }
        else
        {
            foreach (var f in folder.GetDirectories())
            {
                Traverse(f.FullName, callback);
            }
        }
    }
}