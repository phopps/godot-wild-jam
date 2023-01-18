using Godot;
using System;

public class Player : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	public int temp;
	[Export]
	public int maxTemp;
	[Export]
	public int minTemp;

	public int charge;
	[Export]
	public int maxCharge;


	public Vector2 up = new Vector2(0, -1);
	public Vector2 right = new Vector2(1, 0);
	public Vector2 left = new Vector2(0, 1);
	public Vector2 down = new Vector2(-1, 0);

	int tileSize = 32;

	GameManager game;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		game = (GameManager)GetNode("/root/Main/Manager");

		Position = Position.Snapped(new Vector2(1, 1) * tileSize);
		Position += new Vector2(1, 1) * tileSize / 2;
	}

	public void Move(Vector2 dir)
	{
		Position += dir;
	}

	public void CheckInputs()
	{
		if (Input.IsActionPressed("move_9"))
		{
			Move(up);
		}
		if (Input.IsActionPressed("move_7"))
		{
			Move(down);
		}
		if (Input.IsActionPressed("move_1"))
		{
			Move(left);
		}
		if (Input.IsActionPressed("move_3"))
		{
			Move(right);
		}
	}


	/*

	Get the X value of the Isometric scale w/ x-y & get the Y w/ x+y halved;

	*/
	public Vector2 CartesianToIso(Vector2 v)
	{
		return new Vector2(v.x - v.y, (v.x + v.y) / 2);
	}

	public override void _Process(float delta)
	{
		game.playerPosition = GlobalPosition;
		// Position = Position.Snapped(new Vector2(1,osition = GlobalPosition;
		CheckInputs();
	}
}
