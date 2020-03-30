using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameFlowService : IGameFlowService
{
    private readonly Dictionary<GameFlowState, Func<IGameStateHandler>> _statePrepareFactories = new Dictionary<GameFlowState, Func<IGameStateHandler>>
    {
        { GameFlowState.None, () => new AlwaysSuccessStateHandler()},
        { GameFlowState.Menu, () => new PrepareMenuHandler(App.Resolve<MenuScreenController>())},
        { GameFlowState.Level, () => new PrepareLevelHandler(
            App.Resolve<LevelScreenController>(),
            App.Resolve<IRoundController>())}
    }; 
    
    private readonly Dictionary<GameFlowState, Func<IGameStateHandler>> _stateEnterFactories = new Dictionary<GameFlowState, Func<IGameStateHandler>>
    {
        { GameFlowState.None, () => new AlwaysSuccessStateHandler()},
        { GameFlowState.Menu, () => new EnterMenuHandler(App.Resolve<MenuScreenController>())},
        { GameFlowState.Level, () => new EnterLevelHandler(
            App.Resolve<LevelScreenController>(),
            App.Resolve<IRoundController>())}
    };
    
    private readonly Dictionary<GameFlowState, Func<IGameStateHandler>> _stateExitFactories = new Dictionary<GameFlowState, Func<IGameStateHandler>>
    {
        { GameFlowState.None, () => new AlwaysSuccessStateHandler()},
        { GameFlowState.Menu, () => new ExitMenuHandler(App.Resolve<MenuScreenController>())},
        { GameFlowState.Level, () => new ExitLevelHandler(
            App.Resolve<LevelScreenController>(),
            App.Resolve<IRoundController>())}
    };

    private readonly IGameFlowContainer _container;
    private SimpleChain _chain;

    public GameFlowService([NotNull] IGameFlowContainer container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
    }
    
    public void ChangeState()
    {
        if (_chain != null) return;
        if (_container.NextState == _container.CurrentState) return;

        _stateExitFactories.TryGetValue(_container.CurrentState, out var exitCurrent);
        _statePrepareFactories.TryGetValue(_container.NextState, out var prepareNext);
        _stateEnterFactories.TryGetValue(_container.NextState, out var enterNext);
        
        _chain = new SimpleChain();
        _chain.Add(new ProcessStateHandler(exitCurrent));
        _chain.Add(new ProcessStateHandler(prepareNext));
        _chain.Add(new ProcessStateHandler(enterNext));
        _chain.Done += OnChainDone;
        _chain.Process();
    }

    private void OnChainDone(bool success)
    {
        _chain.Dispose();
        _chain = null;
        if (success)
        {
            _container.CurrentState = _container.NextState;
        }
        else
        {
            Debug.LogError($"GameFlowService :: OnChainDone : Can't process state {_container.NextState}");
        }
    }
}