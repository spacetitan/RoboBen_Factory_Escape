using Godot;

[GlobalClass]
public partial class GameManager : Node
{
	public static GameManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}
	
	private ConfigFile settings = new ConfigFile();

	public override void _Ready()
	{
		init();
		InitializeSettings();
	}
	
	public void InitializeSettings()
	{
		Error error = settings.Load("res://settings.cfg");

		if (error != Error.Ok)
		{
			CreateNewSettingsFile();
			LoadSettings();
		}
		else
		{
			LoadSettings();
		}
	}

	public void CreateNewSettingsFile()
	{
		settings.SetValue("Settings", "music", .8f);
		settings.SetValue("Settings", "sfx", .8f);

		settings.Save("res://settings.cfg");
	}

	public void SaveSettings()
	{
		settings.SetValue("Settings", "music", AudioManager.instance.musicPlayer.volume);
		settings.SetValue("Settings", "sfx", AudioManager.instance.sfxPlayer.volume);

		settings.Save("res://settings.cfg");
	}

	public void LoadSettings()
	{
		AudioManager.instance.musicPlayer.SetVolume((double)settings.GetValue("Settings", "music")); 
		AudioManager.instance.sfxPlayer.SetVolume((double)settings.GetValue("Settings", "sfx")); 
	}
}
