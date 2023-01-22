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
  public TextureProgress battery;
  public TextureProgress heat;
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
    battery = GetNode<TextureProgress>("BatteryTexture");
    heat = GetNode<TextureProgress>("HeatTexture");
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

    //Fadein();
    GameStartText(); //FIXME: run this whenever the game *actually* starts.
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
    if (!sLog_bg.Visible) {
      sLog_bg.Visible = true;
    }
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

  private String[] allLogs = {
    "System Activation.",
    "Critical system damage detected.",
    "Auto-repair functionality enabled [n] cycles ago.",
    "Chronometer malfunction.",
    "Attempting to establish connection with main server...",
    "Main server unresponsive.",
    "Attempting to establish connection with Fleet Command...",
    "Fleet Command unresponsive.",
    "External chronometers unreachable.",
    "Writing to log based on local unit chassis timer.",
    "Core system restored.",
    "Motor restored.",
    "Error. Gear direction change disabled.",
    "Error. Locomotion systems unable to turn axles.",
    "Self-repair functionality active.",
    "Salvage functionality active.",
    "Prime Directive: Repair GSC Hoplite.",
    "Secondary Directive: Restore unit functionality in order to enable repairs.",
    "----",//18
    "Unable to interact.",//19
    "Door opened.",//20
    "Salvage secured.",//21
    "Manipulator Arm acquired.",//22
    "Solar Panels acquired.",//23
    "Proximity Sensor acquired.",//24
    "Enhanced battery acquired.",//25
    "Temperature Sensor acquired.",//26
    "Cutting arm acquired.",//27
    "Audio Sensor acquired. Please begin calibrations."//28
  };

  public String GetLog(int id) {
    //given an id, return the appropriate string.
    String log = allLogs[id];
    
    if (log != null) {
      return log;
    } else {
      return "Unknown command";
    }
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

  public void GameStartText() {
    //when game is started, add initial data logs to queue. Might be nice to time other things together, but not for v1.
    DisplayLog(0);
    DisplayLog(1);
    DisplayLog(2);
    DisplayLog(3);
    DisplayLog(4);
    DisplayLog(5);
    DisplayLog(6);
    DisplayLog(7);
    DisplayLog(8);
    DisplayLog(9);
    DisplayLog(10);
    DisplayLog(11);
    DisplayLog(12);
    DisplayLog(13);
    DisplayLog(14);
    DisplayLog(15);
    DisplayLog(16);
    DisplayLog(17);
    DisplayLog(18);
  }

}
