using Godot;
using System;

public class Tile : Node2D
{
	public struct TileData
	{
		public Vector2 coord;
		public int index;
		public string name;
		public bool colorActive;
	}
}

public class TileDictionary<TileData> : System.Collections.Generic.Dictionary<int, Tile.TileData>
{
	public void Add(int key, Vector2 coord, string name, int index, bool colorActive)
	{
		Tile.TileData val;
		val.coord = coord;
		val.index = index;
		val.name = name;
		val.colorActive = colorActive;
		this.Add(key, val);
	}
}


