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

	TileMap sceneryMap;

	AnimatedSprite roverUp;
	AnimatedSprite roverDown;

	AnimatedSprite optics;
	AnimatedSprite charge;
	AnimatedSprite temp;
	AnimatedSprite opticsDown;
	AnimatedSprite chargeDown;
	AnimatedSprite tempDown;

	HUD HUD;

	bool hasOptics = false;
	bool hasTemp = false;
	bool hasCharge = false;


	float heat = 50;
	float maxHeat = 100;
	float battery = 100;
	float minBattery = 0;
	float maxBattery = 100;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		game = (GameManager)GetNode("/root/Main/Manager");
		HUD = (HUD)GetNode("/root/Main/Camera2D/HUD");

		roverUp = (AnimatedSprite)GetNode("RoverUp");
		roverDown = (AnimatedSprite)GetNode("RoverDown");

		optics = (AnimatedSprite)GetNode("RoverUp/Optics");
		opticsDown = (AnimatedSprite)GetNode("RoverDown/OpticsDown");
		charge = (AnimatedSprite)GetNode("RoverUp/Charge");
		chargeDown = (AnimatedSprite)GetNode("RoverDown/ChargeDown");
		temp = (AnimatedSprite)GetNode("RoverUp/Temp");
		tempDown = (AnimatedSprite)GetNode("RoverDown/TempDown");

		sceneryMap = (TileMap)GetNode("/root/Main/WorldSpace/Node2D/ParentMap/Scenery");


		Position = new Vector2(16, 0);
		movementCooldown = GetNode<Timer>("MovementCooldown");

	}

	public void GetInput()
	{
    // bool canMoveForward = ...
		if (movementCooldown.IsStopped())
		{
			velocity = new Vector2(); // (0, 0)
      // up && canMoveForward
			if (Input.IsActionPressed("up"))
			{
				velocity = new Vector2(1, -1) * tileSize / 2;
				Position += velocity.Rotated(rotationDirection).Snapped(tileSize / 2);
				// start cooldown
				movementCooldown.Start();
				GetCurrentTile();
				DrainBattery();
			}
			else if (Input.IsActionPressed("down"))
			{
				velocity = new Vector2(-1, 1) * tileSize / 2;
				Position += velocity.Rotated(rotationDirection).Snapped(tileSize / 2);
				// start cooldown
				movementCooldown.Start();
				GetCurrentTile();
				DrainBattery();
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
    // GD.Print("rotation direction = " + rotationDirection);
	}
	public override void _Process(float delta)
	{
		game.playerPosition = GlobalPosition;
    GD.Print("player position = " + game.playerPosition);
		GetInput();
	}


	public void DrainBattery()
	{
		if (battery >= minBattery)
		{
			battery = battery -= 2;
			HUD.UpdateBattery(battery);
		}
		else
		{
			// die
		}
	}

	public void SetTile(Tile.TileData tile)
	{
		tile.used = true;
	}


	public void GetCurrentTile()
	{
		foreach (var node in game.tileDict)
		{
			if (game.refPosition == node.Value.coord - new Vector2(1,1) && node.Value.step == 1)
			{
        GD.Print("current tile key = " + node.Key);
        GD.Print("current tile tag = " + node.Value.tag);
				if (node.Value.tag == "item" && hasOptics == false && node.Value.used == false)
				{
					hasOptics = true;
					optics.Visible = true;
					opticsDown.Visible = true;
					game.visionRange = 5;
					game.green = true;
					SetTile(node.Value);
					return;
				}
				if (node.Value.tag == "item" && hasCharge == false)
				{
					hasCharge = true;
					charge.Visible = true;
					chargeDown.Visible = true;
					HUD.battery.Visible = true;
					game.violet = true;
					SetTile(node.Value);
					return;
				}
				if (node.Value.tag == "item" && hasTemp == false)
				{
					hasTemp = true;
					temp.Visible = true;
					tempDown.Visible = true;
					HUD.heat.Visible = true;
					game.orange = true;
					game.blue = true;
					SetTile(node.Value);
					return;
				}
				if (node.Value.tag == "charge")
				{
					battery = battery += (maxBattery - battery);
					if (battery >= maxBattery)
					{
						battery = maxBattery;
						HUD.UpdateBattery(battery);
					}
					else
					{
						HUD.UpdateBattery(battery);
					}
				}
			}

			else if (game.refPosition == node.Value.coord && node.Value.step == 0)
			{
				if (node.Value.tag == "desert")
				{
					heat = heat += 2;
					HUD.UpdateHeat(heat);
				}
				if (node.Value.tag == "tundra")
				{
					heat = heat -= 2;
					HUD.UpdateHeat(heat);
				}
				if (node.Value.tag == "grass" && heat >= 51 || node.Value.tag == "null" && heat >= 51)
				{
					heat = heat -= 2;
					HUD.UpdateHeat(heat);
					if (heat <= 50)
					{
						heat = heat = 50;
						HUD.UpdateHeat(heat);
					}
				}

			}
		}
	}


 }

