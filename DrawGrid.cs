using Godot;
using System;
using System.Linq;

public class DrawGrid : Node2D
{
	
	/*
		I was playing around here with a secondary grid draw,
		this does draw lines but it does not do so isometrically,
		I wanted to use a 2D camera and canvas size as references but
		I don't know how to do that in godot
		
		so the sizes are arbitrary !!
		
		- burcarz
	*/

	[Export]
	public bool on = true;

	public Vector2 cam;
	public Vector2 size;

	// Called when the node enters the scene tree for the first time.
	public void _Draw()
	{

		cam = new Vector2(10,10);
		size = new Vector2(10,10);

		if (on == true)
		{
			// size = GetViewPortRect().Size * GetParent().GetNode("Camera2D").zoom / 2;
			// cam = GetParent().GetNode("Camera2D").position;
			
			int camA = (int)(((cam.x - size.x) / 64) - 1);
			int camB = (int)(((size.x + cam.x) / 64) + 1);

			int camC = (int)(((cam.y - size.y) / 64) - 1);
			int camD = (int)(((size.y + cam.y) / 64) + 1);

			foreach (int i in Enumerable.Range(camA, camB))
			{
				DrawLine(new Vector2(i * 32, cam.y + size.y + 100), new Vector2(i * 32, cam.y - size.y - 100), new Color(255,0,0));
			}
			foreach (int i in Enumerable.Range(camC, camD))
			{
				DrawLine(new Vector2(i * 32, cam.x + size.x + 100), new Vector2(i * 32, cam.x - size.x - 100), new Color(255,0,0));
			}
		   
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		
	}
}
