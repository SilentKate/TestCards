using System;
using JetBrains.Annotations;

public class MenuScreenController : IScreenController
{
    public event Action Hidden;
    public event Action Shown;
    
    private readonly IGameFlowService _gameFlowService;
    private readonly MenuScreenView _view;

    public MenuScreenController(
        [NotNull] IGameFlowService gameFlowService,
        [NotNull] MenuScreenView view)
    {
        _gameFlowService = gameFlowService ?? throw new ArgumentNullException(nameof(gameFlowService));
        _view = view != null ? view : throw new ArgumentNullException(nameof(view));
    }

    public void Setup()
    {
        _view.DataContext = new MenuScreenViewModel(_gameFlowService);
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