using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

public class PackPc
{
    [MenuItem("Zzs工具/打包/PC整包")]
    public static void StartPack()
    {
        UpdateGameGonfig();
        //打包整体资源
        BuildAddressables();
        //打包
        BuildAddressablesAndPlayer();
    }

    [MenuItem("Zzs工具/打包/PC热更包")]
    public static void StartHotPack()
    {
        UpdateGameGonfig();

        
    }

    [MenuItem("Zzs工具/打包/一键更新游戏所有配置")]
    public static void UpdateGameGonfig()
    {
        //1.更新最新表格数据
        ExcelTool.ExcelToXML();

        //2.更新资源数据
        CheckPicRes.CheckLittleRoot();

        //3.生成图集
        PackAtlas.GenAtlas();

        //4.将jpg自动放入group中
        PackAtlas.AutoSetJpgs();

        Debug.Log("一键更新完成！");
    }

    //打包整包Pc
    public static void BuildAddressablesAndPlayer()
    {
        var options = new BuildPlayerOptions();
        BuildPlayerOptions playerSettings = BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(options);

        BuildPipeline.BuildPlayer(playerSettings);
    }

    public static string build_script
            = "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedMode.asset";
    public static string settings_asset
        = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
    public static string profile_name = "Default";
    private static AddressableAssetSettings settings;

    static void getSettingsObject(string settingsAsset)
    {
        // This step is optional, you can also use the default settings:
        //settings = AddressableAssetSettingsDefaultObject.Settings;

        settings
            = AssetDatabase.LoadAssetAtPath<ScriptableObject>(settingsAsset)
                as AddressableAssetSettings;

        if (settings == null)
            Debug.LogError($"{settingsAsset} couldn't be found or isn't " +
                           $"a settings object.");
    }

    static void setProfile(string profile)
    {
        string profileId = settings.profileSettings.GetProfileId(profile);
        if (String.IsNullOrEmpty(profileId))
            Debug.LogWarning($"Couldn't find a profile named, {profile}, " +
                             $"using current profile instead.");
        else
            settings.activeProfileId = profileId;
    }

    static void setBuilder(IDataBuilder builder)
    {
        int index = settings.DataBuilders.IndexOf((ScriptableObject)builder);

        if (index > 0)
            settings.ActivePlayerDataBuilderIndex = index;
        else
            Debug.LogWarning($"{builder} must be added to the " +
                             $"DataBuilders list before it can be made " +
                             $"active. Using last run builder instead.");
    }

    static bool buildAddressableContent()
    {
        AddressableAssetSettings
            .BuildPlayerContent(out AddressablesPlayerBuildResult result);
        bool success = string.IsNullOrEmpty(result.Error);

        if (!success)
        {
            Debug.LogError("Addressables build error encountered: " + result.Error);
        }
        return success;
    }

    [MenuItem("Zzs工具/资源/资源打整包")]
    public static bool BuildAddressables()
    {
        getSettingsObject(settings_asset);
        setProfile(profile_name);
        IDataBuilder builderScript
          = AssetDatabase.LoadAssetAtPath<ScriptableObject>(build_script) as IDataBuilder;

        if (builderScript == null)
        {
            Debug.LogError(build_script + " couldn't be found or isn't a build script.");
            return false;
        }

        setBuilder(builderScript);

        return buildAddressableContent();
    }
}


