using Godot;
using System;
using System.Collections.Generic;

public partial class ShopModel : UIModel
{
    public override void Enter()
    {
        ShopView view = this.views["shop"] as ShopView;
        view.SetData();
        view.ShowView();
        
        UIManager.instance.vfxModel.OpenCurtain();
    }
    
    public override void Exit()
    {
        foreach (KeyValuePair<string, UIView> view in this.views)
        {
            view.Value.Exit();
        }
    }
}
