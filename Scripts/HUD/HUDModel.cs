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
                this.views.Add(view.ID, view);
            }
        }
        //HideModel();
    }
    
    public void ChangeHUDTo(UIManager.UIState state)
    {
        switch (state)
        {
            case UIManager.UIState.RUN:
                this.views[ViewID.ROOM_HUD].HideView();
                this.views[ViewID.BATTLE_HUD].HideView();
                this.views[ViewID.HAND].HideView();
                
                this.views[ViewID.RUN_HUD].ShowView();
                break;
            
            case UIManager.UIState.BATTLE:
                this.views[ViewID.RUN_HUD].HideView();
                
                this.views[ViewID.HAND].ShowView();
                this.views[ViewID.BATTLE_HUD].ShowView();
                break;
            
            case UIManager.UIState.TREASURE:
            case UIManager.UIState.REST:
            case UIManager.UIState.SHOP:
            case UIManager.UIState.EVENT:
                this.views[ViewID.RUN_HUD].HideView();
                
                this.views[ViewID.ROOM_HUD].ShowView();
                break;
            
            case UIManager.UIState.START:
            case UIManager.UIState.GAMEOVER:
                this.views[ViewID.ROOM_HUD].HideView();
                this.views[ViewID.BATTLE_HUD].HideView();
                this.views[ViewID.HAND].HideView();
                this.views[ViewID.RUN_HUD].HideView();
                break;
                
            default:
                break;
        }
    }

    public void SetPlayerData(Player player)
    {
        BattleHUDView hud = this.views[ViewID.BATTLE_HUD] as BattleHUDView;
        hud.SetData(player);
        
        HandView hand = this.views[ViewID.HAND] as HandView;
        hand.SetData(player);
    }
}
