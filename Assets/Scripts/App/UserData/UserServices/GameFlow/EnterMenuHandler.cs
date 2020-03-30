using System;

public class EnterMenuHandler : IGameStateHandler
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