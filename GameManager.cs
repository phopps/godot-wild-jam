using Godot;
using System;

public class GameManager : Node
{
	
	/* 
	
	THIS IS A SINGLETON / AUTOLOADER
	
	- burcarz
	*/

	// public static GameManager Instance;

	public Vector2 selectedTile;
	public Vector2 playerPosition;

	public bool green;
	public bool orange;
	public bool blue;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

}
