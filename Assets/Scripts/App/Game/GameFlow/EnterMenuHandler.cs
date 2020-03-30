using System;
using JetBrains.Annotations;

public class EnterMenuHandler : IGameStateHandler
{
    public event Action<bool> Done;
    
    private readonly IScreenController _screenController;

    public EnterMenuHandler([NotNull] IScreenController screenController)
    {
        _screenController = screenController ?? throw new ArgumentNullException(nameof(screenController));
    }
    
    public void Handle()
    {
        _screenController.Shown += OnShown;
        _screenController.Show();
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