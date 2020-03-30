using UnityEngine;
using UnityEngine.UI;

public class LevelScreenView : MonoBehaviour, IDataContextHandler<LevelScreenViewModel>
{
    [SerializeField] private CanvasGroup _wrapper;
    [SerializeField] private Button _button;
    
    private LevelScreenViewModel _viewModel;

    public LevelScreenViewModel DataContext
    {
        set
        {
            Cleanup();
            _viewModel = value;
            if (_viewModel != null)
            {
                Setup();
            }
        }
    }

    public void Show()
    {
        _wrapper.alpha = 1;
    }

    public void Hide()
    {
        _wrapper.alpha = 0;
    }
    
    private void Setup()
    {
        _button.onClick.AddListener(OnClicked);
    }

    private void Cleanup()
    {
        _button.onClick.RemoveListener(OnClicked);
    }
    
    private void OnClicked()
    {
        _viewModel.ExitButtonAction();
    }
}