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

public class ExcelConfig
{
    /// <summary>
    /// 存放excel表文件夹的的路径，本例Excel表放在了"Assets/Excels/"当中
    /// </summary>
    public static readonly string excelsFolderPath = Application.dataPath + "/Excel/";


    /// <summary>
    /// 存放Excel转化后文件的文件夹路径
    /// </summary>
    public static readonly string XMLExportPath = "Assets/XML/";

}

public class ExcelTool
{
    [MenuItem("Zzs工具/刷表---Excel表格转成XML数据")]
    public static void ExcelToXML()
    {
        DirectoryInfo folder = new DirectoryInfo(ExcelConfig.excelsFolderPath);
        foreach (FileInfo excel in folder.GetFiles("*.xlsx"))
        {
            //遍历所有excel表
            string excelName = excel.Name;

            //excel表
            ExcelPackage excelPackage = new ExcelPackage(excel);
            //遍历所有sheet
            for (int SheetIndex = 1; SheetIndex <= excelPackage.Workbook.Worksheets.Count; SheetIndex++)
            {
                ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[SheetIndex];
                string sheetName = sheet.Name;

                //XML名字为:表格名字_sheet名字(这里的.name带有xlxs后缀 要去除一下）
                string XmlName = excelName.Substring(0, excelName.IndexOf('.')) + "_" + sheetName;

                //完整存放地址+名字+后缀
                string XmlPath = ExcelConfig.XMLExportPath + XmlName + ".XML";

                XmlDocument doc = new XmlDocument();
                //创建根节点
                XmlElement root = doc.CreateElement(sheetName);
                doc.AppendChild(root);

                //拿到第一行数据，待会做赋值处理
                List<string> TitleNames = new List<string>() { "" };
                for (int rowIndex = 1; rowIndex <= sheet.Dimension.Columns; rowIndex++)
                {
                    string rowData = sheet.Cells[1, rowIndex].Value.ToString();

                    TitleNames.Add(rowData);
                }

                //遍历每一行数据
                int ColumnIndex = 2;
                while (ColumnIndex > 0)
                {
                    //没有数据就跳出循环
                    if (sheet.Cells[ColumnIndex, 1].Value == null) break;

                    //开始写入Xml
                    // 一级子节点
                    XmlElement Columndata = doc.CreateElement("data" + (ColumnIndex + 1).ToString());
                    // 设置和根节点的关系
                    root.AppendChild(Columndata);

                    //某一行的所有列
                    for (int rowIndex = 1; rowIndex <= sheet.Dimension.Columns; rowIndex++)
                    {
                        string rowData = sheet.Cells[ColumnIndex, rowIndex].Value.ToString();

                        //当前格子的数据
                        XmlElement Rowdata = doc.CreateElement(TitleNames[rowIndex]);
                        Columndata.AppendChild(Rowdata);
                        Rowdata.InnerText = rowData;
                    }
                    ColumnIndex++;
                }
                doc.Save(XmlPath);
            }
        }
        AssetDatabase.Refresh();

        Debug.Log("刷表完成！");
    }
}

public static class CheckPicRes
{
    public static XmlDocument doc = null;

    public static XmlElement root = null;
    //完整存放地址+名字+后缀
    public static string XmlPath = ExcelConfig.XMLExportPath + "PicCount" + ".XML";

    /// <summary>
    /// 检查图集icon拼写错误
    /// </summary>
    [MenuItem("Zzs工具/检查--图集icon拼写错误")]
    public static void CheckLittleRoot()
    {
        string path = Application.dataPath + "/Resources_Pack";
        Debug.Log("检查拼写的工作目录：" + path);

        //创建节点
        doc = new XmlDocument();
        root = doc.CreateElement("Root");
        doc.AppendChild(root);

        Traverse(path);

        doc.Save(XmlPath);
        AssetDatabase.Refresh();
    }

    //递归
    private static void Traverse(string path)
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
                //创建节点
                XmlElement data = doc.CreateElement(folder.Name);
                
                root.AppendChild(data);
                data.InnerText = (folder.GetFiles("*.jpg").Length - 1).ToString();
            }
        }
        else
        {
            foreach (var f in folder.GetDirectories())
            {
                Traverse(f.FullName);
            }
        }
    }
}
