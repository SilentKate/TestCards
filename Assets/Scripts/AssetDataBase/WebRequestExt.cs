using UnityEngine.Networking;

public static class WebRequestExt
{
    public static UnityWebRequestProcess GetProcess(this UnityWebRequestAsyncOperation asyncOperation)
    {
        return new UnityWebRequestProcess(asyncOperation);
    }
}