using Godot;
using System;

[Tool]
public class Grid : Resource
{
	// size of grid columns / rows
	[Export]
	public Vector2 size;
	
	// size of cell pixels
	[Export]
	public Vector2 cellSize;
	
	[Export]
	public float cellHalf;
	
	// [Export]
	// public Resource SubResource;

	// [Export]
	// public Grid GridData = ResourceLoader.Load<Grid>("res://Resources/Grid.tres");

	// public override void _Ready()
	// {
	// 	size = new Vector2(20, 20);
	// 	cellSize = new Vector2(80, 80);

	// 	cellHalf = cellSize / 2;
	// }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	/*

	returs the position of the cell's center;

	*/


}
