using UnityEngine;

[CreateAssetMenu(menuName = "Sprite Style")]
public class SpriteStyle : ScriptableObject
{
    public Sprite NormalSprite;
    public Color NormalColor;
    
    public Sprite HightlightedSprite;
    public Color HighlightedColor;

    public Sprite SelectedSprite;
    public Color SelectedColor;
}