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
	
	public Run currentRun = null;

	public override void _Ready()
	{
		init();
	}

	public void NewRun(PlayerData playerData)
	{
		this.currentRun = new Run(playerData);
		GameManager.instance.SaveGame();
	}
}
