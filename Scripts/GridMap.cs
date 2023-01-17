using Godot;
using System;

public class GridMap : TileMap
{
	/*
	
	TILEMAP EXTENSION DESIGNED TO USE TILE VALUES
	
	- burcarz
	
	*/

	public Godot.Collections.Array cells;
	// public Godot.Collections.Dictionary<Vector2, int> tileDictionary = new Godot.Collections.Dictionary<Vector2, int>();
	
	// TileDictionary<int, TValue> tileDict = new TileDictionary<int, TValue>();
	// TileDictionaryint  tileDict;
	GameManager game;

	TileDictionary<Tile.TileData> tileDict = new TileDictionary<Tile.TileData>();

	[Export]
	public Vector2 cellSize;
	public Vector2 cellHalf;
	[Export]
	public Vector2 size;

	public Vector2 cellSelected;
	public Vector2 cellPosition;

	[Export]
	public int visionRange;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cells = GetUsedCells();
		cellHalf = cellSize / 2;
		game = (GameManager)GetNode("/root/Main/Manager");



		// k is the tile key
		int k = 0;
		int i;
		// cells = GetUsedCellsById(1);
		foreach (var c in cells)
		{
			Vector2 cellData;
			cellData =  (Vector2)c;
			// i is index of the tile
			i = GetCellv(cellData);
			// n is name of the tile in a string for reference later
			string n = TileSet.TileGetName(i);
			if (n == null)
			{
				n = "empty";
			}
			tileDict.Add(k, cellData, n, i, false);
			k++;
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
			UnSetTiles(cellSelected);
			SetTiles(cellSelected);
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
		foreach (var node in tileDict)
		{

			if (InCircle(refPosition, node.Value.coord) == false)
			{
				SetCellv(node.Value.coord, -1);
			}
		}
	}

	public void SetTiles(Vector2 refPosition)
	{
		/*
			SET TILES INSDE X/Y VIEW RANGE
		*/
		foreach (var node in tileDict)
		{
			if (InCircle(refPosition, node.Value.coord) == true)
			{
				SetCellv(node.Value.coord, node.Value.index);
				if (game.green == true)
				{
					SetGreenTiles(node.Value);
				}
				if (game.blue == true)
				{
					SetBlueTiles(node.Value);
				}
				if (game.orange == true)
				{
					SetOrangeTiles(node.Value);
				}
				
			}
		}
	}

	public void SetGreenTiles(Tile.TileData tile)
	{
		int i = TileSet.FindTileByName($"{tile.name}green");

		if (tile.name.Contains("green"))
		{
			return;
		}
		else if (1 <= i && i <= 100)
		{
			tile.name = $"{tile.name}green";
			tile.index = i;
			SetCellv(tile.coord, tile.index);
		}
	}

	public void SetOrangeTiles(Tile.TileData tile)
	{
		int i = TileSet.FindTileByName($"{tile.name}orange");

		if (tile.name.Contains("orange"))
		{
			return;
		}
		else if (1 <= i && i <= 100)
		{
			tile.name = $"{tile.name}orange";
			tile.index = i;
			SetCellv(tile.coord, tile.index);
		}
	}

	public void SetBlueTiles(Tile.TileData tile)
	{
		int i = TileSet.FindTileByName($"{tile.name}blue");

		if (tile.name.Contains("blue"))
		{
			return;
		}
		else if (1 <= i && i <= 100)
		{
			tile.name = $"{tile.name}blue";
			tile.index = i;
			SetCellv(tile.coord, tile.index);
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



	public override void _Input(InputEvent inputEvent)
	{
		if (Input.IsActionPressed("ui_accept"))
		{
			game.orange = true;
			game.blue = true;
			game.green = true;
		}
		else
		{
			game.blue = false;
			game.orange = false;
			game.green = false;
		}
	}
}
