using Godot;

[GlobalClass]
public partial class EventManager : Node
{
	public static EventManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}
	
	[Signal] public delegate void CombatStartedEventHandler();
	[Signal] public delegate void CombatEndedEventHandler();
	[Signal] public delegate void PlayerTurnStartedEventHandler();
	[Signal] public delegate void PlayerTurnEndedEventHandler();
	
	[Signal] public delegate void PlayerDamagedEventHandler();
	[Signal] public delegate void PlayerHealedEventHandler();
	

	public override void _Ready()
	{
		init();
	}
}
