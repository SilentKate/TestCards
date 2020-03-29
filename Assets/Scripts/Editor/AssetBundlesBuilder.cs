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

    public AssetType option = AssetType.Windows;

    private void OnGUI()
    {
        option = (AssetType) EditorGUILayout.EnumPopup("Choose type", option);
        if (!GUILayout.Button("Build")) return;
        
        switch (option)
        {
            case AssetType.Windows:
                Build(BuildTarget.StandaloneWindows64);
                break;
            case AssetType.IOS:
                throw new NotImplementedException("AssetBundleBuilder : option not implemented");
                break;
            case AssetType.Android:
                throw new NotImplementedException("AssetBundleBuilder : option not implemented");
                break;
        }
    }

    private void Build(BuildTarget target)
    {
        BuildPipeline.BuildAssetBundles(
            "Assets/StreamingAssets/AssetBundles",
            BuildAssetBundleOptions.None,
            target);
    }
}
