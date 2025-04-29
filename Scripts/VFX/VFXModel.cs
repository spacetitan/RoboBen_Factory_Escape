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
                this.views.Add(view.viewID, view);
            }
        }
        
        GetSceneNodes();
    }
    
    private void GetSceneNodes()
    {
        
    }

    public void OpenCurtain()
    {
        this.ShowModel();
        CurtainView view = this.views["curtain"] as CurtainView;
        view.ShowView();
    }

    public void CloseCurtain(Action callback = null)
    {
        this.ShowModel();
        CurtainView view = this.views["curtain"] as CurtainView;
        view.CloseCurtain(callback);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
