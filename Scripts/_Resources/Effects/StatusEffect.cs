using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class StatusEffect : Effect
{
	public Status status;
	public StatusEffect(Status status, AudioStream audio)
	{
		this.status = status;
		this.sfx = audio;
	}

	public override void Execute(Character target) 
	{
		if(target == null) { return; }

		if(target is Enemy)
		{
		    Enemy enemy = target as Enemy;
		    enemy.statusHandler.AddStatus(status);
		}
		else if(target is Player)
		{
		    Player player = target as Player;
		    player.statusHandler.AddStatus(status);
		}

		if(this.sfx != null) { AudioManager.instance.sfxPlayer.Play(this.sfx); }
	}

	public override void Execute(List<Character> targets) 
	{
		if(targets == null) { return; }

		foreach (Character target in targets)
		{
			if(target == null) { continue; }

			if(target is Enemy)
			{
			    Enemy enemy = target as Enemy;
			    enemy.statusHandler.AddStatus(status);
			}
			else if(target is Player)
			{
			    Player player = target as Player;
			    player.statusHandler.AddStatus(status);
			}
			
			if(this.sfx != null) { AudioManager.instance.sfxPlayer.Play(this.sfx); }
		}
	}
}
