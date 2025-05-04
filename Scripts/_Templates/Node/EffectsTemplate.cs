// meta-name: Effects
// meta-description: For effects that alter stats

using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class _CLASS_ : Effect
{
    private int amount = 1;
    public _CLASS_(int amount, AudioStream audio)
    {
        this.amount = amount;
        this.sfx = audio;
    }

    public override void Execute(Character target) 
    {
        if(target == null) { return; }

        //put effect here

        if(this.sfx != null) { AudioManager.instance.sfxPlayer.Play(this.sfx); }
    }

    public override void Execute(List<Character> targets) 
    {
        if(targets == null) { return; }

        foreach (Node target in targets)
        {
            if(target == null) { continue; }

            //put effect here
			
            if(this.sfx != null) { AudioManager.instance.sfxPlayer.Play(this.sfx); }
        }
    }
}