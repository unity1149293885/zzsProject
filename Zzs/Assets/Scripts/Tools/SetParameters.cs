#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using System.Reflection;

[CreateAssetMenu(fileName = "new Paramenters", menuName = "SetParameters", order = 1)]
public class SetParameters : ScriptableObject
{
    public List<string> paths = null;

    public TextureImporterType TextureType = TextureImporterType.Default;
    public TextureImporterShape TextureShape = TextureImporterShape.Texture2D;

    public bool sRGB = true;
    public TextureImporterAlphaSource AlphaSource = TextureImporterAlphaSource.FromInput;
    public bool AlphaIsTransparency = false;

    public TextureImporterNPOTScale NonPowerOf2 = TextureImporterNPOTScale.None;
    public bool IsRead = true;
    public bool StreamingMipmaps = false;
    public int MipMapPriority = 0;
    public bool GenerateMipMaps = false;
    public bool BorderMipmap = false;
    public TextureImporterMipFilter MipMapFiltering = TextureImporterMipFilter.BoxFilter;
    public float AlphaCutoffValue = 0.5f;
    public bool MipMapsPreserveCoverage = false;
    public bool FadeoutMipMaps = false;
    public int FadeRangeleft = 0;
    public int FadeRangeright = 1;


    public TextureWrapMode WrapMode = TextureWrapMode.Repeat;
    public FilterMode FilterMode = FilterMode.Bilinear;
    public int AnisoLevel = 1;

    public int DefaultMaxSize = 1024;
    public TextureResizeAlgorithm DefaultResizeAlgorithm = TextureResizeAlgorithm.Mitchell;
    public TextureImporterFormat DefaultFormat = TextureImporterFormat.RGBA32;
    public TextureImporterCompression DefaultCompression = TextureImporterCompression.Uncompressed;
    public bool IsUseCurnchCompre = false;
    public int DefaultCompressionQuality = 50;

    public bool isOverrideIOS = false;
    public int IOSMaxSize = 1024;
    public TextureResizeAlgorithm IOSResizeAlgorithm = TextureResizeAlgorithm.Mitchell;
    public TextureImporterFormat IOSFormat = TextureImporterFormat.RGBA32;
    public int IOSCompressionQuality = 50;

    public bool isOverrideAndroid = false;
    public int AndroidMaxSize = 1024;
    public TextureResizeAlgorithm AndroidResizeAlgorithm = TextureResizeAlgorithm.Mitchell;
    public TextureImporterFormat AndroidFormat = TextureImporterFormat.RGBA32;
    public int AndroidCompressionQuality = 50;
    public AndroidETC2FallbackOverride androidETC2FallbackOverride = AndroidETC2FallbackOverride.UseBuildSettings;
}

public class Secondpop : EditorWindow
{
    void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.Label("是否确认刷新所有路径下的文件");
        GUILayout.Space(20);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("确认"))
        {
            ModifyFilesInBulk.Instance.RefreshData();
            ModifyFilesInBulk.Instance.secondWindow.Close();
        }

        if (GUILayout.Button("返回"))
        {
            ModifyFilesInBulk.Instance.secondWindow.Close();
        }

        GUILayout.EndHorizontal();
    }
}

public class ModifyFilesInBulk : EditorWindow
{
    private static ModifyFilesInBulk instance;

    public static ModifyFilesInBulk Instance { get => instance; set => instance = value; }

    [MenuItem("Assets/png")]
    public static void SetAllTextureType()
    {
        if (Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets).Length == 0)
        {
            SetSections();
            return;
        }

