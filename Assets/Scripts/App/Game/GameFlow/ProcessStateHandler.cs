using System;

public class ProcessStateHandler : ChainElement
{
    private readonly Func<IGameStateHandler> _factory;

    public ProcessStateHandler(Func<IGameStateHandler> factory)
    {
        _factory = factory;
    }
    
    public override void Handle(object context = null)
    {
        var handler = _factory?.Invoke();
        if (handler == null)
        {
            HandleInterrupted();
            return;
        }
        
        handler.Done += 
            (result) =>
            {
                handler.Dispose();
                if (result)
                {
                    HandleNext();
                }
                else
                {
                    HandleInterrupted();
                }
            };
        handler.Handle();
    }
}