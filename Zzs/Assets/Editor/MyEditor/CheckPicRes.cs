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
    //������ŵ�ַ+����+��׺
    public static string XmlPath = ExcelConfig.XMLExportPath + "PicCount" + ".XML";

    public static string path = Application.dataPath + "/Resources_Pack";

    public delegate void MyCallBack(DirectoryInfo folder);

    /// <summary>
    /// ���ͼ��iconƴд����
    /// </summary>
    [MenuItem("Zzs����/��Դ/iconƴд������&&������ԴXML")]
    public static void CheckLittleRoot()
    {
        Debug.Log("���ƴд�Ĺ���Ŀ¼��" + path);

        //�����ڵ�
        doc = new XmlDocument();
        root = doc.CreateElement("Root");
        doc.AppendChild(root);

        Traverse(path, (folder) => {
            //�����ڵ�
            XmlElement data = doc.CreateElement(folder.Name);

            root.AppendChild(data);
            data.InnerText = (folder.GetFiles("*.jpg").Length - 1).ToString();
        });

        doc.Save(XmlPath);
        AssetDatabase.Refresh();

        Debug.Log("��Դ��ϢXML���ɳɹ�����");
    }

    //�ݹ�
    public static void Traverse(string path, MyCallBack callback = null)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
        if (folder.GetDirectories().Length == 0)
        {
            //�±�û���ļ�����
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
                Debug.LogError("�ļ��У�" + folder.FullName + "�� ȱ��ͼ��icon��" + name);
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