using Godot;
using System;

public class MainMenu : CanvasLayer {

  // [Signal]
  // public delegate void IntroFinished();

  private AnimationPlayer anim;
  private bool creditsVisible = false;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()  {
    anim = GetNode<AnimationPlayer>("MainAnimator");
    anim.Play("Intro");
    anim.Queue("WaitingLoop");
  }

  public void StartButtonPressed() {
    anim.Play("PressStart");
  }

  public void CreditsButtonPressed() {
    if (creditsVisible) {
      GetNode<ColorRect>("Credits").Hide();
      // GetNode<ColorRect>("Credits").SetModulate(new Color(1,1,1,0));
      creditsVisible = false;
    } else {
      GetNode<ColorRect>("Credits").Show();
      // GetNode<ColorRect>("Credits").SetModulate(new Color(1,1,1,1));
      creditsVisible = true;
    }
  }

  public void AnimationFinished(String which) {
    // GD.Print("animation finished.");
    if (which == "PressStart") {
      // EmitSignal("IntroFinished");
      this.Hide();
    }
  }

}
