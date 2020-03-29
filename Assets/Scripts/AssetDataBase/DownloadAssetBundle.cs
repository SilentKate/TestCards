using UnityEngine.Networking;

public class DownloadAssetBundle : ChainElement
{
    private readonly AssetDataSource _source;

    public DownloadAssetBundle(AssetDataSource source)
    {
        _source = source;
    }
    
    public override void Handle(object context = null)
    {
        var needDownloadContext = (HandleAssetBundleVersion.Result) context;

        UnityWebRequestProcess process = null;
        if (needDownloadContext != null)
        {
            if (!needDownloadContext.NeedDownload)
            {
                HandleNext();
                return;
            }
            
            process = UnityWebRequestAssetBundle.GetAssetBundle(_source.ExternalAssetUrl, needDownloadContext.Hash).SendWebRequest().GetProcess();
        }
        else
        {
            process = UnityWebRequestAssetBundle.GetAssetBundle(_source.ExternalAssetUrl).SendWebRequest().GetProcess();
        }

        process.AddHandler(
            webRequest =>
            {
                if (!webRequest.isHttpError && !webRequest.isNetworkError)
                {
                    HandleNext();    
                }
                else
                {
                    HandleInterrupted();
                }
            });
    }
}