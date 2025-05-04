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
	public Dictionary<RoomData.Type, Texture2D> runIcons = new Dictionary<RoomData.Type, Texture2D>();
	public Dictionary<StringName, Texture2D> HUDIcons = new Dictionary<StringName, Texture2D>();

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

		this.runIcons = new Dictionary<RoomData.Type, Texture2D>()
		{
			{RoomData.Type.COMBAT, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/battleIcon.png")},
			{RoomData.Type.EVENT, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/EventIcon.png")},
			{RoomData.Type.REST, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/rest-icon.png")},
			{RoomData.Type.SHOP, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/ShopIcon.png")},
			{RoomData.Type.TREASURE, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/TreasureCentered.png")},
		};

		this.HUDIcons = new Dictionary<StringName, Texture2D>()
		{
			{"health",ResourceLoader.Load<Texture2D>("res://Art/Sprites/HUD/healthIcon.png")},
			{"money",ResourceLoader.Load<Texture2D>("res://Art/Sprites/HUD/money-icon.png")},
			{"reRoll",ResourceLoader.Load<Texture2D>("res://Art/Sprites/HUD/rerollicon.png")},
			{"map",ResourceLoader.Load<Texture2D>("res://Art/Sprites/HUD/map-icon.png")},
			{"powerUp",ResourceLoader.Load<Texture2D>("res://Art/Sprites/HUD/powerup-icon.png")},
		};
	}
}
