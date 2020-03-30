﻿using System;
using JetBrains.Annotations;

public class LevelScreenController : IScreenController
{
    public event Action Hidden;
    public event Action Shown;
    
    private readonly IGameFlowService _gameFlowService;
    private readonly LevelScreenView _view;

    public LevelScreenController(
        [NotNull] IGameFlowService gameFlowService,
        [NotNull] LevelScreenView view)
    {
        _gameFlowService = gameFlowService ?? throw new ArgumentNullException(nameof(gameFlowService));
        _view = view != null ? view : throw new ArgumentNullException(nameof(view));
    }

    public void Setup()
    {
        _view.DataContext = new LevelScreenViewModel(_gameFlowService);
    }

    public void Cleanup()
    {
        _view.DataContext = null;
    }

    public void Show()
    {
        _view.Show();
        Shown?.Invoke();
    }

    public void Hide()
    {
        _view.Hide();
        Hidden?.Invoke();
    }
}