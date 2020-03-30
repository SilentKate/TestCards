using System;
using JetBrains.Annotations;

public class LevelScreenViewModel
{
    public Action ExitButtonAction => _gameFlowService.ChangeState;
    
    private const int BestResultsCount = 5;
    
    private readonly IGameFlowService _gameFlowService;
    private readonly IResultService _resultService;

    public LevelScreenViewModel([NotNull] IGameFlowService gameFlowService)
    {
        _gameFlowService = gameFlowService ?? throw new ArgumentNullException(nameof(gameFlowService));
    }
}