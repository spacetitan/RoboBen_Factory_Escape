using System;
using System.IO;
using Godot;
using Godot.Collections;

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
	private string settingsPath = "res://settings.cfg";
	
	private string savePath = Path.Join(ProjectSettings.GlobalizePath("user://") + "RoboBen/");
	private string loadPath = Path.Join(ProjectSettings.GlobalizePath("user://")+ "RoboBen/SaveData.json");
	public Dictionary loadData { get; private set; } = null;

	public override void _Ready()
	{
		init();
		InitializeSettings();
	}
	
	public void InitializeSettings()
	{
		Error error = settings.Load(this.settingsPath);

		if (error != Error.Ok)
		{
			CreateNewSettingsFile();
			LoadSettings();
		}
		else
		{
			LoadSettings();
		}

		if (HasLoadFile())
		{
			LoadGame();
		}
	}

	public void CreateNewSettingsFile()
	{
		settings.SetValue("Settings", "music", .8f);
		settings.SetValue("Settings", "sfx", .8f);

		settings.Save(this.settingsPath);
	}

	public void SaveSettings()
	{
		settings.SetValue("Settings", "music", AudioManager.instance.musicPlayer.volume);
		settings.SetValue("Settings", "sfx", AudioManager.instance.sfxPlayer.volume);

		settings.Save(this.settingsPath);
	}

	public void LoadSettings()
	{
		AudioManager.instance.musicPlayer.SetVolume((double)settings.GetValue("Settings", "music")); 
		AudioManager.instance.sfxPlayer.SetVolume((double)settings.GetValue("Settings", "sfx")); 
	}

	public void SaveGame()
	{
		string json = Json.Stringify(RunManager.instance.currentRun.saveRun(), "", false, true);
		GD.Print("saving to: " + this.savePath);

		if (!Directory.Exists(savePath))
		{
			Directory.CreateDirectory(savePath);
		}

		try
		{
			File.WriteAllText(this.loadPath, json);
		}
		catch (Exception e)
		{
			GD.Print(e);
			throw;
		}
	}

	public bool HasLoadFile()
	{
		if (!Directory.Exists(savePath) || !Directory.Exists(loadPath))
		{
			return false;
		}
		
		string data = File.ReadAllText(this.loadPath);
		
		Json loadGame = new Json();
		Error err = loadGame.Parse(data);

		if (err != Error.Ok)
		{
			GD.Print(err);
			return false;
		}
		return true;
	}

	public void LoadGame()
	{
		string data = File.ReadAllText(this.loadPath);
		
		Json loadGame = new Json();
		Error err = loadGame.Parse(data);

		if (err != Error.Ok)
		{
			GD.Print(err);
			return;
		}

		this.loadData = new Dictionary();
		this.loadData = (Dictionary)loadGame.Data;
	}

	public Dictionary GetLoadData(string key = null)
	{
		if (key != null)
		{
			return (Dictionary) this.loadData[key];
		}

		return this.loadData;
	}

	public void DeleteSaveData()
	{
		if (!Directory.Exists(savePath))
		{
			return;
		}

		DirAccess.RemoveAbsolute(loadPath);
	}
}
