using Godot;
using Godot.Collections;

[GlobalClass]
public partial class ResourceManager : Node
{
	public static ResourceManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}
	
	public Texture2D debugIcon = null;
	
	public Dictionary<StringName, PlayerData> characters = new Dictionary<StringName, PlayerData>();
	
	public Dictionary<StringName, Status> statuses = new Dictionary<StringName, Status>();
	
	public Dictionary<StringName, CardData> cards = new Dictionary<StringName, CardData>();

	public override void _Ready()
	{
		init();
		LoadResources();
	}

	public void LoadResources()
	{
		this.debugIcon = ResourceLoader.Load<Texture2D>("res://icon.svg");
		
		this.characters = new Dictionary<StringName, PlayerData>()
		{
			{"roboBen", ResourceLoader.Load<PlayerData>("res://Resources/Characters/RoboBen.tres")}
		};
	}
}
