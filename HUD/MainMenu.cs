using System;
using Godot;

public class MainMenu : CanvasLayer
{

    // [Signal]
    // public delegate void IntroFinished();

    private AnimationPlayer anim;
    private bool creditsVisible = false;

    private AudioStreamPlayer audioMainMenu;
    private AudioStreamPlayer audioLevel;
    private AudioStreamPlayer audioMenuUp;
    private AudioStreamPlayer audioMenuDown;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        audioMainMenu = GetNode<AudioStreamPlayer>("/root/Main/Audio/MainMenu");
        audioLevel = GetNode<AudioStreamPlayer>("/root/Main/Audio/Level");
        audioMenuUp = GetNode<AudioStreamPlayer>("/root/Main/Audio/MenuUp");
        audioMenuDown = GetNode<AudioStreamPlayer>("/root/Main/Audio/MenuDown");
        audioMainMenu.Play();
        anim = GetNode<AnimationPlayer>("MainAnimator");
        anim.Play("Intro");
        anim.Queue("WaitingLoop");
    }

    public void StartButtonPressed()
    {
        audioMenuDown.Play();
        anim.Play("PressStart");
    }

    public void CreditsButtonPressed()
    {
        if (creditsVisible)
        {
            audioMenuUp.Play();
            GetNode<ColorRect>("Credits").Hide();
            // GetNode<ColorRect>("Credits").SetModulate(new Color(1,1,1,0));
            creditsVisible = false;
        }
        else
        {
            audioMenuDown.Play();
            GetNode<ColorRect>("Credits").Show();
            // GetNode<ColorRect>("Credits").SetModulate(new Color(1,1,1,1));
            creditsVisible = true;
        }
    }

    public void AnimationFinished(String which)
    {
        // GD.Print("animation finished.");
        if (which == "PressStart")
        {
            // EmitSignal("IntroFinished");
            this.Hide();
            audioMainMenu.Stop();
            audioLevel.Play();
        }
    }

}
