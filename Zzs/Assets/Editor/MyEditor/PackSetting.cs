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
    public bool isConnectNet = false;//�Ƿ�������

    public bool isOpenDebug = true;//�Ƿ���ʾdebug���棿

    public bool isDirectLogin = true;//�Ƿ�ֱ�ӵ�¼��ȥ �����߼��
}



[CustomEditor(typeof(PackSetting))]
public class ScoreDataEditor : Editor
{
    public bool isConnectNet;
    public bool isOpenDebug;
    public bool isDirectLogin;
    public override void OnInspectorGUI()
    {
        GUILayout.Label("����������ã�");
        isConnectNet = GUILayout.Toggle(isConnectNet, "�Ƿ�����", GUILayout.Height(20), GUILayout.Width(150));
        isOpenDebug = GUILayout.Toggle(isOpenDebug, "�Ƿ���debug", GUILayout.Height(20), GUILayout.Width(150));
        isDirectLogin = GUILayout.Toggle(isDirectLogin, "�Ƿ�������¼���", GUILayout.Height(20), GUILayout.Width(150));

        GUILayout.Label("����ѡ�");

        if (GUILayout.Button("һ��������Ϸ��������"))
        {
            PackGame.UpdateGameGonfig();
        }
        if (GUILayout.Button("�����������������ã�"))
        {
            //�����ļ�����
            XMLTools.UpdateGameConfigXML(isConnectNet, isOpenDebug, isDirectLogin);

            //��ʼ����
            PackGame.StartPack();
        }
        if (GUILayout.Button("�������������������"))
        {
            //��ʼ����
            PackGame.BuildAddressablesAndPlayer();
        }

        if (GUILayout.Button("�����ȸ���"))
        {
            //�����ļ�����
            PackGame.UpdateGameGonfig();

            //���ȸ���
            var path = ContentUpdateScript.GetContentStateDataPath(true);
            if (!string.IsNullOrEmpty(path))
            {
                ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings, path);
            }
        }

        
    }
}
