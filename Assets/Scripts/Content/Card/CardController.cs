using System;
using UnityEngine;

public class CardController : IDisposable
{
    public event Action<CardController> Selected;
    public event Action<CardController> RemoveHandled;
    
    public Transform Transform => _cardView.transform;
    public int Id { get; private set; }
    
    private readonly CardView _cardView;
    private readonly CardBehaviour _cardBehaviour;

    public CardController(CardView cardView, int id)
    {
        _cardView = cardView;
        Id = id;
        _cardBehaviour = _cardView.GetComponent<CardBehaviour>();
        _cardBehaviour.OnContentShown += OnContentShown;
        _cardBehaviour.RemoveHandled += OnRemoveHandled;
    }

    private void OnContentShown()
    {
        Selected?.Invoke(this);
    }
    
    private void OnRemoveHandled()
    {
        RemoveHandled?.Invoke(this);
    }

    public void HandleRemove()
    {
        _cardBehaviour.HandleRemove();
    }

    public void Reset()
    {
        _cardBehaviour.Reset();
    }

    public void Dispose()
    {
        _cardBehaviour.OnContentShown -= OnContentShown;
        _cardBehaviour.RemoveHandled -= OnRemoveHandled;
    }
}