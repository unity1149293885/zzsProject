using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// AssetBundle �������
/// </summary>
public class BuildAssetBundle
{

    public static List<FileInfo> AllSprite = new List<FileInfo>();
    /// <summary>
    /// ����������е�AssetBundles������
    /// </summary>
    [MenuItem("AssetBundleTools/BuildAllAssetBundles_PC")]
    public static void BuildAllAB_PC()
    {
        // ���AB���·��
        string strABOutPAthDir = string.Empty;

        // ��ȡ��StreamingAssets���ļ���·������һ������ļ��У����Զ��壩
        strABOutPAthDir = Application.streamingAssetsPath;

        // �ж��ļ����Ƿ���ڣ����������½�
        if (Directory.Exists(strABOutPAthDir) == false)
        {
            Directory.CreateDirectory(strABOutPAthDir);
        }
        // �������AB�� (Ŀ��ƽ̨������Ҫ���ü���)
        BuildPipeline.BuildAssetBundles(strABOutPAthDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }

    /// <summary>
    /// ����������е�AssetBundles������
    /// </summary>
    [MenuItem("AssetBundleTools/BuildAllAssetBundles_Android")]
    public static void BuildAllAB_Android()
    {
        // ���AB���·��
        string strABOutPAthDir = string.Empty;

        // ��ȡ��StreamingAssets���ļ���·������һ������ļ��У����Զ��壩
        strABOutPAthDir = Application.streamingAssetsPath;

        // �ж��ļ����Ƿ���ڣ����������½�
        if (Directory.Exists(strABOutPAthDir) == false)
        {
            Directory.CreateDirectory(strABOutPAthDir);
        }
        // �������AB�� (Ŀ��ƽ̨������Ҫ���ü���)
        BuildPipeline.BuildAssetBundles(strABOutPAthDir, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    private static string _extension = "*.jpg";
    //[MenuItem("AssetBundleTools/FileSeachWithExtension")]
    static void StartSearch()
    {
        //����������
        AllSprite.Clear();
        string assetPath = "D:/SoftWear/VisualStudioProject/ZzsCarDream/Assets";
        Search(assetPath);
    }
    public static void Search(string assetPath)
    {
        // �� GUID ת��Ϊ ·��
        // �ж��Ƿ��ļ���
        if (Directory.Exists(assetPath))
        {
            DirectoryInfo dInfo = new DirectoryInfo(assetPath);
            if (dInfo == null)
            {
                Debug.LogError("dInfo == null");
                return;
            }
            // ��ȡ �ļ����Լ����ļ�����������չ��Ϊ  _extension ���ļ�
            FileInfo[] fileInfoArr = dInfo.GetFiles(_extension);
            for (int i = 0; i < fileInfoArr.Length; ++i)
            {
                string fullName = fileInfoArr[i].FullName;
                AllSprite.Add(fileInfoArr[i]);
            }

            FileInfo[] files = dInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
            //�ļ�����һ��������ļ���
            DirectoryInfo[] folders = dInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < folders.Length; i++)
            {
                //folders[i].FullName��Ӳ���ϵ�����·������
                //folders[i].Name���ļ�������
                int folderAssetsIndex = folders[i].FullName.IndexOf("Assets");
                //��Assets��ʼȡ·��
                string folderPath = folders[i].FullName.Substring(folderAssetsIndex);
                Search(folderPath);//�ݹ�����������ļ���
            }
        }
        else
        {
            Debug.LogError("û�и�·����");
        }
    }

    [MenuItem("AssetBundleTools/AutoSet AssetBundleName")]
    public static void AutoSetBundleName()
    {
        string path = Path.Combine(Application.dataPath, "Resource");
        string assetbundle_name = "icon_sprite";

        StartSearch();
        for(int i = 0; i < AllSprite.Count; i++)
        {
            string asset_path = AllSprite[i].FullName.Substring(44, AllSprite[i].FullName.Length - 44);
            Debug.LogError(asset_path);

            AssetImporter ai = AssetImporter.GetAtPath(asset_path);
            if (ai == null) Debug.LogError("aiΪ��:"+ asset_path);

            ai.assetBundleName = assetbundle_name;
        }
    }
}