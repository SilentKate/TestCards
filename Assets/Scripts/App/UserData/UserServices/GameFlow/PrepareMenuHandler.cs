using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareMenuHandler : IGameStateHandler
{
    public event Action<bool> Done;
   
    public void Handle()
    {
        var asyncOperation = SceneManager.LoadSceneAsync(GameSettings.MenuSceneId);
        asyncOperation.completed += OnSceneLoaded;
        if (asyncOperation.isDone)
        {
            OnSceneLoaded(asyncOperation);
        }
    }

    private void OnSceneLoaded(AsyncOperation asyncOperation)
    {
        asyncOperation.completed -= OnSceneLoaded;
        Done?.Invoke(true);
    }

    public void Dispose()
    {
        Done = null;
    }
}