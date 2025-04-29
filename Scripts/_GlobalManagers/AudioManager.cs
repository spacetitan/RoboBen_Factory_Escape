using Godot;

[GlobalClass]
public partial class AudioManager : Node
{
	public static AudioManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}

	public AudioPlayer musicPlayer { get; private set; } = null;
	public AudioPlayer sfxPlayer { get; private set; } = null;
	
	public override void _Ready()
	{
		init();
		GetSceneNodes();
	}

	public void GetSceneNodes()
	{
		this.musicPlayer = GetNode<AudioPlayer>("%MusicPlayer");
		this.sfxPlayer = GetNode<AudioPlayer>("%SFXPlayer");
	}
}
