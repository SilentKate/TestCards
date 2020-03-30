using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareLevelHandler : IGameStateHandler
{
    public event Action<bool> Done;
    
    private readonly IScreenController _screenController;
    private readonly IRoundController _roundController;

    public PrepareLevelHandler(
        [NotNull] IScreenController screenController,
        [NotNull] IRoundController roundController)
    {
        _screenController = screenController ?? throw new ArgumentNullException(nameof(screenController));
        _roundController = roundController ?? throw new ArgumentNullException(nameof(roundController));
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
        _roundController.Setup();
        Done?.Invoke(true);
    }

    public void Dispose()
    {
        Done = null;
    }
}