using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class CardBehaviour : MonoBehaviour, IPointerClickHandler
{
    [Serializable]
    public struct StateConfig
    {
        public CardState state;
        public string animationTrigger;
    }

    public event Action OnContentShown;
    public event Action RemoveHandled;
    
    [SerializeField] private StateConfig[] _configs;
    
    private Animator _animator;
    private int _currentTrigger;

    private CardState State
    {
        set
        {
            if (_cardState == value) return;
            _cardState = value;
            ProcessCurrentState(value);
        }
        get => _cardState;
    }
    private CardState _cardState = CardState.ContentHidden;
    
    [UsedImplicitly]
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentTrigger = 0;
    }

    [UsedImplicitly]
    private void Start()
    {
        State = CardState.Appeared;
    }

    [UsedImplicitly]
    public void Selected()
    {
        OnContentShown?.Invoke();
    }
    
    [UsedImplicitly]
    public void Disappeared()
    {
        RemoveHandled?.Invoke();
    }
    
    [UsedImplicitly]
    public void Appeared()
    {
        Reset();
    }

    public void Reset()
    {
        State = CardState.ContentHidden;
    }

    public void HandleRemove()
    {
        State = CardState.Disappear;
    }
    
    private void ProcessCurrentState(CardState value)
    {
        foreach (var config in _configs)
        {
            if (config.state != value) continue;
            ProcessTrigger(config.animationTrigger);
        }
    }

    private void ProcessTrigger(string trigger)
    {
        var newTrigger = Animator.StringToHash(trigger);
        _animator.ResetTrigger(_currentTrigger);
        _currentTrigger = newTrigger;
        _animator.SetTrigger(_currentTrigger);
    }

    [UsedImplicitly]
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        if (State == CardState.ContentHidden || State == CardState.Appeared)
        {
            State = CardState.ContentShown;
        }
    }
}