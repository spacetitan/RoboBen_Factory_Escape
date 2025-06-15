using Godot;
using System;

[GlobalClass]
public partial class UIView : Control
{
    [Export] public UIModel.ViewID ID = UIModel.ViewID.NONE;
    protected UIModel owner = null;

    public virtual void InitializeView(UIModel owner)
    {
        this.owner = owner;
        HideView();
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public void ShowView()
    {
        Tween tween = UIManager.instance.tween;
        if(tween != null)
        {
            tween.Kill();
        }

        tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Linear);
        tween.TweenCallback(Callable.From(this.Show));
        tween.TweenProperty(this, "modulate", Colors.White, .15f);
        tween.TweenCallback(Callable.From(this.Enter));
    }

    public void ShowView(float time)
    {
        Tween tween = UIManager.instance.tween;
        if(tween != null)
        {
            tween.Kill();
        }

        tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Linear);
        tween.TweenCallback(Callable.From(this.Show));
        tween.TweenProperty(this, "modulate", Colors.White, time);
        tween.TweenCallback(Callable.From(this.Enter));
    }

    public void HideView()
    {
        Tween tween = UIManager.instance.tween;
        if(tween != null)
        {
            tween.Kill();
        }

        tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenProperty(this, "modulate", Colors.Transparent, .15f);
        tween.TweenCallback(Callable.From(this.Exit));
        tween.TweenCallback(Callable.From(this.Hide));
    }

    public void HideView(float time, Action callback = null)
    {
        Tween tween = UIManager.instance.tween;
        if(tween != null)
        {
            tween.Kill();
        }

        tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenProperty(this, "modulate", Colors.Transparent, time);
        tween.TweenCallback(Callable.From(this.Exit));
        tween.TweenCallback(Callable.From(this.Hide));
        tween.Finished += () =>
        {
            callback?.Invoke();
        };
    }
}
