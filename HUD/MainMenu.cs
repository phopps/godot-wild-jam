using Godot;

public class MainMenu : CanvasLayer
{
    // [Signal] public delegate void IntroFinished();
    private AnimationPlayer anim;
    private bool creditsVisible = false;
    private AudioStreamPlayer audioMainMenu;
    private AudioStreamPlayer audioCredits;
    private AudioStreamPlayer audioLevel;
    private AudioStreamPlayer audioMenuUp;
    private AudioStreamPlayer audioMenuDown;
    private AudioStreamPlayer audioEngineIdle;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        audioMainMenu = GetNode<AudioStreamPlayer>("/root/Main/Audio/MainMenu");
        audioCredits = GetNode<AudioStreamPlayer>("/root/Main/Audio/Credits");
        audioLevel = GetNode<AudioStreamPlayer>("/root/Main/Audio/Level");
        audioMenuUp = GetNode<AudioStreamPlayer>("/root/Main/Audio/MenuUp");
        audioMenuDown = GetNode<AudioStreamPlayer>("/root/Main/Audio/MenuDown");
        audioEngineIdle = GetNode<AudioStreamPlayer>("/root/Main/Audio/EngineIdle");
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
            audioCredits.Stop();
            audioMainMenu.Play();
            GetNode<ColorRect>("Credits").Hide();
            // GetNode<ColorRect>("Credits").SetModulate(new Color(1,1,1,0));
            creditsVisible = false;
        }
        else
        {
            audioMenuDown.Play();
            audioMainMenu.Stop();
            audioCredits.Play();
            GetNode<ColorRect>("Credits").Show();
            // GetNode<ColorRect>("Credits").SetModulate(new Color(1,1,1,1));
            creditsVisible = true;
        }
    }

    public void AnimationFinished(string which)
    {
        // GD.Print("animation finished.");
        if (which == "PressStart")
        {
            // EmitSignal("IntroFinished");
            // this.Hide();
            Hide();
            audioMainMenu.Stop();
            audioLevel.Play();
            audioEngineIdle.Play();
        }
    }
}
