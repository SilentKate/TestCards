using System;
using System.Collections.Generic;

public interface IResultService
{
    void SaveResult(Result result);
    IEnumerable<Result> GetResults(Func<IEnumerable<Result>, IEnumerable<Result>> selector = null);
}