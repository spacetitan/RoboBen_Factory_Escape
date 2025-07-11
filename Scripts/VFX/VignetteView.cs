using Godot;
using System;

public partial class VignetteView : UIView
{
    private Timer timer = null;

    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.timer = this.GetNode<Timer>("%Timer");
        this.timer.Timeout += OnTimerTimeout;
    }

    public void OnPlayerHit()
    {
        this.ShowView(0.15f);
        timer.Start();
    }

    public void OnTimerTimeout()
    {
        this.HideView(0.15f);
    }
}
