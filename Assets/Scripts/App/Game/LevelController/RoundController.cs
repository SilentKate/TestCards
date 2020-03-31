using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class RoundController : IRoundController
{
    private readonly RulesCollection _rulesCollection;
    private readonly IResultService _resultService;
    private CardsController _cardsController;
    private RulesCollection.RoundRules _currentRules;

    private int _roundCompletedCount = 0;
    
    public RoundController(
        [NotNull] IResultService resultService,
        [NotNull] RulesCollection rulesCollection)
    {
        _resultService = resultService ?? throw new ArgumentNullException(nameof(resultService));
        _rulesCollection = rulesCollection != null ? rulesCollection : throw new ArgumentNullException(nameof(rulesCollection));
    }

    public void Setup()
    {
        _roundCompletedCount = 0;
        _cardsController = new CardsController();
        _cardsController.CardSelected += OnCardCardSelected;
        _cardsController.BunchRemoved += OnBunchRemoved;
    }

    public void Cleanup()
    {
        _cardsController.CardSelected -= OnCardCardSelected;
        _cardsController.BunchRemoved -= OnBunchRemoved;
        _cardsController.Dispose();
        if (_roundCompletedCount > 0)
        {
            _resultService.SaveResult(new Result {score = _roundCompletedCount});
        }
    }

    public void StartRound()
    {
        _currentRules = _rulesCollection.GetRandom();
        _cardsController.SpawnBunches(_currentRules.bunchesCount, _currentRules.bunchCapacity);
        if (_cardsController.TotalCardsCount == 0)
        {
            throw new InvalidOperationException("RoundController :: StartRound : Can't spawn cards!");
        }
    }

    private void OnCardCardSelected()
    {
        if (_cardsController.CurrentSelectedCount > _currentRules.selectedCount)
        {
            _cardsController.Reset();
            return;
        }

        var cardToRemoveIds = new List<int>();
        foreach (var kvp in _cardsController.SelectedCountsById)
        {
            if (kvp.Value == _currentRules.bunchCapacity)
            {
                cardToRemoveIds.Add(kvp.Key);
            }
        }

        _cardsController.RemoveBunches(cardToRemoveIds);
    }

    private void OnBunchRemoved()
    {
        if (_cardsController.TotalCardsCount > 0) return;
        _roundCompletedCount++;
        StartRound();    
    }
}