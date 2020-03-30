using System;

public interface IGameStateHandler : IDisposable
{
    event Action<bool> Done;
    void Handle();
}