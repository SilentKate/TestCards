using System;

public class ExitLevelHandler : IGameStateHandler
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public event Action<bool> Done;
    public void Handle()
    {
        throw new NotImplementedException();
    }
}