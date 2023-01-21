using Godot;
using System;

public class Player : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export] public int speed = 100;
	public Vector2 velocity = new Vector2();
	public Vector2 tileSize = new Vector2(32, 16);
	public int rotationDirection = 0;
	private Timer movementCooldown;
	GameManager game;

	AnimatedSprite roverUp;
	AnimatedSprite roverDown;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		game = (GameManager)GetNode("/root/Main/Manager");
		roverUp = (AnimatedSprite)GetNode("RoverUp");
		roverDown = (AnimatedSprite)GetNode("RoverDown");
		Position = new Vector2(16, 0);
		movementCooldown = GetNode<Timer>("MovementCooldown");

	}

	public void GetInput()
	{
		if (movementCooldown.IsStopped())
		{
			velocity = new Vector2(); // (0, 0)
			if (Input.IsActionPressed("up"))
			{
				velocity = new Vector2(1, -1) * tileSize / 2;
				Position += velocity.Rotated(rotationDirection).Snapped(tileSize / 2);
				// start cooldown
				movementCooldown.Start();
			}
			else if (Input.IsActionPressed("down"))
			{
				velocity = new Vector2(-1, 1) * tileSize / 2;
				Position += velocity.Rotated(rotationDirection).Snapped(tileSize / 2);
				// start cooldown
				movementCooldown.Start();
			}
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("left"))
		{
			if (rotationDirection == 4)
			{
				rotationDirection -= 1;
				
			}
			else if (rotationDirection == 3)
			{
				rotationDirection -= 2;
			}
			else
			{
				rotationDirection += 4;
				if (rotationDirection >= 5)
				{
					rotationDirection = 0;
				}
			}
		}
		if (inputEvent.IsActionPressed("right"))
		{
			if (rotationDirection == 1)
			{
				rotationDirection += 2;
				
			}
			else
			{
				rotationDirection += 1;
				if (rotationDirection >= 5)
				{
					rotationDirection = 0;
				}
			}
		}
		GD.Print(rotationDirection);

		if (rotationDirection == 1)
		{
			roverDown.Visible = true;
			roverUp.Visible = false;
			roverDown.Scale = new Vector2(1, 1);
		}
		else if (rotationDirection == 0)
		{
			roverUp.Visible = true;
			roverDown.Visible = false;
			roverUp.Scale = new Vector2(1, 1);
		}
		else if (rotationDirection == 4)
		{
			roverUp.Visible = true;
			roverDown.Visible = false;
			roverUp.Scale = new Vector2(-1, 1);
		}
		else if (rotationDirection == 3)
		{
			roverUp.Visible = false;
			roverDown.Visible = true;
			roverDown.Scale = new Vector2(-1, 1);
		}
	}
	public override void _Process(float delta)
	{
		game.playerPosition = GlobalPosition;
		GetInput();
	}
 }
	
