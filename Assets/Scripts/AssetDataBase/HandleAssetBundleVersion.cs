using UnityEngine;

public class HandleAssetBundleVersion : ChainElement
{
    public class Result
    {
        public bool NeedDownload { get; set; }
        public Hash128 Hash { get; set; }
    }
    
    private readonly AssetDataSource _source;

    public HandleAssetBundleVersion(AssetDataSource source)
    {
        _source = source;
    }

    public override void Handle(object context = null)
    {
        ExternalAssetsValidationHelper.IsVersionLoaded(_source,
            (needDownload, hash) =>
            {
                if (needDownload)
                {
                    ExternalAssetsValidationHelper.ClearAllVersions(_source);
                }

                if (hash.isValid)
                {
                    HandleNext(new Result { NeedDownload = needDownload, Hash = hash });
                }
                else
                {
                    HandleInterrupted();
                }
            });
    }
}