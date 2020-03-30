using UnityEngine;

public static class AssetsStorageExt
{
    public static T GetCardAsset<T>(this AssetsStorage storage, string name) where T : Object
    {
        return storage.GetBundle("cards").LoadAsset<T>(name);
    }

    public static void UnloadCardAssets(this AssetsStorage storage)
    {
        storage.UnloadObjects("cards");
    }
}