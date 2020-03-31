using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreenView : MonoBehaviour, IDataContextHandler<MenuScreenViewModel>
{
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private Text _label;
    [SerializeField] private Button _button;
    
    private MenuScreenViewModel _viewModel;

    public MenuScreenViewModel DataContext
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
        _wrapper.SetActive(true);
    }

    public void Hide()
    {
        _wrapper.SetActive(false);
    }
    
    private void Setup()
    {
        var strBuilder = new StringBuilder();
        foreach (var result in _viewModel.Results)
        {
            strBuilder.Append($"{result.score}; ");
        }
        _label.text = strBuilder.ToString();
        _button.onClick.AddListener(OnClicked);
    }

    private void Cleanup()
    {
        _label.text = string.Empty;
        _button.onClick.RemoveListener(OnClicked);
    }
    
    private void OnClicked()
    {
        _viewModel.LevelButtonAction();
    }
}