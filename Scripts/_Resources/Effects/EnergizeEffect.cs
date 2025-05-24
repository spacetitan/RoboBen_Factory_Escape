using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class EnergizeEffect : Effect
{
	private int amount = 1;
	public EnergizeEffect(int amount, AudioStream audio)
	{
		this.amount = amount;
		this.sfx = audio;
	}

	public override void Execute(Character target) 
	{
		if(target == null) { return; }

		if(target is Player)
        {
            Player player = target as Player;
            player.AddEnergy(this.amount);
        }

		if(this.sfx != null) { AudioManager.instance.sfxPlayer.Play(this.sfx); }
	}

	public override void Execute(List<Character> targets) 
	{
		if(targets == null) { return; }

		foreach (Node target in targets)
		{
			if(target == null) { continue; }

			if(target is Player)
	        {
	            Player player = target as Player;
	            player.AddEnergy(this.amount);
	        }
			
			if(this.sfx != null) { AudioManager.instance.sfxPlayer.Play(this.sfx); }
		}
	}
}
