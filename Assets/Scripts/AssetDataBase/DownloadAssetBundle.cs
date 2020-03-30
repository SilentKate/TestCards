using UnityEngine.Networking;

public class DownloadAssetBundle : ChainElement
{
    private readonly AssetDataSource _source;
    private readonly AssetsStorage _storage;

    public DownloadAssetBundle(AssetDataSource source, AssetsStorage storage)
    {
        _source = source;
        _storage = storage;
    }
    
    public override void Handle(object context = null)
    {
        var needDownloadContext = (HandleAssetBundleVersion.Result) context;

        UnityWebRequestProcess process = null;
        if (needDownloadContext != null)
        {
            // если needDownloadContext.NeedDownload, то надо проверить место на устройстве и выгрузить старые версии 
            process = needDownloadContext.NeedDownload 
                ? UnityWebRequestAssetBundle.GetAssetBundle(_source.ExternalAssetUrl).SendWebRequest().GetProcess() 
                : UnityWebRequestAssetBundle.GetAssetBundle(_source.ExternalAssetUrl, needDownloadContext.Hash).SendWebRequest().GetProcess();
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
                    var bundle = DownloadHandlerAssetBundle.GetContent(webRequest);
                    _storage.Add(bundle);
                    HandleNext();    
                }
                else
                {
                    HandleInterrupted();
                }
            });
    }
}