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

    public IEnumerable<Result> GetResults(
        Func<IEnumerable<Result>,
        IEnumerable<Result>> selector = null)
    {
        var results = _container.GetResults();
        return selector != null 
            ? selector(results)
            : results;
    }
}