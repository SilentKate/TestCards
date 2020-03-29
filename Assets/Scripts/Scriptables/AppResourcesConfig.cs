using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/External resources config")]
public class AppResourcesConfig : ScriptableObject
{
    [Serializable]
    public struct Source
    {
//        public string cachedVersionDirectory;
//        public string cachedVersionName;
//        public string externalVersionPath;
        public string externalAssetPath;
        public string externalAssetName;
    }

    public Source[] sources;

    public List<AssetDataSource> GetSources()
    {
        return sources.Select(source => new AssetDataSource(source)).ToList();
    }
}