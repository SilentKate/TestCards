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
        TryCreateFile(GetPath(fileName));
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
            File.WriteAllText(GetPath(kvp.Key), data);
        } 
    }

    private string TryLoadFromFile(string fileName)
    {
        try
        {
            var result = File.ReadAllText(GetPath(fileName));
            return result;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return string.Empty;
    }

    private string GetPath(string fileName)
    {
        return Path.Combine(_persistentDataPath, fileName);
    }
    
    private void TryCreateFile(string persistentDataPath)
    {
        if (!File.Exists(persistentDataPath))
        {
            File.Create(persistentDataPath);
        }
    }
}