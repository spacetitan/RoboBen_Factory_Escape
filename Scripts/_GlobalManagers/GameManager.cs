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
	
	private string savePath = Path.Join(ProjectSettings.GlobalizePath("user://") + "Saves/");
	private string loadPath = Path.Join(ProjectSettings.GlobalizePath("user://")+ "Saves/SaveData.json");
	public Dictionary loadData { get; private set; } = null;
	
	public bool ftue { get; private set; } = true;
	public DisplayServer.WindowMode windowMode { get; private set; } = DisplayServer.WindowMode.Fullscreen;

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
	}

	public void CreateNewSettingsFile()
	{
		settings.SetValue("Settings", "FTUE", true);
		settings.SetValue("Settings", "music", .8f);
		settings.SetValue("Settings", "sfx", .8f);
		settings.SetValue("Settings", "windowMode", (int) DisplayServer.WindowMode.Fullscreen);

		settings.Save(this.settingsPath);
	}

	public void SaveSettings()
	{
		settings.SetValue("Settings", "FTUE", this.ftue);
		settings.SetValue("Settings", "music", AudioManager.instance.musicPlayer.volume);
		settings.SetValue("Settings", "sfx", AudioManager.instance.sfxPlayer.volume);
		settings.SetValue("Settings", "windowMode", (int) this.windowMode);

		settings.Save(this.settingsPath);
	}

	public void LoadSettings()
	{
		this.ftue = (bool)settings.GetValue("Settings", "FTUE");
		AudioManager.instance.musicPlayer.SetVolume((double)settings.GetValue("Settings", "music")); 
		AudioManager.instance.sfxPlayer.SetVolume((double)settings.GetValue("Settings", "sfx")); 
		int mode = (int) settings.GetValue("Settings", "windowMode");
		this.windowMode = (DisplayServer.WindowMode) mode;
		DisplayServer.WindowSetMode(this.windowMode);
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
		if (!File.Exists(loadPath))
		{
			GD.Print("no data present");
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
		if (this.loadData != null)
		{
			if (key != null)
			{
				return (Dictionary) this.loadData[key];
			}

			return this.loadData;
		}
		else
		{
			return null;
		}
	}

	public void DeleteSaveData()
	{
		if (!File.Exists(loadPath))
		{
			return;
		}

		DirAccess.RemoveAbsolute(loadPath);
		this.loadData = null;
	}

	public void SetWindowMode(long id)
	{
		switch (id)
		{
			case 0:
				this.windowMode = DisplayServer.WindowMode.Fullscreen;
				break;
            
			case 1:
				this.windowMode = DisplayServer.WindowMode.Windowed;
				break;
            
			case 2:
				this.windowMode = DisplayServer.WindowMode.ExclusiveFullscreen;
				break;
            
			default:
				break;
		}
		DisplayServer.WindowSetMode(this.windowMode);
	}

	public void SetFTUE(bool val)
	{
		this.ftue = val;
		SaveSettings();
	}
}
