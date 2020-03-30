using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class ExternalAssetValidator
{
    private readonly List<AssetDataSource> _pathSources;
    private readonly AssetsStorage _storage;

    public ExternalAssetValidator(
        [NotNull] List<AssetDataSource> pathSources,
        [NotNull] AssetsStorage storage)
    {
        _pathSources = pathSources ?? throw new ArgumentNullException(nameof(pathSources));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public SimpleChain DownloadResourcesIfNeed()
    {
        var chain = new SimpleChain();
        foreach (var source in _pathSources)
        {
            chain.Add(new HandleAssetBundleVersion(source));
            chain.Add(new DownloadAssetBundle(source, _storage));
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