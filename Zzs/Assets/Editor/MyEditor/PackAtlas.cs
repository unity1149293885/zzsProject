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
        [MenuItem("Zzs工具/资源/生成图集")]
        public static void GenAtlas()
        {
            List<string> allSprites = GetAllLittleSpriteName();

            Sprite[] sprites = new Sprite[allSprites.Count];
            for (int i = 0; i < allSprites.Count; i++)
            {
                sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(allSprites[i]);
            }

            // 打包 Sprite 到 Texture2D 图集中
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

            // 导出 Sprite Atlas 到指定的路径
            string outputPath = Path.Combine("Assets/Atlas/", "LittleIcon.spriteatlas");
            AssetDatabase.CreateAsset(spriteAtlas, outputPath);
            AssetDatabase.SaveAssets();

            Debug.Log("图集已生成！");
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


        [MenuItem("Zzs工具/资源/jpg自动设Group")]
        public static void AutoSetJpgs()
        {
            string path = CheckPicRes.path;

            string groupName = "icon_sprite";
            // 获取 AddressableAssetSettings 对象
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            // 查找指定名称的 Group 或创建新 Group
            var group = settings.FindGroup(groupName) ?? settings.CreateGroup(groupName, false, false, true, null);

            CheckPicRes.Traverse(path, (folder) =>
            {
                foreach (var it in folder.GetFiles("*.jpg"))
                {
                    //刷新jpg格式
                    TextureImporter textureImporter = AssetImporter.GetAtPath(GetLoadPath(it.FullName)) as TextureImporter;
                    if(ModifyFilesInBulk.Instance == null)
                    {
                        EditorWindow window = EditorWindow.GetWindow(typeof(ModifyFilesInBulk));
                        window.Show();
                    }
                    ModifyFilesInBulk.Instance.SetTextureFormat(textureImporter);

                    //添加group
                    if (!it.Name.Contains("little"))
                    {
                        // 创建或更新指定路径的 Addressable Entry，并将其添加到 Group 中
                        var guid = AssetDatabase.AssetPathToGUID(GetLoadPath(it.FullName));
                        var entry = settings.CreateOrMoveEntry(guid, group);
                        entry.address = it.Name;//这个就是加载时的key
                        entry.SetLabel("Texture", true);
                    }
                }
            });

            // 保存修改并刷新编辑器界面
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}