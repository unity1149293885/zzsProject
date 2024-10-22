using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorTools;
using UnityEditor;
using System.IO;
using System.Xml;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets;

[CreateAssetMenu(fileName = "PackSetting", menuName = "PackSetting", order = 1)]
public class PackSetting: ScriptableObject
{
    public bool isConnectNet = false;//是否联网？

    public bool isOpenDebug = true;//是否显示debug界面？

    public bool isDirectLogin = true;//是否直接登录上去 无需走检测
}



[CustomEditor(typeof(PackSetting))]
public class ScoreDataEditor : Editor
{
    public bool isConnectNet;
    public bool isOpenDebug;
    public bool isDirectLogin;
    public override void OnInspectorGUI()
    {
        GUILayout.Label("打包参数设置：");
        isConnectNet = GUILayout.Toggle(isConnectNet, "是否联网", GUILayout.Height(20), GUILayout.Width(150));
        isOpenDebug = GUILayout.Toggle(isOpenDebug, "是否开启debug", GUILayout.Height(20), GUILayout.Width(150));
        isDirectLogin = GUILayout.Toggle(isDirectLogin, "是否跳过登录检测", GUILayout.Height(20), GUILayout.Width(150));

        GUILayout.Label("构建选项：");

        if (GUILayout.Button("一键更新游戏所有配置"))
        {
            PackGame.UpdateGameGonfig();
        }
        if (GUILayout.Button("构建整包（更新配置）"))
        {
            //配置文件更新
            XMLTools.UpdateGameConfigXML(isConnectNet, isOpenDebug, isDirectLogin);

            //开始构建
            PackGame.StartPack();
        }
        if (GUILayout.Button("构建整包（单纯打包）"))
        {
            //开始构建
            PackGame.BuildAddressablesAndPlayer();
        }

        if (GUILayout.Button("构建热更包"))
        {
            //配置文件更新
            PackGame.UpdateGameGonfig();

            //打热更包
            var path = ContentUpdateScript.GetContentStateDataPath(true);
            if (!string.IsNullOrEmpty(path))
            {
                ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings, path);
            }
        }

        
    }
}
