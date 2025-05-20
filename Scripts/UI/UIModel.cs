using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class UIModel : Control
{
	public enum ViewID { NONE, RUN_HUD, ROOM_HUD, BATTLE_HUD, HAND, TITLE, PLAY, BATTLE, TREASURE, REST, SHOP, POP_UP, OPTIONS, DECK, WIN, REWARD, MAP, COLLECTIONS, CURTAIN, VIGNETTE}
    [Export] public UIManager.UIState state = UIManager.UIState.NONE;
    public Dictionary<ViewID, UIView> views { get; private set; } = new Dictionary<ViewID, UIView>();
    
    public virtual void InitializeModel()
    {
        foreach (Control control in this.GetChildren())
        {
            if (control is UIView)
            {
                UIView view = control as UIView;
                view.InitializeView(this);
                this.views.Add(view.ID, view);
            }
        }
        HideModel();
    }

    public virtual void Enter()
    {
    }

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
