using Godot;

namespace Game.Scripts.Data;
public class CardData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Texture2D Icon { get; set; }
    public PackedScene Scene { get; set; }
}