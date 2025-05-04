using Godot;
using System;

public partial class CurtainView : UIView
{
    private Action onClosed = null;
    
    public override void InitializeView(UIModel owner)
    {
        this.owner = owner;
    }
    
    public void CloseCurtain(Action callback = null)
    {
        this.onClosed = callback;
        this.ShowView(1f);
    }

    public override void Enter()
    {
        if (this.onClosed != null)
        {
            this.onClosed();
        }
        
        this.HideView(1f);
    }

    public override void Exit()
    {
        this.onClosed = null;
    }
}
