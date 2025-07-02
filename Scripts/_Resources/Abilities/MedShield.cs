using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class MedShield : Ability
{
    public override void ApplyEffects(List<Character> targets, PlayerData playerStats)
    {
        BattleModel model = UIManager.instance.models[UIManager.UIState.BATTLE] as BattleModel;

        int num = 0;
        foreach (Status status in model.player.statusHandler.statuses)
        {
            if (status.stacks > 0)
            {
                num += status.stacks;
            }
            else
            {
                num += status.duration;
            }
        }
        
        GuardEffect guard = new GuardEffect(this.value + num, this.playSFX);
        guard.Execute(targets);
        
        base.ApplyEffects(targets, playerStats);
    }
}
