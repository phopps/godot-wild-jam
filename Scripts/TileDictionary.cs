using Godot;
using System;

public class TileDictionary<TileData> : System.Collections.Generic.Dictionary<int, Tile.TileData>
{
	public void Add(int key, Vector2 coord, string name, int index, string tag, int temp, int step, bool used)
	{
		Tile.TileData val;
		val.coord = coord;
		val.index = index;
		val.name = name;
		val.tag = tag;
		val.temp = temp;
		val.step = step;
		val.used = used;
		this.Add(key, val);
	}
}

public class Tile : Node2D
{
	public struct TileData
	{
		public Vector2 coord;
		public int index;
		public int temp;
		public string name;
		public string tag;
		public int step;
		public bool used;
	}
}


