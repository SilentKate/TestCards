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

    [SerializeField] private StateConfig[] _configs;
    
    private Animator _animator;
    private int _currentTrigger;

    [UsedImplicitly]
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentTrigger = 0;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
    }
}