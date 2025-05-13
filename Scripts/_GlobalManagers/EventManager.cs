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
	
	[Signal] public delegate void CardPlayedEventHandler();
	[Signal] public delegate void CardBurnedEventHandler();
	
	[Signal] public delegate void PlayerDamagedEventHandler();
	[Signal] public delegate void PlayerHealedEventHandler();
	
	[Signal] public delegate void PlayerDiedEventHandler();
	[Signal] public delegate void EnemyDiedEventHandler(Enemy enemy);
	
	[Signal] public delegate void CardAimStartedEventHandler(CardUI card);
	[Signal] public delegate void AbilityAimStartedEventHandler(Player player);
	[Signal] public delegate void AimEndedEventHandler();

	public override void _Ready()
	{
		init();
	}
}
