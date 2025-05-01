using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class HealEffect : Effect
{
    public int amount = 0;
    public Modifier.Type receivermodifierType = Modifier.Type.HEALED;

    public HealEffect(int value, AudioStream sfx)
    {
        this.amount = value;
        this.sfx = sfx;
    }

    public override void Execute(Character target)
    {
        if(target == null)
        {
            return;
        }

        if(target is Enemy)
        {
            // Enemy enemy = (Enemy)target;
            // enemy.Heal(amount);
        }
        else if(target is Player)
        {
            Player player = target as Player;
            player.Heal(amount);
        }
        AudioManager.instance.sfxPlayer.Play(this.sfx);
    }

    public override void Execute(List<Character> targets)
    {
        foreach (Node target in targets)
        {
            if(target == null)
            {
                continue;
            }

            if(target is Enemy)
            {
                // Enemy enemy = (Enemy)target;
                // enemy.Heal(amount);
            }
            else if(target is Player)
            {
                Player player = target as Player;
                player.Heal(amount);
            }
            AudioManager.instance.sfxPlayer.Play(this.sfx);
        }
    }
}