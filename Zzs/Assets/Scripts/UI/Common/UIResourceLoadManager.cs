using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class UIResourceLoadManager : UnitySingleton<UIResourceLoadManager>
{

    private Dictionary<string, SpriteAtlas> mUISpriteAtlasDic = new Dictionary<string, SpriteAtlas>();

    private AsyncOperationHandle<SpriteAtlas> handle;
    private void Awake()
    {
        Addressables.LoadAssetAsync<SpriteAtlas>("LittleIcon").Completed += (obj) =>
        {
            handle = obj;
            if (handle.Result != null)
            {
                Debug.Log("����ͼ��LittleIcon���");
            }
            else
            {
                Debug.LogError("����ͼ��LittleIconʧ�� ���飡");
            }
        };
    }


    public SpriteAtlas GetSpriteAtlas(string _atlasName)
    {
        if (mUISpriteAtlasDic.ContainsKey(_atlasName))
        {
            if (mUISpriteAtlasDic[_atlasName] == null) mUISpriteAtlasDic[_atlasName] = handle.Result;
        }
        else
        {
            mUISpriteAtlasDic.Add(_atlasName, handle.Result);
        }
        var ans = mUISpriteAtlasDic[_atlasName];
        return ans;
    }

    public Sprite LoadSprite(string _atlasName, string _spriteName)
    {
        Sprite tempSprite = null;
        SpriteAtlas tempAtlas = GetSpriteAtlas(_atlasName);
        if (tempAtlas != null)
        {
            tempSprite = tempAtlas.GetSprite(_spriteName);
        }
        else
        {
            Debug.LogError("ȱ��ͼ����" + _atlasName);
            return null;
        }
        if(tempSprite == null)
        {
            Debug.LogError("ͼ����" + _atlasName + "��ȱ�٣�" + _spriteName);
            return tempAtlas.GetSprite("cbba���Ҽ�_little");
        }
        return tempSprite;
    }

    public Sprite[] LoadSprites(string _atlasName, Sprite[] _spriteArray)
    {
        SpriteAtlas tempAtlas = GetSpriteAtlas(_atlasName);
        if (tempAtlas != null)
        {
            if (_spriteArray == null || _spriteArray.Length < tempAtlas.spriteCount) _spriteArray = new Sprite[tempAtlas.spriteCount];
            if (tempAtlas != null) tempAtlas.GetSprites(_spriteArray);
        }
        return _spriteArray;
    }


    public string GetSpritePath(ItemInfo info,int num)
    {
        string ans ="";
        ans += info.name;
        ans += "_";
        ans += num.ToString();
        ans += ".jpg";
        return ans;
    }

    public string GetSpriteFolderPath(ItemInfo info)
    {
        string ans = "Assets/Resources_Pack/";
        ans += info.typeId.ToString();
        ans += "/";
        ans += info.name;

        return ans;
    }

    //ֻ��Ӧ��untiy�༭���� �������Ӧ
    public int GetIconCount(ItemInfo info)
    {
        string path = GetSpriteFolderPath(info);
        DirectoryInfo folder = new DirectoryInfo(path);
        if (folder.GetDirectories().Length == 0)
        {
            return folder.GetFiles("*.jpg").Length - 1;
        }
        else
        {
            Debug.LogError("·������ ���飺" + path);
            return 0;
        }
    }

    public Sprite GetTexture(string path)
    {
        Sprite sprite = null;
        
        return sprite;
    }
}

public class UnitySingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    GameObject tempObject = new GameObject();
                    tempObject.hideFlags = HideFlags.HideAndDontSave;
                    _instance = (T)tempObject.AddComponent(typeof(T));
                    Object.DontDestroyOnLoad(tempObject);
                }
            }
            return _instance;
        }
    }
}
