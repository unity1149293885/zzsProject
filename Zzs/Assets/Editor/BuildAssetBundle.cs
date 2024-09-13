using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// AssetBundle 打包工具
/// </summary>
public class BuildAssetBundle
{

    public static List<FileInfo> AllSprite = new List<FileInfo>();
    /// <summary>
    /// 打包生成所有的AssetBundles（包）
    /// </summary>
    [MenuItem("AssetBundleTools/BuildAllAssetBundles_PC")]
    public static void BuildAllAB_PC()
    {
        // 打包AB输出路径
        string strABOutPAthDir = string.Empty;

        // 获取“StreamingAssets”文件夹路径（不一定这个文件夹，可自定义）
        strABOutPAthDir = Application.streamingAssetsPath;

        // 判断文件夹是否存在，不存在则新建
        if (Directory.Exists(strABOutPAthDir) == false)
        {
            Directory.CreateDirectory(strABOutPAthDir);
        }
        // 打包生成AB包 (目标平台根据需要设置即可)
        BuildPipeline.BuildAssetBundles(strABOutPAthDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }

    /// <summary>
    /// 打包生成所有的AssetBundles（包）
    /// </summary>
    [MenuItem("AssetBundleTools/BuildAllAssetBundles_Android")]
    public static void BuildAllAB_Android()
    {
        // 打包AB输出路径
        string strABOutPAthDir = string.Empty;

        // 获取“StreamingAssets”文件夹路径（不一定这个文件夹，可自定义）
        strABOutPAthDir = Application.streamingAssetsPath;

        // 判断文件夹是否存在，不存在则新建
        if (Directory.Exists(strABOutPAthDir) == false)
        {
            Directory.CreateDirectory(strABOutPAthDir);
        }
        // 打包生成AB包 (目标平台根据需要设置即可)
        BuildPipeline.BuildAssetBundles(strABOutPAthDir, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    private static string _extension = "*.jpg";
    //[MenuItem("AssetBundleTools/FileSeachWithExtension")]
    static void StartSearch()
    {
        //清空重新添加
        AllSprite.Clear();
        string assetPath = "D:/SoftWear/VisualStudioProject/ZzsCarDream/Assets";
        Search(assetPath);
    }
    public static void Search(string assetPath)
    {
        // 将 GUID 转换为 路径
        // 判断是否文件夹
        if (Directory.Exists(assetPath))
        {
            DirectoryInfo dInfo = new DirectoryInfo(assetPath);
            if (dInfo == null)
            {
                Debug.LogError("dInfo == null");
                return;
            }
            // 获取 文件夹以及子文件加中所有扩展名为  _extension 的文件
            FileInfo[] fileInfoArr = dInfo.GetFiles(_extension);
            for (int i = 0; i < fileInfoArr.Length; ++i)
            {
                string fullName = fileInfoArr[i].FullName;
                AllSprite.Add(fileInfoArr[i]);
            }

            FileInfo[] files = dInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
            //文件夹下一层的所有文件夹
            DirectoryInfo[] folders = dInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < folders.Length; i++)
            {
                //folders[i].FullName：硬盘上的完整路径名称
                //folders[i].Name：文件夹名称
                int folderAssetsIndex = folders[i].FullName.IndexOf("Assets");
                //从Assets开始取路径
                string folderPath = folders[i].FullName.Substring(folderAssetsIndex);
                Search(folderPath);//递归遍历所有子文件夹
            }
        }
        else
        {
            Debug.LogError("没有该路径！");
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
            if (ai == null) Debug.LogError("ai为空:"+ asset_path);

            ai.assetBundleName = assetbundle_name;
        }
    }
}