using Godot;

[Tool]
public class Vision : Node
{
    public Grid grid = ResourceLoader.Load<Grid>("res://Resources/Grid.tres");
    public int visionRange;
    public Player player;
    public Vector2 cell = Vector2.Zero;
}
