using Godot;
using System;
using System.Collections.Generic;

public partial class HUDModel : UIModel
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
        //HideModel();
    }
    
    public void ChangeHUDTo(UIManager.UIState state)
    {
        switch (state)
        {
            case UIManager.UIState.RUN:
                this.views["roomHUD"].HideView();
                //this.views["battleHUD"].HideView();
                
                this.views["runHUD"].ShowView();
                break;
            
            case UIManager.UIState.BATTLE:
            default:
                break;
        }
    }
}