        var arr = Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets);

        string folder = AssetDatabase.GetAssetPath(arr[0]);
        Debug.Log("Reimport Path:" + folder);

        DirectoryInfo direction = new DirectoryInfo(folder);

        FileInfo[] pngFiles = direction.GetFiles("*.png", SearchOption.AllDirectories);

        try
        {
            SetTexture(pngFiles);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }

    #region 参数设置

    string[] TextureTypeOptions = { "Default", "Normal map", "Editor GUI and Legacy GUI", "Sprite(2D and UI)", "Cursor", "Cookie", "Lightmap", "Sigle Channel" };
    int TextureTypeIndex = 0;

    string[] TextureShapeOptions = { "2D", "Cube" };
    int TextureShapeIndex = 0;

    bool isSRGB;

    string[] AlphaSourceOptions = { "None", "Input Texture Alpha", "From Gray Scale" };
    int AlphaSourceIndex = 0;

    bool isAlphaIsTransparency;

    string[] NonPowerof2Options = { "None", "ToNearest", "ToLarger", "ToSmaller" };
    int NonPowerof2Index = 0;

    bool isReadandWriteEnable;

    bool isStreamingMipMaps;

    int MipMapPriority;

    bool isGenerateMipMaps;

    bool isBorderMipMaps;

    string[] MipMapFiteringOptions = { "Box", "Kaiser" };
    int MipMapFiteringIndex = 0;

    bool isMipMapsPreserveCoverage;

    bool isFadeoutMipMaps;

    int FadeRangeleft = 0;
    int FadeRangeright = 1;

    string[] WarpModeOptions = { "Repeat", "Clamp", "Mirror", "Mirror Once" };
    int WarpModeIndex = 0;

    string[] FilterModeOptions = { "Bilinear", "Point(no filter)", "Trilinear" };
    int FilterModeIndex = 0;

    int AnisoLevel;

    string[] DefaultMaxSizeOptions = { "32", "64", "128", "256", "512", "1024", "2048", "4096", "8192" };
    int DefaultMaxSizeIndex = 0;
    string[] DefaultResizeAlgorithmOptions = { "Mitchell", "Bilinear" };
    int DefaultResizeAlgorithmIndex = 0;
    string[] DefaultFormatOptions = { "Automatic", "Alpha 8", "RGB 24 bit", "RGBA 32 bit", "RGBA 16 bit", "R 16 bit", "R 8" };
    int DefaultFormatIndex = 0;
    string[] DefaultCompresssionOptions = { "None", "Low Quality", "Normal Quality", "High Quality" };
    int DefaultCompresssionIndex = 0;
    bool IsUseCurnchCompre = false;
    int DefaultCompresssionQuality = 0;

    bool isOverrideIOS;
    string[] IOSMaxSizeOptions = { "32", "64", "128", "256", "512", "1024", "2048", "4096", "8192" };
    int IOSMaxSizeIndex = 0;
    string[] IOSResizeAlgorithmOptions = { "Mitchell", "Bilinear" };
    int IOSResizeAlgorithmIndex = 0;
    string[] IOSFormatOptions = { "Alpha 8", "RGB 24 bit", "RGBA 32 bit", "RGB 16 bit", "R 16 bit", "RGBA 16 bit", "RGBA Half", "RGB Compressed PVRTC 2 bits", "RGBA Compressed PVRTC 2 bits", "RGB Compressed PVRTC 4 bits", "RGBA Compressed PVRTC 4 bits", "RGB Compressed ETC 4 bits", "R Compressed ETC 4 bits", "RG Compressed EAC 8 bits", "RGB Compressed ETC2 4 bits", "RGB + 1-bit Alpha Compressed ETC2 4 bits", "RGBA Compressed ETC2 8 bits", "RGB Compressed ASTC 4*4 block", "RGB Compressed ASTC 5*5 block", "RGB Compressed ASTC 6*6 block", "RGB Compressed ASTC 8*8 block", "RGB Compressed ASTC 10*10 block", "RGB Compressed ASTC 12*12 block", "RGBA Compressed ASTC 4*4 block", "RGBA Compressed ASTC 5*5 block", "RGBA Compressed ASTC 6*6 block", "RGBA Compressed ASTC 8*8 block", "RGBA Compressed ASTC 10*10 block", "RGBA Compressed ASTC 12*12 block", "R 8", "RGB Crunched ETC", "RGBA Crunched ETC" };
    int IOSFormatIndex = 0;
    string[] IOSCompresssionOptions = { "Fast", "Nromal", "Best" };
    int IOSCompresssionIndex = 0;
    int IOSCompresssionQuality = 0;

    bool isOverrideAndroid;
    string[] AndroidMaxSizeOptions = { "32", "64", "128", "256", "512", "1024", "2048", "4096", "8192" };
    int AndroidMaxSizeIndex = 0;
    string[] AndroidResizeAlgorithmOptions = { "Mitchell", "Bilinear" };
    int AndroidResizeAlgorithmIndex = 0;
    string[] AndroidFormatOptions = { "Alpha 8", "RGB 24 bit", "RGBA 32 bit", "RGB 16 bit", "R 16 bit", "RGBA 16 bit", "RGBA Half", "RGB Compressed PVRTC 2 bits", "RGBA Compressed PVRTC 2 bits", "RGB Compressed PVRTC 4 bits", "RGBA Compressed PVRTC 4 bits", "RGB Compressed ETC 4 bits", "R Compressed ETC 4 bits", "RG Compressed EAC 8 bits", "RGB Compressed ETC2 4 bits", "RGB + 1-bit Alpha Compressed ETC2 4 bits", "RGBA Compressed ETC2 8 bits", "RGB Compressed ASTC 4*4 block", "RGB Compressed ASTC 5*5 block", "RGB Compressed ASTC 6*6 block", "RGB Compressed ASTC 8*8 block", "RGB Compressed ASTC 10*10 block", "RGB Compressed ASTC 12*12 block", "RGBA Compressed ASTC 4*4 block", "RGBA Compressed ASTC 5*5 block", "RGBA Compressed ASTC 6*6 block", "RGBA Compressed ASTC 8*8 block", "RGBA Compressed ASTC 10*10 block", "RGBA Compressed ASTC 12*12 block", "R 8", "RGB Crunched ETC", "RGBA Crunched ETC" };
    int AndroidFormatIndex = 0;
    string[] AndroidCompresssionOptions = { "Fast", "Nromal", "Best" };
    int AndroidCompresssionIndex = 0;
    string[] OverrideETC2fallbackOptions = { "Use build settings", "32-bit", "16-bit", "32-bit(half resolution)" };
    int OverrideETC2fallbackIndex = 0;
    int AndroidCompresssionQuality = 0;

    TextureImporterFormat[] FormatOptions = { TextureImporterFormat.Alpha8, TextureImporterFormat.RGB24, TextureImporterFormat.RGBA32, TextureImporterFormat.RGB16, TextureImporterFormat.R16, TextureImporterFormat.RGBA16, TextureImporterFormat.RGBAHalf, TextureImporterFormat.PVRTC_RGB2, TextureImporterFormat.PVRTC_RGBA2, TextureImporterFormat.PVRTC_RGB4, TextureImporterFormat.PVRTC_RGBA4, TextureImporterFormat.ETC2_RGB4, TextureImporterFormat.EAC_R, TextureImporterFormat.EAC_RG, TextureImporterFormat.ETC_RGB4Crunched, TextureImporterFormat.ETC2_RGB4, TextureImporterFormat.ETC2_RGBA8, TextureImporterFormat.ASTC_4x4, TextureImporterFormat.ASTC_5x5, TextureImporterFormat.ASTC_6x6, TextureImporterFormat.ASTC_8x8, TextureImporterFormat.ASTC_10x10, TextureImporterFormat.ASTC_12x12, TextureImporterFormat.ASTC_4x4, TextureImporterFormat.ASTC_5x5, TextureImporterFormat.ASTC_6x6, TextureImporterFormat.ASTC_8x8, TextureImporterFormat.ASTC_10x10, TextureImporterFormat.ASTC_12x12, TextureImporterFormat.R8, TextureImporterFormat.ETC_RGB4Crunched, TextureImporterFormat.ETC2_RGBA8Crunched };


    bool foldout;

    int count = 0;
    List<string> paths = new List<string>();
    List<Rect> rects = new List<Rect>();

    bool isfirst = true;

    public EditorWindow secondWindow;


    #endregion

    void OnGUI()
    {
        SetParameters parametersConfig = AssetDatabase.LoadAssetAtPath<SetParameters>("Assets/Scripts/Tools/png.asset") as SetParameters;


        if (null == parametersConfig)
        {
            Debug.LogError("����asset�Ƿ�Ϊ��");
            return;
        }

        instance = this;

        if (isfirst)
        {
            isfirst = false;
            LoadStartParameters(parametersConfig);
            if (parametersConfig.paths != null)
            {
                for (int i = 0; i < parametersConfig.paths.Count; i++)
                {
                    rects.Add(new Rect());
                }
            }
        }

        foldout = EditorGUILayout.Foldout(foldout, "配置1");
        if (foldout)
        {
            TextureTypeIndex = EditorGUILayout.Popup("Texture Type", TextureTypeIndex, TextureTypeOptions);

            TextureShapeIndex = EditorGUILayout.Popup("Texture Shape", TextureShapeIndex, TextureShapeOptions);

            isSRGB = GUILayout.Toggle(isSRGB, "sRGB(Color Texture)", GUILayout.Height(20), GUILayout.Width(100));

            AlphaSourceIndex = EditorGUILayout.Popup("Alpha Source", AlphaSourceIndex, AlphaSourceOptions);

            isAlphaIsTransparency = GUILayout.Toggle(isAlphaIsTransparency, "Alpha Is Transparency", GUILayout.Height(20), GUILayout.Width(100));

            GUILayout.Space(10);

            NonPowerof2Index = EditorGUILayout.Popup("Non Power of 2", NonPowerof2Index, NonPowerof2Options);

            isReadandWriteEnable = GUILayout.Toggle(isReadandWriteEnable, "Read and Write Enable", GUILayout.Height(20), GUILayout.Width(100));

            isStreamingMipMaps = GUILayout.Toggle(isStreamingMipMaps, "Streaming Mip Maps", GUILayout.Height(20), GUILayout.Width(100));

            if (isStreamingMipMaps)
            {
                MipMapPriority = EditorGUILayout.IntField("Mip Map Priority��", MipMapPriority);
            }
            else
            {
                MipMapPriority = 0;
            }

            isGenerateMipMaps = GUILayout.Toggle(isGenerateMipMaps, "Generate Mip Maps", GUILayout.Height(20), GUILayout.Width(100));

            if (isGenerateMipMaps)
            {
                isBorderMipMaps = GUILayout.Toggle(isBorderMipMaps, "Border Mip Maps", GUILayout.Height(20), GUILayout.Width(100));

                MipMapFiteringIndex = EditorGUILayout.Popup("Mip Map Fitering", MipMapFiteringIndex, MipMapFiteringOptions);

                isMipMapsPreserveCoverage = GUILayout.Toggle(isMipMapsPreserveCoverage, "Mip Maps Preserve Coverage", GUILayout.Height(20), GUILayout.Width(100));

                isFadeoutMipMaps = GUILayout.Toggle(isFadeoutMipMaps, "Fadeout Mip Maps", GUILayout.Height(20), GUILayout.Width(100));

                if (isFadeoutMipMaps)
                {
                    GUILayout.BeginHorizontal();
                    FadeRangeleft = EditorGUILayout.IntField("FadeRangeleft��", FadeRangeleft);
                    FadeRangeright = EditorGUILayout.IntField("FadeRangeleft��", FadeRangeright);
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.Space(10);

            WarpModeIndex = EditorGUILayout.Popup("Warp Mode", WarpModeIndex, WarpModeOptions);

            FilterModeIndex = EditorGUILayout.Popup("Filter Mode", FilterModeIndex, FilterModeOptions);

            AnisoLevel = (int)EditorGUILayout.Slider("Aniso Level", AnisoLevel, 0, 16);

            GUILayout.Space(10);

            GUILayout.Label("Default");
            DefaultMaxSizeIndex = EditorGUILayout.Popup("Max Size", DefaultMaxSizeIndex, DefaultMaxSizeOptions);
            DefaultResizeAlgorithmIndex = EditorGUILayout.Popup("Resize Algorithm", DefaultResizeAlgorithmIndex, DefaultResizeAlgorithmOptions);
            DefaultFormatIndex = EditorGUILayout.Popup("Format", DefaultFormatIndex, DefaultFormatOptions);
            DefaultCompresssionIndex = EditorGUILayout.Popup("Compresssion", DefaultCompresssionIndex, DefaultCompresssionOptions);
            IsUseCurnchCompre = GUILayout.Toggle(IsUseCurnchCompre, "Use Crunch Compression", GUILayout.Height(20), GUILayout.Width(100));
            DefaultCompresssionQuality = (int)EditorGUILayout.Slider("Compressor Quality", DefaultCompresssionQuality, 0, 100);

            GUILayout.Space(10);

            EditorGUILayout.LabelField("IOS");
            isOverrideIOS = GUILayout.Toggle(isOverrideIOS, "Override for IOS", GUILayout.Height(20), GUILayout.Width(100));

            if (isOverrideIOS)
            {
                IOSMaxSizeIndex = EditorGUILayout.Popup("Max Size", IOSMaxSizeIndex, IOSMaxSizeOptions);
                IOSResizeAlgorithmIndex = EditorGUILayout.Popup("Resize Algorithm", IOSResizeAlgorithmIndex, IOSResizeAlgorithmOptions);
                IOSFormatIndex = EditorGUILayout.Popup("Format", IOSFormatIndex, IOSFormatOptions);
                //IOSCompresssionIndex = EditorGUILayout.Popup("Compresssion", IOSCompresssionIndex, IOSCompresssionOptions);
                IOSCompresssionQuality = (int)EditorGUILayout.Slider("Compressor Quality", IOSCompresssionQuality, 0, 100);
            }
            GUILayout.Space(10);

            EditorGUILayout.LabelField("Android");
            isOverrideAndroid = GUILayout.Toggle(isOverrideAndroid, "Override for Android", GUILayout.Height(20), GUILayout.Width(100));

            if (isOverrideAndroid)
            {
                AndroidMaxSizeIndex = EditorGUILayout.Popup("Max Size", AndroidMaxSizeIndex, AndroidMaxSizeOptions);
                AndroidResizeAlgorithmIndex = EditorGUILayout.Popup("Resize Algorithm", AndroidResizeAlgorithmIndex, AndroidResizeAlgorithmOptions);
                AndroidFormatIndex = EditorGUILayout.Popup("Format", AndroidFormatIndex, AndroidFormatOptions);
                //AndroidCompresssionIndex = EditorGUILayout.Popup("Compresssion", AndroidCompresssionIndex, AndroidCompresssionOptions);
                OverrideETC2fallbackIndex = EditorGUILayout.Popup("Override ETC2 fallback", OverrideETC2fallbackIndex, OverrideETC2fallbackOptions);
                AndroidCompresssionQuality = (int)EditorGUILayout.Slider("Compressor Quality", AndroidCompresssionQuality, 0, 100);
            }

        }

        GUILayout.Space(20);

        for (int i = 0; i < count; i++)
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label("路径" + (i + 1));
            rects[i] = EditorGUILayout.GetControlRect(GUILayout.Width(800));

            paths[i] = EditorGUI.TextField(rects[i], paths[i]);

            //if (Event.current.type == EventType.DragExited && rects[i].Contains(Event.current.mousePosition))
            //{
            //    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            //    if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
            //    {
            //        paths[i] = DragAndDrop.paths[0];
            //    }
            //}
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("新增文件夹路径"))
        {
            count++;
            paths.Add("");
            rects.Add(EditorGUILayout.GetControlRect(GUILayout.Width(800)));
        }

        if (GUILayout.Button("清空文件夹路径"))
        {
            ClearData();
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("保存配置文件数据"))
        {
            SetAllparameters(parametersConfig);
            ShowNotification(new GUIContent("当前配置已保存"));
        }

        if (GUILayout.Button("根据配置刷新文件夹"))
        {
            secondWindow = EditorWindow.GetWindow(typeof(Secondpop));
            secondWindow.Show();
        }
    }
    //根据设置 更新序列化信息
    void SetAllparameters(SetParameters parametersConfig)
    {
        TextureImporterType type = TextureImporterType.Default;
        switch (TextureTypeIndex)
        {
            case 0:
                type = TextureImporterType.Default;
                break;
            case 1:
                type = TextureImporterType.NormalMap;
                break;
            case 2:
                type = TextureImporterType.GUI;
                break;
            case 3:
                type = TextureImporterType.Sprite;
                break;
            case 4:
                type = TextureImporterType.Cursor;
                break;
            case 5:
                type = TextureImporterType.Cookie;
                break;
            case 6:
                type = TextureImporterType.Lightmap;
                break;

            case 7:
                type = TextureImporterType.SingleChannel;
                break;
        }
        parametersConfig.TextureType = type;

        TextureImporterShape shape = TextureImporterShape.Texture2D;
        switch (TextureShapeIndex)
        {
            case 0:
                shape = TextureImporterShape.Texture2D;
                break;
            case 1:
                shape = TextureImporterShape.TextureCube;
                break;

        }
        parametersConfig.TextureShape = shape;

        parametersConfig.sRGB = isSRGB;

        TextureImporterAlphaSource alphaSource = TextureImporterAlphaSource.None;
        switch (AlphaSourceIndex)
        {
            case 0:
                alphaSource = TextureImporterAlphaSource.None;
                break;
            case 1:
                alphaSource = TextureImporterAlphaSource.FromInput;
                break;
            case 2:
                alphaSource = TextureImporterAlphaSource.FromGrayScale;
                break;

        }
        parametersConfig.AlphaSource = alphaSource;

        parametersConfig.AlphaIsTransparency = isAlphaIsTransparency;

        switch (NonPowerof2Index)
        {
            case 0:
                parametersConfig.NonPowerOf2 = TextureImporterNPOTScale.None;
                break;
            case 1:
                parametersConfig.NonPowerOf2 = TextureImporterNPOTScale.ToNearest;
                break;
            case 2:
                parametersConfig.NonPowerOf2 = TextureImporterNPOTScale.ToLarger;
                break;
            case 3:
                parametersConfig.NonPowerOf2 = TextureImporterNPOTScale.ToSmaller;
                break;
        }

        parametersConfig.IsRead = isReadandWriteEnable;

        parametersConfig.StreamingMipmaps = isStreamingMipMaps;

        parametersConfig.MipMapPriority = MipMapPriority;

        parametersConfig.GenerateMipMaps = isGenerateMipMaps;

        parametersConfig.BorderMipmap = isBorderMipMaps;

        TextureImporterMipFilter MipFilter = TextureImporterMipFilter.BoxFilter;
        switch (MipMapFiteringIndex)
        {
            case 0:
                MipFilter = TextureImporterMipFilter.BoxFilter;
                break;
            case 1:
                MipFilter = TextureImporterMipFilter.KaiserFilter;
                break;
        }
        parametersConfig.MipMapFiltering = MipFilter;

        parametersConfig.MipMapsPreserveCoverage = isMipMapsPreserveCoverage;

        parametersConfig.FadeoutMipMaps = isFadeoutMipMaps;

        parametersConfig.FadeRangeleft = FadeRangeleft;
        parametersConfig.FadeRangeright = FadeRangeright;

        TextureWrapMode mode = TextureWrapMode.Repeat;
        switch (WarpModeIndex)
        {
            case 0:
                mode = TextureWrapMode.Repeat;
                break;
            case 1:
                mode = TextureWrapMode.Clamp;
                break;
            case 2:
                mode = TextureWrapMode.Mirror;
                break;
            case 3:
                mode = TextureWrapMode.MirrorOnce;
                break;
        }
        parametersConfig.WrapMode = mode;

        FilterMode mode2 = FilterMode.Bilinear;
        switch (FilterModeIndex)
        {
            case 0:
                mode2 = FilterMode.Bilinear;
                break;
            case 1:
                mode2 = FilterMode.Point;
                break;
            case 2:
                mode2 = FilterMode.Trilinear;
                break;
        }
        parametersConfig.FilterMode = mode2;

        parametersConfig.AnisoLevel = AnisoLevel;

        parametersConfig.DefaultMaxSize = (int)(Math.Pow(2, DefaultMaxSizeIndex) * 32);
        parametersConfig.DefaultResizeAlgorithm = DefaultResizeAlgorithmIndex == 0 ? TextureResizeAlgorithm.Mitchell : TextureResizeAlgorithm.Bilinear;
        switch (DefaultFormatIndex)
        {
            case 0:
                parametersConfig.DefaultFormat = TextureImporterFormat.Automatic;
                break;
            case 1:
                parametersConfig.DefaultFormat = TextureImporterFormat.Alpha8;
                break;
            case 2:
                parametersConfig.DefaultFormat = TextureImporterFormat.RGB24;
                break;
            case 3:
                parametersConfig.DefaultFormat = TextureImporterFormat.RGBA32;
                break;
            case 4:
                parametersConfig.DefaultFormat = TextureImporterFormat.RGB16;
                break;
            case 5:
                parametersConfig.DefaultFormat = TextureImporterFormat.R16;
                break;
            case 6:
                parametersConfig.DefaultFormat = TextureImporterFormat.R8;
                break;
        }
        switch (DefaultCompresssionIndex)
        {
            case 0:
                parametersConfig.DefaultCompression = TextureImporterCompression.Uncompressed;
                break;
            case 1:
                parametersConfig.DefaultCompression = TextureImporterCompression.CompressedLQ;
                break;
            case 2:
                parametersConfig.DefaultCompression = TextureImporterCompression.Compressed;
                break;
            case 3:
                parametersConfig.DefaultCompression = TextureImporterCompression.CompressedHQ;
                break;

        }

        parametersConfig.IsUseCurnchCompre = IsUseCurnchCompre;
        parametersConfig.DefaultCompressionQuality = DefaultCompresssionQuality;

        parametersConfig.isOverrideIOS = isOverrideIOS;
        parametersConfig.IOSMaxSize = (int)(Math.Pow(2, IOSMaxSizeIndex) * 32);
        parametersConfig.IOSResizeAlgorithm = IOSResizeAlgorithmIndex == 0 ? TextureResizeAlgorithm.Mitchell : TextureResizeAlgorithm.Bilinear;
        parametersConfig.IOSFormat = FormatOptions[IOSFormatIndex];

        parametersConfig.IOSCompressionQuality = IOSCompresssionQuality;

        parametersConfig.isOverrideAndroid = isOverrideAndroid;
        parametersConfig.AndroidMaxSize = (int)(Math.Pow(2, AndroidMaxSizeIndex) * 32);
        parametersConfig.AndroidResizeAlgorithm = AndroidResizeAlgorithmIndex == 0 ? TextureResizeAlgorithm.Mitchell : TextureResizeAlgorithm.Bilinear;

        parametersConfig.AndroidFormat = FormatOptions[AndroidFormatIndex];
        switch (OverrideETC2fallbackIndex)
        {
            case 0:
                parametersConfig.androidETC2FallbackOverride = AndroidETC2FallbackOverride.UseBuildSettings;
                break;
            case 1:
                parametersConfig.androidETC2FallbackOverride = AndroidETC2FallbackOverride.Quality32Bit;
                break;
            case 2:
                parametersConfig.androidETC2FallbackOverride = AndroidETC2FallbackOverride.Quality16Bit;
                break;
            case 3:
                parametersConfig.androidETC2FallbackOverride = AndroidETC2FallbackOverride.Quality32BitDownscaled;
                break;

        }
        parametersConfig.AndroidCompressionQuality = AndroidCompresssionQuality;


        parametersConfig.paths = paths;
    }

    //�������л���ʼ��Ϣ
    void LoadStartParameters(SetParameters parametersConfig)
    {
        switch (parametersConfig.TextureType)
        {
            case TextureImporterType.Default:
                TextureTypeIndex = 0;
                break;
            case TextureImporterType.NormalMap:
                TextureTypeIndex = 1;
                break;
            case TextureImporterType.GUI:
                TextureTypeIndex = 2;
                break;
            case TextureImporterType.Sprite:
                TextureTypeIndex = 3;
                break;
            case TextureImporterType.Cursor:
                TextureTypeIndex = 4;
                break;
            case TextureImporterType.Cookie:
                TextureTypeIndex = 5;
                break;
            case TextureImporterType.Lightmap:
                TextureTypeIndex = 6;
                break;

            case TextureImporterType.SingleChannel:
                TextureTypeIndex = 7;
                break;
        }

        TextureImporterShape shape = TextureImporterShape.Texture2D;

        switch (parametersConfig.TextureShape)
        {
            case TextureImporterShape.Texture2D:
                TextureShapeIndex = 0;
                break;
            case TextureImporterShape.TextureCube:
                TextureShapeIndex = 1;
                break;

        }

        isSRGB = parametersConfig.sRGB;

        switch (parametersConfig.AlphaSource)
        {
            case TextureImporterAlphaSource.None:
                AlphaSourceIndex = 0;
                break;
            case TextureImporterAlphaSource.FromInput:
                AlphaSourceIndex = 1;
                break;
            case TextureImporterAlphaSource.FromGrayScale:
                AlphaSourceIndex = 2;
                break;

        }

        isAlphaIsTransparency = parametersConfig.AlphaIsTransparency;

        switch (parametersConfig.NonPowerOf2)
        {
            case TextureImporterNPOTScale.None:
                NonPowerof2Index = 0;
                break;
            case TextureImporterNPOTScale.ToNearest:
                NonPowerof2Index = 1;
                break;
            case TextureImporterNPOTScale.ToLarger:
                NonPowerof2Index = 2;
                break;
            case TextureImporterNPOTScale.ToSmaller:
                NonPowerof2Index = 3;
                break;
        }

        isReadandWriteEnable = parametersConfig.IsRead;

        isStreamingMipMaps = parametersConfig.StreamingMipmaps;

        MipMapPriority = parametersConfig.MipMapPriority;

        isGenerateMipMaps = parametersConfig.GenerateMipMaps;

        isBorderMipMaps = parametersConfig.BorderMipmap;

        TextureImporterMipFilter MipFilter = TextureImporterMipFilter.BoxFilter;
        switch (parametersConfig.MipMapFiltering)
        {
            case TextureImporterMipFilter.BoxFilter:
                MipMapFiteringIndex = 0;
                break;
            case TextureImporterMipFilter.KaiserFilter:
                MipMapFiteringIndex = 1;
                break;
        }

        isMipMapsPreserveCoverage = parametersConfig.MipMapsPreserveCoverage;

        isFadeoutMipMaps = parametersConfig.FadeoutMipMaps;

        FadeRangeleft = parametersConfig.FadeRangeleft;
        FadeRangeright = parametersConfig.FadeRangeright;

        switch (parametersConfig.WrapMode)
        {
            case TextureWrapMode.Repeat:
                WarpModeIndex = 0;
                break;
            case TextureWrapMode.Clamp:
                WarpModeIndex = 1;
                break;
            case TextureWrapMode.Mirror:
                WarpModeIndex = 2;
                break;
            case TextureWrapMode.MirrorOnce:
                WarpModeIndex = 3;
                break;
        }

        FilterMode mode2 = FilterMode.Bilinear;
        switch (parametersConfig.FilterMode)
        {
            case FilterMode.Bilinear:
                FilterModeIndex = 0;
                break;
            case FilterMode.Point:
                FilterModeIndex = 1;
                break;
            case FilterMode.Trilinear:
                FilterModeIndex = 2;
                break;
        }
        AnisoLevel = parametersConfig.AnisoLevel;

        DefaultMaxSizeIndex = (int)Math.Log(parametersConfig.DefaultMaxSize / 32, 2);
        DefaultResizeAlgorithmIndex = parametersConfig.DefaultResizeAlgorithm == TextureResizeAlgorithm.Mitchell ? 0 : 1;
        switch (parametersConfig.DefaultFormat)
        {
            case TextureImporterFormat.Automatic:
                DefaultFormatIndex = 0;
                break;
            case TextureImporterFormat.Alpha8:
                DefaultFormatIndex = 1;
                break;
            case TextureImporterFormat.RGB24:
                DefaultFormatIndex = 2;
                break;
            case TextureImporterFormat.RGBA32:
                DefaultFormatIndex = 3;
                break;
            case TextureImporterFormat.RGB16:
                DefaultFormatIndex = 4;
                break;
            case TextureImporterFormat.R16:
                DefaultFormatIndex = 5;
                break;
            case TextureImporterFormat.R8:
                DefaultFormatIndex = 6;
                break;
        }
        IsUseCurnchCompre = parametersConfig.IsUseCurnchCompre;
        DefaultCompresssionQuality = parametersConfig.DefaultCompressionQuality;

        isOverrideIOS = parametersConfig.isOverrideIOS;
        IOSMaxSizeIndex = (int)Math.Log(parametersConfig.IOSMaxSize / 32, 2);
        IOSResizeAlgorithmIndex = parametersConfig.IOSResizeAlgorithm == TextureResizeAlgorithm.Mitchell ? 0 : 1;
        for (int i = 0; i < FormatOptions.Length; i++)
        {
            if (parametersConfig.IOSFormat == FormatOptions[i])
            {
                IOSFormatIndex = i;
                break;
            }
        }
        //IOSCompresssionIndex = parametersConfig.IOSCompressionQuality;

        isOverrideAndroid = parametersConfig.isOverrideAndroid;
        AndroidMaxSizeIndex = (int)Math.Log(parametersConfig.AndroidMaxSize / 32, 2);
        AndroidResizeAlgorithmIndex = parametersConfig.AndroidResizeAlgorithm == TextureResizeAlgorithm.Mitchell ? 0 : 1;

        for (int i = 0; i < FormatOptions.Length; i++)
        {
            if (parametersConfig.AndroidFormat == FormatOptions[i])
            {
                AndroidFormatIndex = i;
                break;
            }
        }
        //AndroidCompresssionIndex = parametersConfig.AndroidCompressionQuality;

        switch (parametersConfig.androidETC2FallbackOverride)
        {
            case AndroidETC2FallbackOverride.UseBuildSettings:
                AndroidFormatIndex = 0;
                break;
            case AndroidETC2FallbackOverride.Quality32Bit:
                AndroidFormatIndex = 1;
                break;
            case AndroidETC2FallbackOverride.Quality16Bit:
                AndroidFormatIndex = 2;
                break;
            case AndroidETC2FallbackOverride.Quality32BitDownscaled:
                AndroidFormatIndex = 3;
                break;
        }

        if (parametersConfig.paths != null)
        {
            paths = parametersConfig.paths;
            count = parametersConfig.paths.Count;
        }

    }

    //����ļ�������
    void ClearData()
    {
        count = 0;
        paths.Clear();
    }

    //ˢ���ļ�������
    public void RefreshData()
    {
        if (paths == null || paths.Count == 0)
        {
            Debug.LogError("批量处理路径为空，请检查！");
            return;
        }
        for (int i = 0; i < paths.Count; i++)
        {
            var path = paths[i];
            if (path == "") break;
            DirectoryInfo direction = new DirectoryInfo(path);
            //获取PNG
            FileInfo[] pngFiles = direction.GetFiles("*.jpg", SearchOption.AllDirectories);
            try
            {
                SetTexture(pngFiles);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
    }
    //操作文件夹
    static void SetTexture(FileInfo[] fileInfo)
    {
        for (int i = 0; i < fileInfo.Length; i++)
        {
            string fullpath = fileInfo[i].FullName.Replace("\\", "/");
            string path = fullpath.Replace(Application.dataPath, "Assets");
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            EditorUtility.DisplayProgressBar("批量处理图片", fileInfo[i].Name, i / (float)fileInfo.Length);
            SetTextureFormat(textureImporter);

            //Debug.LogError("完事");
        }
    }

    //������ϸ����
    static void SetTextureFormat(TextureImporter textureImporter)
    {
        SetParameters parametersConfig = AssetDatabase.LoadAssetAtPath<SetParameters>("Assets/Scripts/Tools/png.asset") as SetParameters;

        textureImporter.textureType = parametersConfig.TextureType;
        textureImporter.textureShape = parametersConfig.TextureShape;
        textureImporter.sRGBTexture = parametersConfig.sRGB;
        textureImporter.alphaSource = parametersConfig.AlphaSource;
        textureImporter.alphaIsTransparency = parametersConfig.AlphaIsTransparency;

        textureImporter.npotScale = parametersConfig.NonPowerOf2;

        textureImporter.isReadable = parametersConfig.IsRead;
        textureImporter.streamingMipmaps = parametersConfig.StreamingMipmaps;
        textureImporter.streamingMipmapsPriority = parametersConfig.MipMapPriority;

        textureImporter.generateMipsInLinearSpace = parametersConfig.GenerateMipMaps;
        textureImporter.borderMipmap = parametersConfig.BorderMipmap;
        textureImporter.mipmapFilter = parametersConfig.MipMapFiltering;
        textureImporter.mipMapsPreserveCoverage = parametersConfig.MipMapsPreserveCoverage;
        textureImporter.fadeout = parametersConfig.FadeoutMipMaps;
        textureImporter.mipmapFadeDistanceStart = parametersConfig.FadeRangeleft;
        textureImporter.mipmapFadeDistanceEnd = parametersConfig.FadeRangeright;

        textureImporter.wrapMode = parametersConfig.WrapMode;
        textureImporter.filterMode = parametersConfig.FilterMode;

        textureImporter.anisoLevel = parametersConfig.AnisoLevel;

        SetPlatformSettings(textureImporter);
    }

    //���ø���ƽ̨����
    public static void SetPlatformSettings(TextureImporter importer)
    {
        SetParameters parametersConfig = AssetDatabase.LoadAssetAtPath<SetParameters>("Assets/Scripts/Tools/png.asset") as SetParameters;

        TextureImporterPlatformSettings setParam = new TextureImporterPlatformSettings();


        setParam.maxTextureSize = parametersConfig.DefaultMaxSize;
        setParam.resizeAlgorithm = parametersConfig.DefaultResizeAlgorithm;
        setParam.format = parametersConfig.DefaultFormat;
        setParam.textureCompression = parametersConfig.DefaultCompression;
        setParam.crunchedCompression = parametersConfig.IsUseCurnchCompre;
        setParam.compressionQuality = parametersConfig.DefaultCompressionQuality;
        importer.SetPlatformTextureSettings(setParam);

        if (parametersConfig.isOverrideIOS)
        {
            //ios�汾
            setParam = importer.GetPlatformTextureSettings("iOS");
            setParam.maxTextureSize = parametersConfig.IOSMaxSize;
            setParam.resizeAlgorithm = parametersConfig.IOSResizeAlgorithm;
            setParam.format = parametersConfig.IOSFormat;
            setParam.compressionQuality = parametersConfig.IOSCompressionQuality;
            setParam.overridden = true;
            importer.SetPlatformTextureSettings(setParam);

        }
        else
        {
            setParam = importer.GetPlatformTextureSettings("iOS");
            setParam.overridden = false;
            importer.SetPlatformTextureSettings(setParam);
        }

        if (parametersConfig.isOverrideAndroid)
        {
            //��׿�汾
            setParam = importer.GetPlatformTextureSettings("Android");  //������Get�����õ�������Override for Android���ᱻ��Ϊtrue
            setParam.maxTextureSize = parametersConfig.AndroidMaxSize;
            setParam.resizeAlgorithm = parametersConfig.AndroidResizeAlgorithm;
            setParam.format = parametersConfig.AndroidFormat;
            setParam.compressionQuality = parametersConfig.AndroidCompressionQuality;
            setParam.androidETC2FallbackOverride = parametersConfig.androidETC2FallbackOverride;
            setParam.overridden = true;
            importer.SetPlatformTextureSettings(setParam);
        }
        else
        {
            setParam = importer.GetPlatformTextureSettings("Android");
            setParam.overridden = false;
            importer.SetPlatformTextureSettings(setParam);
        }

        importer.SaveAndReimport();
    }

    //���ò����ļ��У���ѡ ��ѡ���ɣ�
    public static void SetSections()
    {
        string parentPath = "Assets";
        List<string> paths = new List<string>();
        foreach (var obj in Selection.GetFiltered<Object>(SelectionMode.Assets))
        {
            paths.Add(obj.name + ".png");

            var path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path))
                continue;

            if (System.IO.Directory.Exists(path))
                parentPath = path;
            else if (System.IO.File.Exists(path))
                parentPath = System.IO.Path.GetDirectoryName(path);
        }


        DirectoryInfo direction = new DirectoryInfo(parentPath);
        ////��ֻ����PNG
        FileInfo[] pngFiles = direction.GetFiles("*.png", SearchOption.AllDirectories);

        List<FileInfo> tempFiles = new List<FileInfo>();

        for (int i = 0; i < pngFiles.Length; i++)
        {
            //Debug.LogError("pngFiles[i].Name:"+pngFiles[i].Name);
            if (paths.Contains(pngFiles[i].Name))
            {
                tempFiles.Add(pngFiles[i]);
            }
        }

        FileInfo[] laterFiles = tempFiles.ToArray();

        try
        {
            SetTexture(laterFiles);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        return;
    }
}

public class ArtMaterialSet : EditorWindow
{
    [MenuItem("Zzs工具/资源/批量修改Png文件格式")]
    static void showWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(ModifyFilesInBulk));
        window.Show();
    }
}

#endif