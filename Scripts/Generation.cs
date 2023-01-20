using Godot;
using System;

public class Generation : TileMap
{


	Vector2 cellSize = new Vector2(32, 16);
	float mapSize = 1024;
	float width;
	float height;
	private Vector2[] dirs;

	
	private Vector2 GetRandXY()
	{
		dirs = new Vector2[]
		{
			new Vector2(-1, 0),
			new Vector2(1, 0),
			new Vector2(0, -1),
			new Vector2(0, 1),
		};
		Vector2 dir = dirs[GD.Randi() % 4];
		// Vector2 dir = dirs[index];
		return new Vector2(dir[0], dir[1]);
	}

	private void CreatePath()
	{
		int maxCycle = 1000;
		int cycle = 0;

		width = mapSize / cellSize.x;
		height = mapSize / cellSize.y;

		Vector2 w = Vector2.Zero;

		while (cycle <= maxCycle)
		{
			Vector2 dir = GetRandXY();
			if (
			dir.x + w.x >= 0 &&
			dir.x + w.x < width &&
			dir.y + w.y >= 0 &&
			dir.y + w.y < height
			)
			{
				w += dir;
				SetCellv(w, 1);
				cycle++;
			}
		}
	}

	private void ClearTiles()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Clear();
			}
		}
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Randomize();
		ClearTiles();
		CreatePath();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (Input.IsActionPressed("ui_accept"))
		{
			GD.Randomize();
			ClearTiles();
			CreatePath();
		};
	}
}
