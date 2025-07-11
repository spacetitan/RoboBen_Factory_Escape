using Godot;
using System;

public partial class StartModel : UIModel
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
    }

    public override void Enter()
    {
        if (GameManager.instance.HasLoadFile())
        {
            GameManager.instance.LoadGame();
        }
        
        UIManager.instance.vfxModel.OpenCurtain();
        AudioManager.instance.SetBackgroundMusic(UIManager.UIState.START);
    }

    public override void Exit()
    {
        this.views[UIModel.ViewID.PLAY].HideView();
    }
}
