using Godot;
using System;

public class GridMap : TileMap
{
	/*
	
	TILEMAP EXTENSION DESIGNED TO USE TILE VALUES
	
	- burcarz
	
	*/

	public Godot.Collections.Array cells;
	GameManager game;


	[Export]
	public Vector2 size;

	public Vector2 cellSelected;
	
	// size of cell pixels
	[Export]
	public Vector2 cellSize;
	
	[Export]
	public float cellHalf;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cells = GetUsedCells();
		cellSize = new Vector2(32, 16);
		game = (GameManager)GetNode("/root/Main/Manager");
		// cellSize = TileMap.CellSize;

		foreach (var c in cells)
		{
			// GD.Print(c.Position);
		}
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{	

			var mouseSelect = WorldToMap(GetGlobalMousePosition());
			var cell = calcuteGridCoordinates(mouseSelect);
			var position = calculateMapPosition(cell);
			var tilePosition = WorldToMap(position);
			var cellSelected = GetCellv(tilePosition);

			game.selectedTile = mouseSelect;

			GD.Print(mouseSelect + " " + "Hovering over cell");
		}
	}

	public Vector2 calculateMapPosition(Vector2 gridPosition)
	{
		return gridPosition * cellSize * cellHalf;
	}

	/*

	returns the coordinates of a cell on the grid

	*/

	public Vector2 calcuteGridCoordinates(Vector2 mapPosition)
	{
		Vector2 gridCoordinates = (mapPosition /  cellSize);
		
		// may need to use MATH FLOOR HERE
		return gridCoordinates;
	}

	/*

	returns the x y coordinates to make sure the cell is in bounds of the grid

	*/

	public bool IsInBounds(Vector2 cellCoordinates)
	{
		bool outOfBounds = cellCoordinates.x >= 0 && cellCoordinates.x < size.x;
		return outOfBounds && cellCoordinates.y >= 0 && cellCoordinates.y < size.y;
	}

	/* 

	clamp function returns x and y values smaller than the size of the grid

	*/
	public Vector2 GridClamp(Vector2 gridPosition)
	{
		Vector2 newGridPosition = gridPosition;
		newGridPosition.x = Mathf.Clamp(Mathf.Round(newGridPosition.x), 0, Mathf.Round(size.x) - 1);
		newGridPosition.y = Mathf.Clamp(Mathf.Round(newGridPosition.y), 0, Mathf.Round(size.x) - 1);
		return newGridPosition;
	}

	public int CellToIndex(Vector2 cell)
	{
		int index = (int) Mathf.Round(cell.x += size.x * cell.y);
		return index;
	}
}
