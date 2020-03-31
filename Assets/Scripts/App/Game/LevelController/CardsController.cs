using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

public class CardsController : IDisposable
{
    public event Action BunchRemoved;
    public event Action CardSelected;
    
    public int CurrentSelectedCount { get; private set; }
    public int TotalCardsCount => _currentCards?.Count ?? 0;
    
    public Dictionary<int, int> SelectedCountsById => _selectedCountsById;
    private readonly Dictionary<int, int> _selectedCountsById = new Dictionary<int, int>();
    
    private AssetsStorage _assets;
    private CardConfigsCollection _configs;
    private List<CardController> _currentCards;
    private List<CardController> _processingCards;
    private SpriteAtlas _atlas;
    
    private readonly SimpleGrid _evenGrid;
    private readonly SimpleGrid _oddGrid;

    public CardsController()
    {
        _assets = App.Resolve<AssetsStorage>();
        _configs = _assets.GetCardAsset<CardConfigsCollection>("CardCollection");
        _atlas = _assets.GetCardAsset<SpriteAtlas>("Cards");
        _evenGrid = Object.FindObjectOfType<EvenGrid>();
        _oddGrid = Object.FindObjectOfType<OddGrid>();
    }
    
    public void Dispose()
    {
        _configs = null;
        _atlas = null;
        _assets = null;
    }
    
    public void SpawnBunches(
        int bunchesCount, 
        int bunchesCapacity)
    {
        Reset();
        
        var totalCount = bunchesCount * bunchesCapacity;
        var countInRow = totalCount / GameSettings.RowCount;
        var grid = countInRow % 2 == 0 ? _evenGrid : _oddGrid;
        while (totalCount > grid.Points.Length)
        {
            bunchesCount--;
            totalCount = bunchesCount * bunchesCapacity;
            grid = countInRow % 2 == 0 ? _evenGrid : _oddGrid;
        }
        
        var configs = _configs.GetRandomConfigs(bunchesCount);
        _currentCards = GetCards(configs, bunchesCapacity);
        LayoutCards(_currentCards, grid);
    }

    public void RemoveBunches(List<int> ids)
    {
        _processingCards = _currentCards.FindAll(value => ids.Contains(value.Id));
        _currentCards.RemoveAll(value => ids.Contains(value.Id));
        foreach (var card in _processingCards)
        {
            card.RemoveHandled += OnCardRemoveHandled;
            card.HandleRemove();
        }
    }

    private void OnCardRemoveHandled(CardController card)
    {
        card.RemoveHandled -= OnCardRemoveHandled;
        _processingCards.Remove(card);
        card.Dispose();
        if (_processingCards.Count > 0) return;
        BunchRemoved?.Invoke();
    }

    public void Reset()
    {
        CurrentSelectedCount = 0;
        _selectedCountsById.Clear();
        if (_currentCards == null) return;
        foreach (var card in _currentCards)
        {
            card.Reset();
        }
    }

    private void AddSelected(CardController cardController)
    {
        CurrentSelectedCount++;
        _selectedCountsById.TryGetValue(cardController.Id, out var count);
        count++;
        _selectedCountsById[cardController.Id] = count;
        CardSelected?.Invoke();
    }
    
    private void LayoutCards(
        List<CardController> currentCards, 
        SimpleGrid grid)
    {
        currentCards.Shuffle();
        for (var i = 0; i < currentCards.Count; i++)
        {
            var card = currentCards[i];
            var point = grid.Points[i];
            card.Transform.position = point.transform.position;
        }
    }

    private List<CardController> GetCards(
        List<CardConfigsCollection.CardConfig> configs, 
        int bunchesCapacity)
    {
        var result = new List<CardController>(); 
        foreach (var config in configs)
        {
            for (var i = 0; i < bunchesCapacity; i++)
            {
                var card = new CardController(GetView(config), config.id);
                card.Selected += AddSelected;
                result.Add(card);
            }
        }
        return result;
    }

    private CardView GetView(CardConfigsCollection.CardConfig config)
    {
        var go = Resources.Load(_configs.prefabName) as GameObject;
        var view = Object.Instantiate(go).GetComponent<CardView>();
        if (view == null) throw new InvalidOperationException($"Cards controller :: GetView: Can't load card view from config {config.id}");
        view.BackgroundSprite = _atlas.GetSprite(_configs.backgroundSpriteName);
        view.ForegroundSprite = _atlas.GetSprite(config.foregroundSpriteName);
        return view;
    }
    
}