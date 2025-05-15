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
                this.views["battleHUD"].HideView();
                this.views["hand"].HideView();
                
                this.views["runHUD"].ShowView();
                break;
            
            case UIManager.UIState.BATTLE:
                this.views["roomHUD"].HideView();
                this.views["runHUD"].HideView();
                
                this.views["hand"].ShowView();
                this.views["battleHUD"].ShowView();
                break;
            
            case UIManager.UIState.TREASURE:
            case UIManager.UIState.REST:
            case UIManager.UIState.SHOP:
                this.views["runHUD"].HideView();
                
                this.views["roomHUD"].ShowView();
                break;
                
            default:
                break;
        }
    }

    public void SetPlayerData(Player player)
    {
        BattleHUDView hud = this.views["battleHUD"] as BattleHUDView;
        hud.SetData(player);
        
        HandView hand = this.views["hand"] as HandView;
        hand.SetData(player);
    }
}
