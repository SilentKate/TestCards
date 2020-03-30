using System;
using System.Collections.Generic;

public interface IResultService
{
    void SaveResult(Result result);
    List<Result> GetResults(Func<List<Result>, List<Result>> selector = null);
}