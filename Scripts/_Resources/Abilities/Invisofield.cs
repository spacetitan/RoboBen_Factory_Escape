using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Invisofield : Ability
{
    public override void ApplyEffects(List<Character> targets, PlayerData playerStats)
    {
        Invis invis = ResourceManager.instance.statuses[Status.StatusID.INVIS].CreateInstance() as Invis;
        invis.SetDuration(this.value);

        BattleModel model = UIManager.instance.models[UIManager.UIState.BATTLE] as BattleModel;
        StatusEffect statusEffect = new StatusEffect(invis, invis.sfx);
        statusEffect.sender = model.player;
        statusEffect.Execute(targets);
        
        AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.INVIS_ON]);
        base.ApplyEffects(targets, playerStats);
    }
}
