using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class App : MonoBehaviour
{
    public static T Resolve<T>() where T : class 
    {
        var type = typeof(T);
        if (_dependencies.TryGetValue(type, out var service)) return service as T;
        throw new InvalidOperationException("App :: Services : Try to access a non-existent service");
    }
    private static readonly Dictionary<Type, object> _dependencies = new Dictionary<Type, object>();

    [SerializeField] private AppResourcesConfig _resourcesConfig;
    [SerializeField] private MenuScreenView _menuScreenView;
    [SerializeField] private LevelScreenView _levelScreenView;
    [SerializeField] private RulesCollection _rulesCollection;
    
    private UserDataStorage _userDataStorage;
    private AppPersistenceService _appPersistenceService;
    private ExternalAssetValidator _assetsValidator;

    [UsedImplicitly]
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    [UsedImplicitly]
    private void Start()
    {
        _menuScreenView.Hide();
        _levelScreenView.Hide();
        
        
        _userDataStorage = new UserDataStorage();
        _appPersistenceService = new AppPersistenceService(Path.Combine(Application.persistentDataPath, "local"));
        SetupUserPersistence(_userDataStorage);
        SetupAssetsServices();
        SetupUserServices(_userDataStorage);
        SetupGameServices();
        ValidateResources();
    }

    private void SetupAssetsServices()
    {
        var storage = new AssetsStorage();
        _dependencies.Add(typeof(AssetsStorage), storage);
        _assetsValidator = new ExternalAssetValidator(_resourcesConfig.GetSources(), storage);
    }

    private void ValidateResources()
    {
        var chain = _assetsValidator.DownloadResourcesIfNeed();
        chain.Done += success => OnValidateResourcesDone(chain, success);
        chain.Process();
    }

    private void OnValidateResourcesDone(SimpleChain chain, bool success)
    {
        chain.Dispose();
        if (success)
        {
            Debug.Log("Hooray!");
            Resolve<IGameFlowService>().ChangeState();
        }
        else
        {
            _assetsValidator.ClearCachedVersions();
            Application.Quit();
        }
    }

    private void SetupUserPersistence(UserDataStorage userDataStorage)
    {
        _appPersistenceService.Register(userDataStorage.Results, "results");
        _appPersistenceService.ApplyFromSaveFile();
    }

    private void SetupUserServices(UserDataStorage userDataStorage)
    {
        _dependencies.Add(typeof(IResultService), new UserResultService(userDataStorage.Results));
    }
    
    private void SetupGameServices()
    {
        var gameFlowService = new GameFlowService(new GameFlowContainer());
        _dependencies.Add(typeof(IGameFlowService), gameFlowService);
        _dependencies.Add(typeof(MenuScreenController), new MenuScreenController(gameFlowService, _menuScreenView));
        _dependencies.Add(typeof(LevelScreenController), new LevelScreenController(gameFlowService, _levelScreenView));
        _dependencies.Add(typeof(IRoundController), new RoundController(Resolve<IResultService>(), _rulesCollection));
    }

    [UsedImplicitly]
    private void OnApplicationQuit()
    {
        _appPersistenceService?.ApplyToSaveFile();
    }

    [UsedImplicitly]
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            var resultsService = Resolve<IResultService>();
            var result = new Result {score = Random.Range(5, 100)};
            resultsService.SaveResult(result);
        }
        
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}