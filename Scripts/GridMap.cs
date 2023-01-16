using Godot;
using System;

public class GridMap : TileMap
{
	/*
	
	TILEMAP EXTENSION DESIGNED TO USE TILE VALUES
	
	- burcarz
	
	*/

	public Godot.Collections.Array cells;
	public Godot.Collections.Dictionary<Vector2, int> tileDictionary = new Godot.Collections.Dictionary<Vector2, int>();
	GameManager game;

	[Export]
	public Vector2 cellSize;
	public Vector2 cellHalf;
	[Export]
	public Vector2 size;

	public Vector2 cellSelected;
	public Vector2 cellPosition;

	[Export]
	public int visionRange;
	double radius;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cells = GetUsedCells();
		cellHalf = cellSize / 2;
		game = (GameManager)GetNode("/root/Main/Manager");

		int k = 0;
		// cells = GetUsedCellsById(1);
		foreach (var c in cells)
		{
			Vector2 cellData;
			cellData = (Vector2) c;
			k = GetCellv(cellData);
			tileDictionary.Add(cellData, k);
			// k++;
		}
	}

	/*
	THIS METHOD DRAWS A CIRCLE AROUND THE PLAYER WITH TILES IN THEIR VISION RANGE
	*/

	public bool InCircle(Vector2 player, Vector2 tile)
	{
		float dx = player.x - tile.x;
		float dy = player.y - tile.y;
		float distance = Mathf.Sqrt(dx*dx + dy*dy);
		return distance <= visionRange;
	}

	public override void _Process(float delta)
	{
			cellSelected = WorldToMap(game.playerPosition);
						SetTiles(cellSelected);
			UnSetTiles(cellSelected);
	}

	/*
	CONVERTS THE TILES VECTOR TO INT FOR SETTING INDEX WHEN SETTING TILES
	*/

	public int GetTileIndex(Vector2 tile)
	{
		int index = GetCellv(tile);

		return index;
	}

	public void UnSetTiles(Vector2 refPosition)
	{
		/*
			UNSET TILES OUTSIDE OF X/Y VIEW RANGE
		*/
		foreach (var node in tileDictionary)
		{

			if (InCircle(refPosition, node.Key) == false)
			{
				SetCellv(node.Key, -1);
			}
		}
	}

	public void SetTiles(Vector2 refPosition)
	{
		/*
			SET TILES INSDE X/Y VIEW RANGE
		*/
		foreach (var node in tileDictionary)
		{
			if (InCircle(refPosition, node.Key) == true)
			{
				SetCellv(node.Key, node.Value);
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
