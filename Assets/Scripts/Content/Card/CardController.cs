using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(CardController))]
[RequireComponent(typeof(CardView))]
public class CardController : MonoBehaviour
{
    private CardView _cardView;
    private CardBehaviour _cardBehaviour;
    private CardState _cardState;

    [UsedImplicitly]
    private void Awake()
    {
        _cardView = GetComponent<CardView>();
        _cardBehaviour = GetComponent<CardBehaviour>();
        _cardState = CardState.None;
    }

    public void Load()
    {
    }

    public void Show()
    {
    }

    public void Hide()
    {
    }
}