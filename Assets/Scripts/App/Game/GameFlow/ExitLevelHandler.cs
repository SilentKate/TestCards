using System;
using JetBrains.Annotations;

public class ExitLevelHandler : IGameStateHandler
{
    public event Action<bool> Done;
    
    private readonly IScreenController _screenController;
    private readonly IRoundController _roundController;

    public ExitLevelHandler(
        [NotNull] IScreenController screenController,
        [NotNull] IRoundController roundController)
    {
        _screenController = screenController ?? throw new ArgumentNullException(nameof(screenController));
        _roundController = roundController ?? throw new ArgumentNullException(nameof(roundController));
    }
    
    public void Handle()
    {
        _roundController.Cleanup();
        _screenController.Hidden += OnHidden;
        _screenController.Hide();
    }

    private void OnHidden()
    {
        _screenController.Hidden -= OnHidden; 
        _screenController.Cleanup();
        Done?.Invoke(true);
    }

    public void Dispose()
    {
        Done = null;
    }
}