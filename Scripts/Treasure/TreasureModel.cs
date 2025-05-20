using Godot;
using System;
using System.Collections.Generic;

public partial class TreasureModel : UIModel
{
    public override void Enter()
    {
        TreasureView view = this.views[ViewID.TREASURE] as TreasureView;
        view.ShowView();
        
        UIManager.instance.vfxModel.OpenCurtain();
    }
    
    public override void Exit()
    {
        foreach (KeyValuePair<ViewID, UIView> view in this.views)
        {
            view.Value.Exit();
        }
    }
}
