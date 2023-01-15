using Godot;
using System;

public class Player : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	public Vector2 up = new Vector2(0, -1);
	public Vector2 right = new Vector2(1, 0);
	public Vector2 left = new Vector2(0, 1);
	public Vector2 down = new Vector2(-1, 0);


	Vector2 direction;
	Vector2 target;

	Vector2 position;

	GameManager game;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		game = (GameManager)GetNode("/root/Main/Manager");
	}

	public void Move()
	{
		target = game.selectedTile;
		if (target != null)
		{
		Position = target;
		GD.Print(Position);
		}
	}

	public override void _Process(float delta)
	{
		Move();
	}
}
