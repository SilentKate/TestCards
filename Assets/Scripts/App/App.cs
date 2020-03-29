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
        if (_services.TryGetValue(type, out var service)) return service as T;
        throw new InvalidOperationException("App :: Services : Try to access a non-existent service");
    }
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    [SerializeField] private AppResourcesConfig _resourcesConfig;
    
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
        _userDataStorage = new UserDataStorage();
        _appPersistenceService = new AppPersistenceService(Path.Combine(Application.persistentDataPath, "local"));
        SetupUserPersistence(_userDataStorage);
        SetupUserServices(_userDataStorage);
        ValidateResources();
    }

    private void ValidateResources()
    {
        _assetsValidator = new ExternalAssetValidator(_resourcesConfig.GetSources());
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
        }
        else
        {
            _assetsValidator.ClearCachedVersions();
            Application.Quit();
        }
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
            var resultsService = App.Resolve<IResultService>();
            var result = new Result {score = Random.Range(5, 100)};
            resultsService.SaveResult(result);
        }

        if (Input.GetKey(KeyCode.S))
        {
            var resultsService = App.Resolve<IResultService>();
            var results = resultsService.GetResults();
            var str = "Current results: ";
            foreach (var result in results)
            {
                str += $"; {result.score}";
            }
            Debug.Log(str);
        }
    }

    private void SetupUserPersistence(UserDataStorage userDataStorage)
    {
        _appPersistenceService.Register(userDataStorage.Results, "results");
        
        _appPersistenceService.ApplyFromSaveFile();
    }

    private void SetupUserServices(UserDataStorage userDataStorage)
    {
        _services.Add(typeof(IResultService), CreateUserResultService(userDataStorage.Results));
    }

    private IResultService CreateUserResultService(IResultContainer container)
    {
        return new UserResultService(container);
    }
}
