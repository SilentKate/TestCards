using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameFlowService : IGameFlowService
{
    private readonly Dictionary<GameFlowState, Func<IGameStateHandler>> _statePrepareFactories = new Dictionary<GameFlowState, Func<IGameStateHandler>>
    {
        { GameFlowState.Menu, () => new PrepareMenuHandler()},
        { GameFlowState.Level, () => new PrepareLevelHandler()}
    }; 
    
    private readonly Dictionary<GameFlowState, Func<IGameStateHandler>> _stateEnterFactories = new Dictionary<GameFlowState, Func<IGameStateHandler>>
    {
        { GameFlowState.Menu, () => new EnterMenuHandler()},
        { GameFlowState.Level, () => new EnterLevelHandler()}
    };
    
    private readonly Dictionary<GameFlowState, Func<IGameStateHandler>> _stateExitFactories = new Dictionary<GameFlowState, Func<IGameStateHandler>>
    {
        { GameFlowState.Menu, () => new ExitMenuHandler()},
        { GameFlowState.Level, () => new ExitLevelHandler()}
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
        var nextState = _container.NextState;
        if (nextState == _container.CurrentState) return;

        _statePrepareFactories.TryGetValue(nextState, out var prepare);
        _stateEnterFactories.TryGetValue(nextState, out var enter);
        _stateExitFactories.TryGetValue(nextState, out var exit);
        
        _chain = new SimpleChain();
        _chain.Add(new ProcessStateHandler(prepare));
        _chain.Add(new ProcessStateHandler(enter));
        _chain.Add(new ProcessStateHandler(exit));
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