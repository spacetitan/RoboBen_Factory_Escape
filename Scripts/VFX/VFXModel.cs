using Godot;
using System;

public partial class VFXModel : UIModel
{
    public override void InitializeModel()
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
        
        GetSceneNodes();
        EventManager.instance.PlayerDamaged += OnPlayerHit;
    }
    
    private void GetSceneNodes()
    {
        
    }

    public void OpenCurtain(Action callback = null, float val = .85f)
    {
        CurtainView view = this.views[ViewID.CURTAIN] as CurtainView;
        view.HideView(val, callback);
    }

    public void CloseCurtain(Action callback = null)
    {
        CurtainView view = this.views[ViewID.CURTAIN] as CurtainView;
        view.CloseCurtain(callback);
    }

    public void OnPlayerHit()
    {
        VignetteView view = this.views[ViewID.VIGNETTE] as VignetteView;
        view.OnPlayerHit();
    }

    public void Shake(Node2D thing, float strength, float duration = 0.2f)
    {
        if(thing == null){return;}

        Vector2 origPos = thing.Position;
        int shakeCount = 10;
        Tween tween = CreateTween();

        for(int i = 0; i < 10; i++)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            rng.Randomize();
            Vector2 shakeOffset = new Vector2(rng.RandfRange(-1.0f, 1.0f), rng.RandfRange(-1.0f, 1.0f));
            Vector2 target = origPos + strength * shakeOffset;

            if(i % 2 == 0)
            {
                target = origPos;
            }

            if(thing != null) 
            {
                tween.TweenProperty(thing, "position", target, duration/shakeCount);
            }
            strength *= 0.75f;
        }

        tween.Finished += () => 
        { 
            thing.Position = origPos; // causes error when player dies before finished
        };
    }
	
    public void Shake(Control thing, float strength, float duration = 0.2f)
    {
        if(thing == null){return;}

        Vector2 origPos = thing.Position;
        int shakeCount = 10;
        Tween tween = CreateTween();

        for(int i = 0; i < 10; i++)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            rng.Randomize();
            Vector2 shakeOffset = new Vector2(rng.RandfRange(-1.0f, 1.0f), rng.RandfRange(-1.0f, 1.0f));
            Vector2 target = origPos + strength * shakeOffset;

            if(i % 2 == 0)
            {
                target = origPos;
            }

            if(thing != null) 
            {
                tween.TweenProperty(thing, "position", target, duration/shakeCount);
            }
            strength *= 0.75f;
        }

        tween.Finished += () => 
        { 
            thing.Position = origPos; // causes error when player dies before finished
        };
    }

    public override void Exit()
    {
        base.Exit();
    }
}
