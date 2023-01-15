using Godot;
using System;

[Tool]
public class Grid : Resource
{
	
	/*
	
	THIS WAS AN ATTEMPT AT CREATING A CUSTOM RESOURCE FOR A GRID
	THE RESOURCE WORKS AND THE VALUES CAN BE USED BUT IM UNSURE HOW
	TO FULLY IMPLEMENT
	
	- burcarz
	
	*/
	
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
