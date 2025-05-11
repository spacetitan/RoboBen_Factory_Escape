using Godot;
using System.Collections.Generic;

public partial class GuardEffect : Effect
{
    private int amount = 0;
    public GuardEffect(int value, AudioStream sfx)
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
            Enemy enemy = (Enemy)target;
            enemy.AddArmor(amount);
        }
        else if(target is Player)
        {
            Player player = target as Player;
            player.AddArmor(amount);
        }
        AudioManager.instance.sfxPlayer.Play(this.sfx);
    }

    public override void Execute(List<Character> targets)
    {
        foreach (Character target in targets)
        {
            if(target == null)
            {
                continue;
            }

            if(target is Enemy)
            {
                Enemy enemy = (Enemy)target;
                enemy.AddArmor(amount);
            }
            else if(target is Player)
            {
                Player player = target as Player;
                player.AddArmor(amount);
            }
            AudioManager.instance.sfxPlayer.Play(this.sfx);
        }
    }
}
