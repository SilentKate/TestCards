using System;
using JetBrains.Annotations;

public class EnterLevelHandler : IGameStateHandler
{
    public event Action<bool> Done;
    
    private readonly IScreenController _screenController;
    private readonly IRoundController _roundController;

    public EnterLevelHandler(
        [NotNull] IScreenController screenController,
        [NotNull] IRoundController roundController)
    {
        _screenController = screenController ?? throw new ArgumentNullException(nameof(screenController));
        _roundController = roundController ?? throw new ArgumentNullException(nameof(roundController));
    }

    public void Handle()
    {
        _screenController.Shown += OnShown;
        _screenController.Show();
        _roundController.StartRound();
    }

    private void OnShown()
    {
        _screenController.Shown -= OnShown; 
        Done?.Invoke(true);
    }

    public void Dispose()
    {
        Done = null;
    }
}