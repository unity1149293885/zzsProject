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
    }
}
