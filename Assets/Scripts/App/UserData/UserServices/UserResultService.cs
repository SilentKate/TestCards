using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class UserResultService : IResultService
{
    private readonly IResultContainer _container;

    public UserResultService(
        [NotNull] IResultContainer container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
    }

    public void SaveResult(Result result)
    {
        _container.AddResult(result);
    }

    public List<Result> GetResults(
        Func<List<Result>,
            List<Result>> selector = null)
    {
        var results = new List<Result>();
        results.AddRange(_container.GetResults());
        return selector != null 
            ? selector(results)
            : results;
    }
}