using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppPersistenceService
{
    private readonly string _persistentDataPath;
    private readonly Dictionary<string, ISnapshotHandler> _snapshotHandlersByFileName
        = new Dictionary<string, ISnapshotHandler>();

    public AppPersistenceService(string directoryPath)
    {
        _persistentDataPath = !string.IsNullOrEmpty(directoryPath)
            ? directoryPath 
            : throw new ArgumentNullException(nameof(directoryPath));
    }

    public void Register(ISnapshotHandler snapshotHandler, string fileName)
    {
        _snapshotHandlersByFileName[fileName] = snapshotHandler;
    }
    
    public void ApplyFromSaveFile()
    {
        foreach (var kvp in _snapshotHandlersByFileName)
        {
            var data = TryLoadFromFile(kvp.Key);
            kvp.Value.ApplySnapshot(data);
        }
    }
    
    public void ApplyToSaveFile()
    {
        foreach (var kvp in _snapshotHandlersByFileName)
        {
            var data = kvp.Value.TakeSnapshot();
            File.WriteAllText(PersistenceHelper.GetPath(_persistentDataPath, kvp.Key), data);
        } 
    }

    private string TryLoadFromFile(string fileName)
    {
        try
        {
            var path = PersistenceHelper.GetPath(_persistentDataPath, fileName);
            if (!File.Exists(path)) return string.Empty;
            var result = File.ReadAllText(path);
            return result;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return string.Empty;
    }
  
}