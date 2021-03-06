﻿using System;
using JetBrains.Annotations;

public class ExitMenuHandler : IGameStateHandler
{
    public event Action<bool> Done;
    
    private readonly IScreenController _screenController;

    public ExitMenuHandler([NotNull] IScreenController screenController)
    {
        _screenController = screenController ?? throw new ArgumentNullException(nameof(screenController));
    }
    
    public void Handle()
    {
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