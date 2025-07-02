using Godot;
using System;

public partial class AudioPlayer : Node
{
    [Export] public double volume = 0.75f;
    
    public void Play(AudioStream audio, bool single = false)
    {
        if(audio == null) {return;}

        if(single) { Stop();}

        foreach(AudioStreamPlayer player in GetChildren())
        {
            if(!player.Playing)
            {
                player.Stream = audio;
                player.Play();
                break;
            }
        }
    }

    public void Stop()
    {
        foreach(AudioStreamPlayer player in GetChildren())
        {
            player.Stop();
            
        }
    }

    public void SetVolume(double level)
    {
        this.volume = level;

        if (this.volume < 0.0f)
        {
            this.volume = 0.0f;
        }

        if (this.volume > 1.0f)
        {
            this.volume = 1.0f;
        }

        //-23 <-> -1
        double decibals = (this.volume * 40) - 45;
        //GD.Print("Setting audio levels: " + decibals);
        foreach(AudioStreamPlayer player in GetChildren())
        {
            player.SetVolumeDb((float)decibals);
        }
    }
}
