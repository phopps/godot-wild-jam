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
    GameManager game;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        game = (GameManager)GetNode("/root/Main/Manager");
        Position = new Vector2(16, 0);
    }

    public void GetInput()
    {
        velocity = new Vector2(); // (0, 0)

        if (Input.IsActionPressed("up"))
        {
            // velocity = new Vector2(32, -16).Rotated(rotationDirection);
            velocity = new Vector2(32, -16).Rotated(rotationDirection);
            velocity = velocity.Snapped(tileSize) * 2;

        }
        if (Input.IsActionPressed("down"))
        {
            velocity = new Vector2(-32, 16).Rotated(rotationDirection);
            velocity = velocity.Snapped(tileSize);
        }

    }

    public override void _Input(InputEvent inputEvent)
    {
        // if (inputEvent.IsActionPressed("left"))
        // {
        //     RotationDegrees -= 90;
        // }
        // if (inputEvent.IsActionPressed("right"))
        // {
        //     RotationDegrees += 90;
        // }
        // var playerLocation = Position;
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

        // if (inputEvent.IsActionPressed("up"))
        // {
        //     playerLocation.x += 32;
        //     playerLocation.y -= 16;
        //     Position = playerLocation;
        // }
    }
    public override void _Process(float delta)
    {
        game.playerPosition = GlobalPosition;
        GetInput();
        Position += velocity;
        // Position += velocity * delta;
        GD.Print(Position.Snapped(tileSize));
    }
}
