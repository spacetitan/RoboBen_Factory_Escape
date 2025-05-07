using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class UIModel : Control
{
    [Export] public UIManager.UIState state = UIManager.UIState.NONE;
    public Dictionary<string, UIView> views { get; private set; } = new Dictionary<string, UIView>();
    
    public virtual void InitializeModel()
    {
        foreach (Control control in this.GetChildren())
        {
            if (control is UIView)
            {
                UIView view = control as UIView;
                view.InitializeView(this);
                this.views.Add(view.viewID, view);
            }
        }
        HideModel();
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public void ShowModel()
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

    public void ShowModel(float time)
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

    public void HideModel()
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

    public void HideModel(float time)
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
    }
}
