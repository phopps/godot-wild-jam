using System;
using Godot;

public class CameraController : Camera2D
{

    [Export]
    float limiter = 0;
    [Export]
    float speed = 0;

    GameManager game;

    public override void _Ready()
    {
        game = (GameManager)GetNode("/root/Main/Manager");
    }

    public override void _PhysicsProcess(float delta)
    {

        if (game.playerPosition.x >= GlobalPosition.x + limiter)
        {
            GlobalPosition = GlobalPosition.LinearInterpolate(game.playerPosition, delta * speed);
        }

        else if (game.playerPosition.x <= GlobalPosition.x + limiter)
        {
            GlobalPosition = GlobalPosition.LinearInterpolate(game.playerPosition, delta * speed);
        }

        if (game.playerPosition.y >= GlobalPosition.y - limiter)
        {
            GlobalPosition = GlobalPosition.LinearInterpolate(game.playerPosition, delta * speed);
        }

        else if (game.playerPosition.y <= GlobalPosition.y - limiter)
        {
            GlobalPosition = GlobalPosition.LinearInterpolate(game.playerPosition, delta * speed);
        }
    }
}
