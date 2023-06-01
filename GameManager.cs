using System;
using Godot;

public class GameManager : Node
{

    /*

	THIS IS A SINGLETON / AUTOLOADER

	- burcarz
	*/

    // public static GameManager Instance;

    public Vector2 selectedTile;
    public Vector2 playerPosition;

    public TileDictionary<Tile.TileData> tileDict = new TileDictionary<Tile.TileData>();

    public Vector2 refPosition;

    public int visionRange;

    public bool green;
    public bool orange;
    public bool blue;
    public bool violet;

    public int time;
    [Export]
    public int dayLength;
    [Export]
    public float rate;

    [Export]
    public float light;
    [Export]
    public float maxLight = 1.5f;
    [Export]
    public float maxDark = 0.3f;

    TileMap map;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        map = (GridMap)GetNode("/root/Main/WorldSpace/Node2D/ParentMap");
        map.Modulate = new Color(light, light, light, 1);
        time = dayLength / 2;
    }

    public override void _Process(float delta)
    {
        DayNightCycle();
    }

    public void DayNightCycle()
    {
        if (time < dayLength)
        {
            time++;
            if (time <= dayLength / 2)
            {
                if (light <= maxLight)
                {
                    map.Modulate = new Color(light + rate, light + rate, light + rate, 1);
                    light = light += rate;
                }
            }
            else if (time >= dayLength / 2)
            {
                if (light >= maxDark)
                {
                    map.Modulate = new Color(light - rate, light - rate, light - rate, 1);
                    light = light -= rate;
                }
            }
        }
        else
        {
            time = 0;
        }
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (Input.IsActionPressed("ui_accept"))
        {
            // DayNightCycle();
        }
    }
}
