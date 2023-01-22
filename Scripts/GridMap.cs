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

	TileMap groundMap;
	TileMap sceneryMap;

	TileDictionary<Tile.TileData> tileDict = new TileDictionary<Tile.TileData>();

	[Export]
	public Vector2 cellSize;
	public Vector2 cellHalf;
	[Export]
	public Vector2 size;

	public Vector2 cellSelected;
	public Vector2 cellPosition;

	int k;

	public enum tags
	{
		grass,
		desert,
		tundra,
		charge,
		item,
		water,
		empty
	}

	[Export]
	public int visionRange;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// tags = ["grass", "desert", "tundra", "charge", "rubble", "water"];
		
		cellHalf = cellSize / 2;
		game = (GameManager)GetNode("/root/Main/Manager");

		groundMap = (TileMap)GetNode("Ground");
		sceneryMap = (TileMap)GetNode("Scenery");

		// k is the tile key
		k = 0;
		int e;
		e = 0;
					GD.Print(e);
		FillDictionary(groundMap, e);
		e = 1;
					GD.Print(e);
		FillDictionary(sceneryMap, e);
	}


	public void FillDictionary(TileMap map, int elevation)
	{
		cells = map.GetUsedCells();

		int i;

		foreach (var c in cells)
		{
			Vector2 cellData;
			cellData =  (Vector2)c;
			// i is index of the tile
			i = map.GetCellv(cellData);

			string name = map.TileSet.TileGetName(i);
			int temp;
			string tag;

			tag = GetTileType(name);
			temp = GetTemp(tag);

			if (name == null)
			{
				name = "empty";
			}
			tileDict.Add(k, cellData, name, i, tag, temp, elevation);
			k++;
		}
	}


	/* 
	absolutely disgusting garbage if else bullshit can't think of a better way to check this
	returns the tag associated with the tile;
	will absolutely break and doesn't really work the way it should im sure of it
	*/
	public string GetTileType(string n)
	{
		string tag = "null";
		for (int i = 0; i < Enum.GetNames(typeof(tags)).Length; i++)
		{
			if(n.Contains(tags.grass.ToString()))
			{
				tag = tags.grass.ToString();
			}
			else if (n.Contains(tags.desert.ToString()))
			{
				tag = tags.desert.ToString();
			}
			else if (n.Contains(tags.tundra.ToString()))
			{
				tag = tags.tundra.ToString();
			}
			else if (n.Contains(tags.charge.ToString()))
			{
				tag = tags.charge.ToString();
			}
			else if (n.Contains(tags.item.ToString()))
			{
				tag = tags.item.ToString();
			}
			else if (n.Contains(tags.water.ToString()))
			{
				tag = tags.water.ToString();
			}		
		}
		return tag;
	}

	// returns the tempeture of the tile based on its tag

	public int GetTemp(string tag)
	{
		int temp = 0;
		if (tag == "tundra")
		{
			temp = -1;
		}
		else if (tag == "desert" || tag == "water")
		{
			temp = 1;
		}
		return temp;
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
			SetZIndex(cellSelected);
	}

	public void SetZIndex(Vector2 refPosition)
	{
		foreach (var node in tileDict)
		{
			if (node.Value.coord >= refPosition && node.Value.step == 1)
			{
				sceneryMap.ZIndex = 0;
			}
			else
			{
				sceneryMap.ZIndex = 2;
			}
		}
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
					groundMap.SetCellv(node.Value.coord, -1);
					sceneryMap.SetCellv(node.Value.coord, -1);
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
				if (node.Value.step == 0)
				{
					groundMap.SetCellv(node.Value.coord, node.Value.index);
				}

				else if (node.Value.step == 1)
				{
					sceneryMap.SetCellv(node.Value.coord, node.Value.index);
				}
				
				
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
				if (game.violet == true)
				{
					SetVioletTiles(node.Value);
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
				if (tile.step == 0)
				{
					groundMap.SetCellv(tile.coord, tile.index);
				}

				else if (tile.step == 1)
				{
					sceneryMap.SetCellv(tile.coord, tile.index);
				}
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
				if (tile.step == 0)
				{
					groundMap.SetCellv(tile.coord, tile.index);
				}

				else if (tile.step == 1)
				{
					sceneryMap.SetCellv(tile.coord, tile.index);
				}
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
				if (tile.step == 0)
				{
					groundMap.SetCellv(tile.coord, tile.index);
				}

				else if (tile.step == 1)
				{
					sceneryMap.SetCellv(tile.coord, tile.index);
				}
		}
	}

	public void SetVioletTiles(Tile.TileData tile)
	{
		int i = TileSet.FindTileByName($"{tile.name}violet");

		if (tile.name.Contains("violet"))
		{
			return;
		}
		else if (1 <= i && i <= 100)
		{
			tile.name = $"{tile.name}violet";
			tile.index = i;
				if (tile.step == 0)
				{
					groundMap.SetCellv(tile.coord, tile.index);
				}

				else if (tile.step == 1)
				{
					sceneryMap.SetCellv(tile.coord, tile.index);
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
