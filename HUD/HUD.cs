using Godot;
using System;
using System.Collections.Generic;

public class HUD : CanvasLayer {
  // [Signal]
  // public delegate void UpdateHud();

  private Timer typewriter_tm;
  private Timer message_tm;
  private Queue<int> display_q = new Queue<int>();
  private String cur_msg;
  private int cur_msg_char;
  private ColorRect partList;
  private ColorRect sLog_bg;
  private Label sLog;
  private ProgressBar battery;
  private ProgressBar heat;
  private AnimationPlayer anim;
  private bool isTyping = false;
  private float gameTimer = 0;
  
  private ScrollContainer scr;
  private VScrollBar v;
  private double maxScroll;

  public override void _Ready() {
    // set up references
    typewriter_tm = GetNode<Timer>("TypewriterTimer");
    message_tm = GetNode<Timer>("MessageDelay");
    partList = GetNode<ColorRect>("PartList");
    sLog_bg = GetNode<ColorRect>("StatusLog_bg");
    sLog = sLog_bg.GetNode<Label>("Scroll/StatusLog");
    battery = GetNode<ProgressBar>("BatteryLevel");
    heat = GetNode<ProgressBar>("HeatLevel");
    anim = GetNode<AnimationPlayer>("AnimationPlayer");

    //clear editor data / hide displays to start.
    ClearLog();
    foreach (Label part in partList.GetChildren()) {
      part.Visible = false;
    }
    partList.Visible = false;
    battery.Visible = false;
    heat.Visible = false;
    
    //set up scrollbar references for forced scrolling
    scr = sLog_bg.GetNode<ScrollContainer>("Scroll");
    v = scr.GetVScrollbar();
    v.Connect("changed", this, "HandleScroll");
    maxScroll = v.MaxValue;
  }

  public override void _Process(float delta) {
    gameTimer += delta;
  }

  public void TestButton() {
    GD.Print("Testbutton pressed.");
    DisplayLog(1);
  }

  public void ClearLog() {
    sLog_bg.Visible = false;
    sLog.Text = "";
  }

  public void DisplayLog(int id) {
    // grab a text entry by id, start displaying it based on typewriter_tm
    // once complete, fire off message_tm
    display_q.Enqueue(id);
    if (message_tm.TimeLeft == 0) {
      //no message currently playing. Start playing.
      isTyping = true;
      message_tm.Start();
    }
  }

  public void NextLog() {
    // called by message_tm
    // GD.Print("NextLog called.");
    if (display_q.Count > 0 && typewriter_tm.TimeLeft == 0) {
      cur_msg = GetLog(display_q.Dequeue());
      cur_msg_char = 0;
      sLog.Text += "\n"+ GetTimestamp();
      message_tm.Stop();
      typewriter_tm.Start();
    }
  }

  public String GetTimestamp(bool fake = false) {
    if (fake) {
      return "XX:XX";
    } else {
      int minutes = (int)(gameTimer/60);
      int seconds = (int)(gameTimer%60);
      return minutes.ToString().PadLeft(2,'0') + ":" + seconds.ToString().PadLeft(2,'0') + " - ";
    }
  }

  public String GetLog(int id) {
    //given an id, return the appropriate string.
    return "The only thing we have to fear is fear itself...";
  }

  public void NextChar() {
    // called by typewriter_tm
    // display next character in string
    // if there is no next character, no longer typing, start message_tm
    sLog.Text += cur_msg[cur_msg_char];
    cur_msg_char++;
    if(cur_msg_char >= cur_msg.Length) {
      typewriter_tm.Stop();
      if (display_q.Count > 0) {
        message_tm.Start();
      }
    }
  }

  public void UpdateHeat(float val) {
    if (!heat.Visible) {
      heat.Visible = true;
    }
    heat.Value = val;
  }

  public void UpdateBattery(float val) {
    if (!battery.Visible) {
      battery.Visible = true;
    }
    battery.Value = val;
  }

  public void GetPart(String name) {
    if (!partList.Visible) {
      partList.Visible = true;
    }
    Label which = partList.GetNode<Label>(name);
    if(which != null) {
      which.Visible = true;
    }
  }

  public void Fadeout() {
    anim.Play("Fadeout");
  }

  public void Fadein() {
    anim.PlayBackwards("Fadeout");
  }

  public void HandleScroll() {
    if (maxScroll != v.MaxValue) {
      maxScroll = v.MaxValue;
      scr.ScrollVertical = (int)v.MaxValue;
    }
  }

}
