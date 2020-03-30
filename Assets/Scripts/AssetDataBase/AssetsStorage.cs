using System;
using System.Collections.Generic;
using UnityEngine;

public class AssetsStorage
{
    private readonly Dictionary<string, AssetBundle> _bundles = new Dictionary<string, AssetBundle>();
    
    public void Add(AssetBundle bundle)
    {
        _bundles[bundle.name] = bundle;
    }

    public AssetBundle GetBundle(string bundleName)
    {
        if (_bundles.TryGetValue(bundleName, out var bundle))
        {
            return bundle;
        }
        throw new InvalidOperationException($"AssetsStorage :: Can't find bundle {bundleName}");
    }
    
    public void UnloadObjects(string bundleName)
    {
        if (_bundles.TryGetValue(bundleName, out var bundle))
        {
            bundle.Unload(true);
        }
    }
}