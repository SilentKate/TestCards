using System;

public interface IScreenController
{
    event Action Hidden;
    event Action Shown;
    
    void Setup();
    void Cleanup();

    void Show();
    void Hide();
}