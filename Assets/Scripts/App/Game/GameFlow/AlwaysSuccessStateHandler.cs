using System;

public class AlwaysSuccessStateHandler : IGameStateHandler
{
    public event Action<bool> Done;
    
    public void Handle()
    {
        Done?.Invoke(true);
    }
    
    public void Dispose()
    {
    }

}