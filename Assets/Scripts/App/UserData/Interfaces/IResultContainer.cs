using System.Collections.Generic;

public interface IResultContainer
{
     void AddResult(Result result);
     IEnumerable<Result> GetResults();
}