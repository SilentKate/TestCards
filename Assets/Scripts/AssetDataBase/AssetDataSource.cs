using System.IO;
using UnityEngine;

public class AssetDataSource
{
     public string ExternalAssetUrl { get; }
     public string ExternalAssetName { get; }
     
     public AssetDataSource(AppResourcesConfig.Source source)
     {
         ExternalAssetName = source.externalAssetName;
         ExternalAssetUrl = GetStreamingAssetsPath(source.externalAssetPath, ExternalAssetName);
     }
    
     private string GetStreamingAssetsPath(string path, string name)
     {
         path = Path.Combine(Application.streamingAssetsPath, path);
         path = Path.Combine(path, name);
         return path;
     }
}