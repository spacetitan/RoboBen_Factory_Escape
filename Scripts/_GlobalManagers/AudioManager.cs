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

	public void SetBackgroundMusic(UIManager.UIState state)
	{
		switch (state)
		{
			case UIManager.UIState.START:
				this.musicPlayer.Play(ResourceManager.instance.music[ResourceManager.MusicID.START], true);
				break;
			
			case UIManager.UIState.RUN:
				this.musicPlayer.Play(ResourceManager.instance.music[ResourceManager.MusicID.RUN], true);
				break;
			
			case UIManager.UIState.BATTLE:
				BattleModel battleModel = UIManager.instance.models[UIManager.UIState.BATTLE] as BattleModel;
				if (battleModel.battleData.tier == 2)
				{
					this.musicPlayer.Play(ResourceManager.instance.music[ResourceManager.MusicID.BOSS], true);
				}
				else
				{
					this.musicPlayer.Play(ResourceManager.instance.music[ResourceManager.MusicID.BATTLE], true);	
				}
				break;
			
			case UIManager.UIState.SHOP:
			case UIManager.UIState.REST:
				this.musicPlayer.Play(ResourceManager.instance.music[ResourceManager.MusicID.SHOP], true);
				break;
			
			case UIManager.UIState.EVENT:
			case UIManager.UIState.TREASURE:
				this.musicPlayer.Play(ResourceManager.instance.music[ResourceManager.MusicID.EVENT], true);
				break;
			
			case UIManager.UIState.GAMEOVER:
				GameOverModel gameOverModel = UIManager.instance.models[UIManager.UIState.GAMEOVER] as GameOverModel;
				if (gameOverModel.win)
				{
					this.musicPlayer.Play(ResourceManager.instance.music[ResourceManager.MusicID.WIN], true);
				}
				else
				{
					this.musicPlayer.Stop();
				}
				break;
		}
	}
}
