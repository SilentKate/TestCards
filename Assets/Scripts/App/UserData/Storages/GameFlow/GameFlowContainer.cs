using System.Collections.Generic;

public class GameFlowContainer : IGameFlowContainer
{
    public GameFlowState CurrentState { get; set; }
    public GameFlowState NextState => _transitions.TryGetValue(CurrentState, out var nextState) ? nextState : CurrentState;

    private readonly Dictionary<GameFlowState, GameFlowState> _transitions = new Dictionary<GameFlowState, GameFlowState>
    {
        { GameFlowState.None, GameFlowState.Menu },
        { GameFlowState.Menu, GameFlowState.Level },
        { GameFlowState.Level, GameFlowState.Menu }
    };
}