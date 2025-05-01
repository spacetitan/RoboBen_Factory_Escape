using Godot;
using System;

[GlobalClass]
public partial class SecondWind : PowerUp
{
    private bool ready = false;
    public override void InitializePowerUp(PowerUpHandler owner)
    {
        EventManager.instance.PlayerHealed -= ActivatePowerUp;
        EventManager.instance.PlayerHealed += ActivatePowerUp;
        
        EventManager.instance.PlayerTurnStarted -= ReadyAbility;
        EventManager.instance.PlayerTurnStarted += ReadyAbility;

        base.InitializePowerUp(owner);
    }

    public override void ActivatePowerUp()
    {
        if(this.ready && UIManager.instance.currentModel.state == UIManager.UIState.BATTLE)
        {
            Player player = this.owner.GetTree().GetFirstNodeInGroup("Player") as Player;
            player.DrawCard();
        
            base.ActivatePowerUp();
            this.ready = false;
        }
    }

    public void ReadyAbility()
    {
        this.ready = true;
    }
}