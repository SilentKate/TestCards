using System.Collections.Generic;
using UnityEngine;

public class ResultsContainer : IResultContainer, ISnapshotHandler
{
    private List<Result> _resultsCollection = new List<Result>();
    
    public void AddResult(Result result)
    {
        _resultsCollection.Add(result);
        _resultsCollection = FilterResults(_resultsCollection, GameSettings.ResultRuntimeCapacity);
    }

    public IEnumerable<Result> GetResults()
    {
        return _resultsCollection;
    }

    public void ApplySnapshot(string value)
    {
        _resultsCollection.AddRange(GetResultFromSnapshot(value));
        _resultsCollection = FilterResults(_resultsCollection, GameSettings.ResultRuntimeCapacity);
    }
    
    public string TakeSnapshot()
    {
        var resultsToSave = FilterResults(_resultsCollection, GameSettings.ResultSaveSnapshotCapacity);
        return JsonHelper.ToJson(resultsToSave);
    }

    private IEnumerable<Result> GetResultFromSnapshot(string value)
    {
       return JsonHelper.FromJson<Result>(value);
    }

    private List<Result> FilterResults(
        List<Result> resultsCollection, 
        int count)
    {
        resultsCollection.Sort((a,b) => b.score.CompareTo(a.score));
        var range = Mathf.Min(count, resultsCollection.Count);
        return resultsCollection.GetRange(0, range);
    }
}