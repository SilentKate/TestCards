using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareLevelHandler : IGameStateHandler
{
    public event Action<bool> Done;
    
    private readonly IScreenController _screenController;

    public PrepareLevelHandler([NotNull] IScreenController screenController)
    {
        _screenController = screenController ?? throw new ArgumentNullException(nameof(screenController));
    }
   
    public void Handle()
    {
        var asyncOperation = SceneManager.LoadSceneAsync(GameSettings.LevelSceneId);
        asyncOperation.completed += OnSceneLoaded;
        if (asyncOperation.isDone)
        {
            OnSceneLoaded(asyncOperation);
        }
    }

    private void OnSceneLoaded(AsyncOperation asyncOperation)
    {
        asyncOperation.completed -= OnSceneLoaded;
        _screenController.Setup();
        Done?.Invoke(true);
    }

    public void Dispose()
    {
        Done = null;
    }
}