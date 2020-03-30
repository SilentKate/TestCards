using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _foreground;
    [SerializeField] private SpriteRenderer _background;
    
    public Sprite ForegroundSprite
    {
        set => _foreground.sprite = value;
    }
    
    public Sprite BackgroundSprite
    {
        set => _background.sprite = value;
    }
}
