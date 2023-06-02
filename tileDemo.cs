using Godot;

public class tileDemo : Node2D
{
    private TileMap map;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        map = GetNode<TileMap>("TileMap"); // store a reference to the tilemap so we can play with it.
        GD.Print("Tilemap Ready");
    }

    public void _on_Button_pressed()
    {
        GD.Print("GetCell at 4,-1:");
        int getcell = map.GetCell(4, -1); // what is the ID of the tile being used at this grid location?
        GD.Print(getcell);
        toggleCell(4, -1);
    }

    private void toggleCell(int x, int y)
    {
        int prev = map.GetCell(x, y);
        if (prev == 2)
        { //since 2 is my ugly white cell image, this is the one we're toggling off/on
            map.SetCell(x, y, 1);
        }
        else
        {
            map.SetCell(x, y, 2);
        }
    }
}
