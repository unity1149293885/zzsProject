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

public class ExcelTool
{
    /// <summary>
    /// 读取excel文件内容
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="columnNum">行数</param>
    /// <param name="rowNum">列数</param>
    /// <returns></returns>
    static ExcelWorksheet ReadExcelContext(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        ExcelPackage excelPackage = new ExcelPackage(fileInfo);
        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

        return worksheet;
    }

    /// <summary>
    /// 读取表数据，生成对应的数组
    /// </summary>
    /// <param name="filePath">excel文件全路径</param>
    /// <returns>Item数组</returns>
    public static UserManager CreateUserArrayWithExcel(string filePath)
    {
        ExcelWorksheet worksheet = ReadExcelContext(filePath);

        List<UserInfo> infos = new List<UserInfo>();
      
        int index = 2;
        while (index > 0)
        {
            if (worksheet.Cells[index, 1].Value == null) break;

            UserInfo item = new UserInfo();
            //解析每列的数据
            item.id = worksheet.Cells[index, 1].Value.ToString();
            item.name = worksheet.Cells[index, 2].Value.ToString();
            item.phone = worksheet.Cells[index, 3].Value.ToString();
            item.UserType = worksheet.Cells[index, 4].Value.ToString();
            infos.Add(item);
            index++;

        }

        UserManager res = new UserManager();
        res.UserList = infos;
        return res;
    }

    public static ItemManager CreasteItemArrayWithExcel(string filePath)
    {
        ExcelWorksheet worksheet = ReadExcelContext(filePath);

        List<ItemInfo> infos = new List<ItemInfo>();
        int i = 2;
        while (i > 0)
        {
            if (worksheet.Cells[i, 1].Value == null) break;

            ItemInfo item = new ItemInfo();
            //解析每列的数据
            //Debug.LogError(worksheet.Cells[i, 2].Value.ToString());
            item.id = worksheet.Cells[i, 1].Value.ToString();
            item.name = worksheet.Cells[i, 2].Value.ToString();
            item.brand = worksheet.Cells[i, 3].Value.ToString();
            item.type = worksheet.Cells[i, 4].Value.ToString();
            item.TeamPrice = worksheet.Cells[i, 5].Value.ToString();
            item.BrokerPrice = worksheet.Cells[i, 6].Value.ToString();
            item.RetailPrice = worksheet.Cells[i, 7].Value.ToString();
            item.TaobaoPrice = worksheet.Cells[i, 8].Value.ToString();
            item.source = worksheet.Cells[i, 9].Value.ToString();
            item.size = worksheet.Cells[i, 10].Value.ToString();
            item.taste = worksheet.Cells[i, 11].Value.ToString();
            item.desc = worksheet.Cells[i, 12].Value.ToString();
            item.tip = worksheet.Cells[i, 13].Value.ToString();

            infos.Add(item);
            i++;
        }

        ItemManager res = new ItemManager();
        res.ItemList = infos;
        return res;
    }


}

public class ExcelConfig
{
    /// <summary>
    /// 存放excel表文件夹的的路径，本例Excel表放在了"Assets/Excels/"当中
    /// </summary>
    public static readonly string excelsFolderPath = Application.dataPath + "/XML/";

    /// <summary>
    /// 要读取的用户 Excel文件名称 -- 后缀为xlsx
    /// </summary>
    public static readonly string UserexcelName = "用户.xlsx";

    /// <summary>
    /// 要读取的物品 Excel文件名称 -- 后缀为xlsx
    /// </summary>
    public static readonly string ItemexcelName = "物品.xlsx";

    /// <summary>
    /// 存放Excel转化后文件的文件夹路径
    /// </summary>
    public static readonly string assetPath = "Assets/XML/";

    /// <summary>
    /// 保存处理后user数据文件名称
    /// </summary>
    public static readonly string UserInfoXML = "UserConfig";
    /// <summary>
    /// 保存处理后item数据文件名称
    /// </summary>
    public static readonly string ItemInfoXML = "ItemConfig";
}
