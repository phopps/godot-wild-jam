using Godot;
using System;

public class GridMap : TileMap
{
	/*
	
	TILEMAP EXTENSION DESIGNED TO USE TILE VALUES
	
	- burcarz
	
	*/

	public Godot.Collections.Array cells;
	public Godot.Collections.Dictionary<int, Vector2> tileDictionary = new Godot.Collections.Dictionary<int, Vector2>();
	GameManager game;

	[Export]
	public Vector2 cellSize;
	public Vector2 cellHalf;
	[Export]
	public Vector2 size;

	public Vector2 cellSelected;
	public Vector2 cellPosition;

	[Export]
	public int visionRangeX;
	[Export]
	public int visionRangeY;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cells = GetUsedCells();
		cellHalf = cellSize / 2;

		game = (GameManager)GetNode("/root/Main/Manager");

		int k = 0;
		foreach (var c in cells)
		{
			Vector2 cellData;
			cellData = (Vector2) c;

			// k = GetCell((int)cellData.x, (int)cellData.y);

			tileDictionary.Add(k, cellData);
			k++;
		}
	}

	/*

	ATTEMPT AT A MOUSE OVER TO SELECT TILE

	*/
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{	

			// GD.Print(game.playerPosition);

			// game.playerPosition = CalculateMapPosition(game.playerPosition);

			// GD.Print(CalculateGridCoordinates(game.playerPosition) + " " + "GRID COORDINATES");
			// GD.Print(IsInBounds(game.playerPosition) + " " + "IN BOUNDS");
			// GD.Print(CellToIndex(game.playerPosition) + " " + "INDEX OF CELL");
			// GD.Print(GridClamp(game.playerPosition) + " " + "CLAMPED GRID COORDINATES");
			// GD.Print(CalculateMapPosition(game.playerPosition) + " " + "MAP POSITION ON GRID");

			cellSelected = WorldToMap(game.playerPosition);
			var tile_id = GetCellv(cellSelected);
			GD.Print(tile_id + " " + cellSelected);

			SetTiles(cellSelected);
			UnSetTiles(cellSelected);
		}
	}

	/*

	CONVERTS THE TILES VECTOR TO INT FOR SETTING INDEX WHEN SETTING TILES

	*/

	public int GetTileIndex(Vector2 tile)
	{
		int index = GetCell((int)tile.x, (int)tile.y);
		return index;
	}

	public void UnSetTiles(Vector2 refPosition)
	{
		/*

			UNSET TILES OUTSIDE OF X/Y VIEW RANGE
			
		*/

		for (int i = 0; i < tileDictionary.Count; i++)
		{
			if (tileDictionary[i].x > refPosition.x + visionRangeX)
			{
				SetCellv(tileDictionary[i], -1);
			}
			if (tileDictionary[i].x < refPosition.x - visionRangeX)
			{
				SetCellv(tileDictionary[i], -1);
			}
			if (tileDictionary[i].y > refPosition.y + visionRangeY)
			{
				SetCellv(tileDictionary[i], -1);
			}
			if (tileDictionary[i].y < refPosition.y - visionRangeY)
			{
				SetCellv(tileDictionary[i], -1);
			}
			// else
			// {
			// 	SetCellv(tileDictionary[i], 1);
			// }
		}
	}

	public void SetTiles(Vector2 refPosition)
	{

		/*

			SET TILES INSDE X/Y VIEW RANGE

		*/

		foreach (var node in tileDictionary)
		{
			if (node.Value.x < refPosition.x + 5)
			{
				SetCellv(node.Value, GetTileIndex(node.Value));
			}
			if (node.Value.x > refPosition.x - 5)
			{
				SetCellv(node.Value, GetTileIndex(node.Value));
			}
			if (node.Value.y < refPosition.y + 5)
			{
				SetCellv(node.Value, GetTileIndex(node.Value));
			}
			if (node.Value.y > refPosition.y - 5)
			{
				SetCellv(node.Value, GetTileIndex(node.Value));
			}
		}
	}

	public Vector2 CalculateMapPosition(Vector2 gridPosition)
	{
		return gridPosition * cellSize + cellHalf;
	}

	/*

	returns the coordinates of a cell on the grid

	*/

	public Vector2 CalculateGridCoordinates(Vector2 mapPosition)
	{
		Vector2 gridCoordinates = (mapPosition /  cellSize);
		
		// may need to use MATH FLOOR HERE
		// return new Vector2(Mathf.Floor(gridCoordinates.x), Mathf.Floor(gridCoordinates.y));
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
