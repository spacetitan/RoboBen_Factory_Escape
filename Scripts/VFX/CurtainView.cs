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
        this.ShowView(.6f);
    }

    public override void Enter()
    {
        if (this.onClosed != null)
        {
            this.onClosed();
        }
    }

    public override void Exit()
    {
        this.onClosed = null;
    }
}
