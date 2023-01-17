using Godot;
using System;

public class Tile : Node2D
{

	public struct TileData
	{
		public Vector2 coord;
		public int step;
		public int index;
		public string biome;
		public bool color;
	}
}

public class TileDictionary<TileData> : System.Collections.Generic.Dictionary<int, Tile.TileData>
{

	public void Add(int key, Vector2 coord, int step, int index)
	{
		Tile.TileData val;
		val.coord = coord;
		val.step = step;
		val.index = index;
		this.Add(key, val);
	}
}


