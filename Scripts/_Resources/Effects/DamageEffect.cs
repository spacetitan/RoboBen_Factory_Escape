using Godot;
using System;
using System.Collections.Generic;

public partial class DamageEffect : Effect
{
    public int amount = 0;
    public Modifier.Type receivermodifierType = Modifier.Type.DMG_TAKEN;
    private bool isDirect = false;

    public DamageEffect(int value, AudioStream sfx, bool isDirect = false)
    {
        this.amount = value;
        this.sfx = sfx;
        this.isDirect = isDirect;
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
            if (enemy.statusHandler.HasStatus(Status.StatusID.INVIS))
            {
                this.sfx = ResourceManager.instance.audio[ResourceManager.AudioID.MISS];
            }
            else if (isDirect)
            {
                enemy.TakeDirectDamage(amount, receivermodifierType);
            }
            else
            {
                enemy.TakeDamage(amount, receivermodifierType);
            }
        }
        else if(target is Player)
        {
            Player player = target as Player;
            
            if (player.statusHandler.HasStatus(Status.StatusID.INVIS))
            {
                this.sfx = ResourceManager.instance.audio[ResourceManager.AudioID.MISS];
            }
            else if (isDirect)
            {
                player.TakeDirectDamage(amount, receivermodifierType);
            }
            else
            {
                player.TakeDamage(amount, receivermodifierType);
            }
        }
		
        if(this.sfx != null) { AudioManager.instance.sfxPlayer.Play(this.sfx); }
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
                if (enemy.statusHandler.HasStatus(Status.StatusID.INVIS))
                {
                    this.sfx = ResourceManager.instance.audio[ResourceManager.AudioID.MISS];
                }
                else if (isDirect)
                {
                    enemy.TakeDirectDamage(amount, receivermodifierType);
                }
                else
                {
                    enemy.TakeDamage(amount, receivermodifierType);
                }
            }
            else if(target is Player)
            {
                Player player = target as Player;
                if (player.statusHandler.HasStatus(Status.StatusID.INVIS))
                {
                    this.sfx = ResourceManager.instance.audio[ResourceManager.AudioID.MISS];
                }
                else if (isDirect)
                {
                    player.TakeDirectDamage(amount, receivermodifierType);
                }
                else
                {
                    player.TakeDamage(amount, receivermodifierType);
                }
            }
            AudioManager.instance.sfxPlayer.Play(this.sfx);
        }
    }
}
