using System;

public class CardController
{
    public event Action<CardController> Selected;
    public event Action<CardController> RemoveHandled;
    
    public int Id { get; set; }
    public CardView CardView { private get; set; }
    
    private readonly CardBehaviour _cardBehaviour;
    private readonly CardState _cardState;

//    public CardController()
//    {
//        _cardBehaviour = CardView.GetComponent<CardBehaviour>();
//        _cardState = CardState.None;
//    }
    
    public void HandleRemove()
    {
    }

    public void Reset()
    {
    }
}