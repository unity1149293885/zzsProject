using OfficeOpenXml;
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
    /// 检查图集icon拼写错误
    /// </summary>
    [MenuItem("Zzs工具/检查图集icon拼写错误")]
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