using Godot;

[GlobalClass]
public partial class RunManager : Node
{
	public static RunManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}

	public Run currentRun { get; private set; } = null;
	public MapGenerator mapGenerator { get; private set; } = new MapGenerator();

	public override void _Ready()
	{
		init();
	}

	public void NewRun(PlayerData playerData)
	{
		this.currentRun = new Run(playerData);
		this.currentRun.SetMapData(this.mapGenerator.GenerateNewMap());
		//GameManager.instance.SaveGame();
	}

	public void SelectRoom(RoomData roomData)
	{
		this.currentRun.ClimbFloor();
	}
}
