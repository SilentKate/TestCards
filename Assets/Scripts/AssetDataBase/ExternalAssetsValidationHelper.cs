using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public static class ExternalAssetsValidationHelper
{
    public static void IsVersionLoaded(
        AssetDataSource source,
        Action<bool, Hash128> handler)
    {
        var request = new UnityWebRequest($"{source.ExternalAssetUrl}.manifest") {downloadHandler = new DownloadHandlerBuffer()};
        var process = request.SendWebRequest().GetProcess();
        process.AddHandler(
            webRequest =>
            {
                if (!webRequest.isHttpError && !webRequest.isNetworkError)
                {
                    var manifest = webRequest.downloadHandler?.text;
                    var hash = string.IsNullOrEmpty(manifest) 
                        ? default 
                        : GetHashFromManifest(manifest);

                    if (hash.isValid)
                    {
                        var versionCached = Caching.IsVersionCached(source.ExternalAssetUrl, hash);
                        handler.Invoke(!versionCached, hash);
                        return;
                    }
                }
                handler.Invoke(false, default);
                process.Dispose();     
            });
    }

    public static void ClearAllVersions(AssetDataSource source)
    {
        Caching.ClearAllCachedVersions(source.ExternalAssetName);
    }
    
    private static Hash128 GetHashFromManifest(string manifest)
    {
        var hashRow = manifest.Split("\n".ToCharArray())[5];
        var hash = Hash128.Parse(hashRow.Split(':')[1].Trim());

        return hash;
    }
}