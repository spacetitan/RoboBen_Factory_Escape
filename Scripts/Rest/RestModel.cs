using Godot;
using System;
using System.Collections.Generic;

public partial class RestModel : UIModel
{

    public override void Enter()
    {
        RestView view = this.views[ViewID.REST] as RestView;
        view.SetData();
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
