using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class ExternalAssetValidator
{
    private readonly List<AssetDataSource> _pathSources;

    public ExternalAssetValidator([NotNull] List<AssetDataSource> pathSources)
    {
        _pathSources = pathSources ?? throw new ArgumentNullException(nameof(pathSources));
    }

    public SimpleChain DownloadResourcesIfNeed()
    {
        var chain = new SimpleChain();
        foreach (var source in _pathSources)
        {
            chain.Add(new HandleAssetBundleVersion(source));
            chain.Add(new DownloadAssetBundle(source));
        }
        return chain;
    }

    public void ClearCachedVersions()
    {
        foreach (var source in _pathSources)
        {
            ExternalAssetsValidationHelper.ClearAllVersions(source);
        }    
    }
}