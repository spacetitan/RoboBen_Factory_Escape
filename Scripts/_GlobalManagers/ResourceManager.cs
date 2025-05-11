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
	public PackedScene playerScene = null;
	
	public Dictionary<StringName, EnemyData> enemies = new Dictionary<StringName, EnemyData>();
	public Dictionary<StringName, Texture2D> intentIcons = new Dictionary<StringName, Texture2D>();
	public PackedScene enemyScene = null;
	
	public Dictionary<StringName, Status> statuses = new Dictionary<StringName, Status>();
	
	public Dictionary<StringName, CardData> cards = new Dictionary<StringName, CardData>();
	public Dictionary<StringName, PowerUp> powerUps = new Dictionary<StringName, PowerUp>();
	public PackedScene displayCard = null;
	
	public Dictionary<RoomData.Type, Texture2D> runIcons = new Dictionary<RoomData.Type, Texture2D>();
	public Dictionary<StringName, Texture2D> HUDIcons = new Dictionary<StringName, Texture2D>();
	
	public Dictionary<StringName, Material> shaders = new Dictionary<StringName, Material>();

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
			{"roboBen", ResourceLoader.Load<PlayerData>("res://Resources/Characters/RoboBen.tres")},
		};
		this.playerScene = ResourceLoader.Load<PackedScene>("res://Scenes/Player/player.tscn");
		
		this.enemies = new Dictionary<StringName, EnemyData>()
		{
			{"grubboid", ResourceLoader.Load<EnemyData>("res://Resources/Enemies/Grubboid.tres")},
			{"grubbig", ResourceLoader.Load<EnemyData>("res://Resources/Enemies/Grubbig.tres")},
			{"grubbfly", ResourceLoader.Load<EnemyData>("res://Resources/Enemies/Grubbfly.tres")},
			{"tortigrubb", ResourceLoader.Load<EnemyData>("res://Resources/Enemies/Tortigrubb.tres")},
			{"grubbmantis", ResourceLoader.Load<EnemyData>("res://Resources/Enemies/Grubbmantis.tres")},
		};
		this.intentIcons = new Dictionary<StringName, Texture2D>()
		{
			{"attack", ResourceLoader.Load<Texture2D>("res://Art/Sprites/Cards/sword-icon.png")},
			{"armor", ResourceLoader.Load<Texture2D>("res://Art/Sprites/Cards/armor-icon.png")},
		};
		this.enemyScene = ResourceLoader.Load<PackedScene>("res://Scenes/Battle/enemy.tscn");

		this.cards = new Dictionary<StringName, CardData>() { };
		this.powerUps = new Dictionary<StringName, PowerUp>() { };
		this.displayCard = ResourceLoader.Load<PackedScene>("res://Scenes/UI/card_display_ui.tscn");

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

		this.shaders = new Dictionary<StringName, Material>()
		{
			{"heal", ResourceLoader.Load<Material>("res://Art/Materials/HealVFX/HealMaterial.tres")},
			{"damage", ResourceLoader.Load<Material>("res://Art/Materials/DamageVFX/DamageMaterial.tres")},
		};
	}
}
