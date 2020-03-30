using System;

public class ExitMenuHandler : IGameStateHandler
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