using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MenuScreenViewModel
{
    public Action LevelButtonAction => _gameFlowService.ChangeState;
    public List<Result> Results => _resultService.GetResults(Select);
    
    private const int BestResultsCount = 5;
    
    private readonly IGameFlowService _gameFlowService;
    private readonly IResultService _resultService;

    public MenuScreenViewModel([NotNull] IGameFlowService gameFlowService)
    {
        _gameFlowService = gameFlowService ?? throw new ArgumentNullException(nameof(gameFlowService));
        _resultService = App.Resolve<IResultService>();
    }
    
    private List<Result> Select(List<Result> results)
    {
        results.Sort((a,b) => b.score.CompareTo(a.score));
        return results.GetRange(0, Mathf.Min(results.Count, BestResultsCount));
    }
}