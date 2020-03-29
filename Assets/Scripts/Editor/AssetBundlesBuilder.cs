using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundlesBuilder : Editor
{
    [MenuItem("Assets/Asset bundles builder")]
    public static void Show()
    {
        var window = EditorWindow.GetWindow<AssetBundleBuilder>();
        window.Show();
    }
}

public class AssetBundleBuilder : EditorWindow
{
    public enum AssetType
    {
        IOS,
        Android,
        Windows
    }

    public string version = "";
    public AssetType option = AssetType.Windows;

    private const string AssetsVersionPath = "Assets/StreamingAssets/AssetBundles/Version";

    private void Awake()
    {
        version = TryLoadFromFile(AssetsVersionPath);
    }
    
    private string TryLoadFromFile(string fileName)
    {
        try
        {
            if (!File.Exists(fileName)) return string.Empty;
            var result = File.ReadAllText(fileName);
            return result;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return string.Empty;
    }

    private void OnGUI()
    {
        GUILayout.Label ("Enter version", EditorStyles.boldLabel);
        version = GUILayout.TextField(version);
        option = (AssetType) EditorGUILayout.EnumPopup("Choose type", option);
        if (!GUILayout.Button("Build")) return;
        
        switch (option)
        {
            case AssetType.Windows:
                Build(BuildTarget.StandaloneWindows64, version);
                break;
            case AssetType.IOS:
                throw new NotImplementedException("AssetBundleBuilder : option not implemented");
                break;
            case AssetType.Android:
                throw new NotImplementedException("AssetBundleBuilder : option not implemented");
                break;
        }
    }

    private void Build(BuildTarget target, string text)
    {
//        BuildPipeline.BuildAssetBundles(
//            "Assets/StreamingAssets/AssetBundles",
//            BuildAssetBundleOptions.None,
//            target);
        SaveVersion(text);
    }

    private void SaveVersion(string text)
    {
        File.WriteAllText(AssetsVersionPath, text);
    }
}
