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
	public PackedScene uiButtonScene = null;
	
	public Dictionary<CharacterData.CharacterID, PlayerData> characters = new Dictionary<CharacterData.CharacterID, PlayerData>();
	public PackedScene playerScene = null;
	
	public Dictionary<CharacterData.CharacterID, EnemyData> enemies = new Dictionary<CharacterData.CharacterID, EnemyData>();
	public PackedScene enemyScene = null;

	public enum IntentID { NONE, }
	public Dictionary<StringName, Texture2D> intentIcons = new Dictionary<StringName, Texture2D>();
	
	public Dictionary<BattleData.BattleID, BattleData> battles = new Dictionary<BattleData.BattleID,BattleData>();
	
	public Dictionary<Status.StatusID, Status> statuses = new Dictionary<Status.StatusID, Status>();
	
	public Dictionary<CardData.CardID, CardData> cards = new Dictionary<CardData.CardID, CardData>();
	
	public Dictionary<PowerUp.PowerUpID, PowerUp> powerUps = new Dictionary<PowerUp.PowerUpID, PowerUp>();
	public PackedScene displayCard = null;
	
	public Dictionary<RoomData.Type, Texture2D> runIcons = new Dictionary<RoomData.Type, Texture2D>();
	public Dictionary<RoomData.Type, Texture2D> runGlowIcons = new Dictionary<RoomData.Type, Texture2D>();
	
	public enum HUDIconID { NONE, HEALTH, MONEY, REROLL, MAP, POWERUP, CARD, }
	public Dictionary<HUDIconID, Texture2D> HUDIcons = new Dictionary<HUDIconID, Texture2D>();
	
	public Dictionary<StringName, Material> shaders = new Dictionary<StringName, Material>();

	public override void _Ready()
	{
		init();
		LoadResources();
	}

	public void LoadResources()
	{
		this.debugIcon = ResourceLoader.Load<Texture2D>("res://icon.svg");
		this.uiButtonScene = ResourceLoader.Load<PackedScene>("res://Scenes/HUDs/ui_button.tscn");
	
		string path = "res://Resources/Characters/";
		foreach (string fileName in DirAccess.GetFilesAt(path))
		{
			PlayerData data = ResourceLoader.Load<PlayerData>(path + fileName);
			this.characters.Add(data.id, data);
		}
		this.playerScene = ResourceLoader.Load<PackedScene>("res://Scenes/Player/player.tscn");
		
		path = "res://Resources/Enemies/";
		foreach (string fileName in DirAccess.GetFilesAt(path))
		{
			EnemyData data = ResourceLoader.Load<EnemyData>(path + fileName);
			this.enemies.Add(data.id, data);
		}
		
		this.intentIcons = new Dictionary<StringName, Texture2D>()
		{
			{"attack", ResourceLoader.Load<Texture2D>("res://Art/Sprites/Cards/sword-icon.png")},
			{"armor", ResourceLoader.Load<Texture2D>("res://Art/Sprites/Cards/armor-icon.png")},
		};
		this.enemyScene = ResourceLoader.Load<PackedScene>("res://Scenes/Battle/enemy.tscn");

		path = "res://Resources/Battle/";
		foreach (string fileName in DirAccess.GetFilesAt(path))
		{
			BattleData data = ResourceLoader.Load<BattleData>(path + fileName);
			this.battles.Add(data.battleID, data);
		}
		
		path = "res://Resources/Status/";
		foreach (string fileName in DirAccess.GetFilesAt(path))
		{
			Status data = ResourceLoader.Load<Status>(path + fileName);
			this.statuses.Add(data.id, data);
		}
		
		path = "res://Resources/Cards/";
		foreach (string fileName in DirAccess.GetFilesAt(path))
		{
			CardData data = ResourceLoader.Load<CardData>(path + fileName);
			this.cards.Add(data.id, data);
		}
		
		path = "res://Resources/PowerUps/";
		foreach (string fileName in DirAccess.GetFilesAt(path))
		{
			PowerUp data = ResourceLoader.Load<PowerUp>(path + fileName);
			this.powerUps.Add(data.id, data);
		}
		this.displayCard = ResourceLoader.Load<PackedScene>("res://Scenes/UI/card_display_ui.tscn");

		this.runIcons = new Dictionary<RoomData.Type, Texture2D>()
		{
			{RoomData.Type.COMBAT, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/battleIcon.png")},
			{RoomData.Type.EVENT, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/EventIcon.png")},
			{RoomData.Type.REST, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/rest-icon.png")},
			{RoomData.Type.SHOP, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/ShopIcon.png")},
			{RoomData.Type.TREASURE, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/TreasureCentered.png")},
		};
		
		this.runGlowIcons = new Dictionary<RoomData.Type, Texture2D>()
		{
			{RoomData.Type.COMBAT, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/combatHighlight.png")},
			{RoomData.Type.EVENT, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/eventHighlight.png")},
			{RoomData.Type.REST, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/restGlow.png")},
			{RoomData.Type.SHOP, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/shopHighlight.png")},
			{RoomData.Type.TREASURE, ResourceLoader.Load<Texture2D>("res://Art/Sprites/Run/treasureCenteredHighlight.png")},
		};

		this.HUDIcons = new Dictionary<HUDIconID, Texture2D>()
		{
			{HUDIconID.HEALTH, ResourceLoader.Load<Texture2D>("res://Art/Sprites/HUD/healthIcon.png")},
			{HUDIconID.MONEY, ResourceLoader.Load<Texture2D>("res://Art/Sprites/HUD/money-icon.png")},
			{HUDIconID.REROLL, ResourceLoader.Load<Texture2D>("res://Art/Sprites/HUD/rerollicon.png")},
			{HUDIconID.MAP, ResourceLoader.Load<Texture2D>("res://Art/Sprites/HUD/map-icon.png")},
			{HUDIconID.POWERUP, ResourceLoader.Load<Texture2D>("res://Art/Sprites/HUD/powerup-icon.png")},
			{HUDIconID.CARD, ResourceLoader.Load<Texture2D>("res://Art/Sprites/CardPile/card-icon.png")},
		};

		this.shaders = new Dictionary<StringName, Material>()
		{
			{"heal", ResourceLoader.Load<Material>("res://Art/Materials/HealVFX/HealMaterial.tres")},
			{"damage", ResourceLoader.Load<Material>("res://Art/Materials/DamageVFX/DamageMaterial.tres")},
		};
	}
}
