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

namespace EditorTools
{
    public static class PackAtlas
    {
        [MenuItem("Zzs����/��Դ/����ͼ��")]
        public static void GenAtlas()
        {
            List<string> allSprites = GetAllLittleSpriteName();

            Sprite[] sprites = new Sprite[allSprites.Count];
            for (int i = 0; i < allSprites.Count; i++)
            {
                sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(allSprites[i]);
            }

            // ��� Sprite �� Texture2D ͼ����
            Texture2D texture = new Texture2D(2048, 2048);
            Rect[] rects = texture.PackTextures(sprites.Select(s => s.texture).ToArray(), 0, 2048);

            var spriteAtlas = new SpriteAtlas();
            spriteAtlas.Add(new[] { texture });

            for (int i = 0; i < sprites.Length; i++)
            {
                Sprite sprite = sprites[i];
                var rect = rects[i];

                spriteAtlas.Add(new[] { sprite });
            }

            // ���� Sprite Atlas ��ָ����·��
            string outputPath = Path.Combine("Assets/Atlas/", "LittleIcon.spriteatlas");
            AssetDatabase.CreateAsset(spriteAtlas, outputPath);
            AssetDatabase.SaveAssets();

            Debug.Log("ͼ�������ɣ�");
        }

        public static List<string> GetAllLittleSpriteName()
        {
            string path = CheckPicRes.path;
            List<string> ans = new List<string>();
            DirectoryInfo folder = new DirectoryInfo(path);

            CheckPicRes.Traverse(path, (folder) =>
            {
                string path = GetLoadPath(folder.FullName + "\\" + folder.Name + "_little.jpg");
                ans.Add(path);
            });

            return ans;
        }

        public static string GetLoadPath(string path)
        {
            int index = 0;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == 'A')
                {
                    index = i;
                }
            }
            return path.Substring(index, path.Length - index).Replace("\\", "/");
        }


        [MenuItem("Zzs����/��Դ/jpg�Զ���Group")]
        public static void AutoSetJpgs()
        {
            string path = CheckPicRes.path;

            string groupName = "icon_sprite";
            // ��ȡ AddressableAssetSettings ����
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            // ����ָ�����Ƶ� Group �򴴽��� Group
            var group = settings.FindGroup(groupName) ?? settings.CreateGroup(groupName, false, false, true, null);

            CheckPicRes.Traverse(path, (folder) =>
            {
                foreach (var it in folder.GetFiles("*.jpg"))
                {
                    //ˢ��jpg��ʽ
                    TextureImporter textureImporter = AssetImporter.GetAtPath(GetLoadPath(it.FullName)) as TextureImporter;
                    if(ModifyFilesInBulk.Instance == null)
                    {
                        EditorWindow window = EditorWindow.GetWindow(typeof(ModifyFilesInBulk));
                        window.Show();
                    }
                    ModifyFilesInBulk.Instance.SetTextureFormat(textureImporter);

                    //���group
                    if (!it.Name.Contains("little"))
                    {
                        // ���������ָ��·���� Addressable Entry����������ӵ� Group ��
                        var guid = AssetDatabase.AssetPathToGUID(GetLoadPath(it.FullName));
                        var entry = settings.CreateOrMoveEntry(guid, group);
                        entry.address = it.Name;//������Ǽ���ʱ��key
                        entry.SetLabel("Texture", true);
                    }
                }
            });

            // �����޸Ĳ�ˢ�±༭������
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}