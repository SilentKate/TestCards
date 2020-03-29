using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.Networking;

public class UnityWebRequestProcess : IDisposable
{
    private UnityWebRequestAsyncOperation _asyncOperation;
    private List<Action<UnityWebRequest>> _handlers = new List<Action<UnityWebRequest>>();
    private bool _isDisposed;

    public UnityWebRequestProcess(UnityWebRequestAsyncOperation asyncOperation)
    {
        _asyncOperation = asyncOperation;
        _asyncOperation.completed += OnRequestCompleted;
    }

    public void AddHandler(Action<UnityWebRequest> handler)
    {
        if (_isDisposed) return;
        if (_handlers.Contains(handler)) return;
        
        _handlers.Add(handler);
        
        if (_asyncOperation.isDone)
        {
            handler?.Invoke(_asyncOperation.webRequest);
        }
    }

    private void OnRequestCompleted(UnityEngine.AsyncOperation obj)
    {
        foreach (var handler in _handlers)
        {
            handler?.Invoke(_asyncOperation.webRequest);
        }
    }

    public void Dispose()
    {
        _isDisposed = true;
        _handlers.Clear();
        _handlers = null;
        _asyncOperation.completed -= OnRequestCompleted;
        _asyncOperation = null;
    }
}