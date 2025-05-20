using Godot;
using System.Collections.Generic;
using System.Linq;
using System;
using Godot.Collections;

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
	
	public List<BattleData> battleDataPool = new List<BattleData>();
	float[] totalBattleWeightByTier = new float[3] {0.0f, 0.0f, 0.0f};
	
	public List<EventData> eventDataPool = new List<EventData>();
	float[] totalEventWeightByTier = new float[3] {0.0f, 0.0f, 0.0f};
	public List<CardData> availableCards { get; private set; } = new List<CardData>();
	public List<PowerUp> availablePowerUps { get; private set; } = new List<PowerUp>();
	
	const float BASE_COMMON_WEIGHT = 6.0F;
	const float BASE_UNCOMMON_WEIGHT = 3.0F;
	const float BASE_RARE_WEIGHT = 1.0F;
	private float rarityTotalWeight = 0;
	private System.Collections.Generic.Dictionary<int, float> rarityWeight = new System.Collections.Generic.Dictionary<int, float>();
	private float rarityWeightCommon = 0;
	private float rarityWeightUncommon = 0;
	private float rarityWeightRare = 0;

	public override void _Ready()
	{
		init();

		foreach (KeyValuePair<BattleData.BattleID, BattleData> data in ResourceManager.instance.battles)
		{
			this.battleDataPool.Add(data.Value);
		}
		
		foreach (KeyValuePair<EventData.EventID, EventData> data in ResourceManager.instance.events)
		{
			this.eventDataPool.Add(data.Value);
		}
		
		foreach (KeyValuePair<CardData.CardID, CardData> card in ResourceManager.instance.cards)
		{
			this.availableCards.Add(card.Value);
		}
		
		foreach (KeyValuePair<PowerUp.PowerUpID, PowerUp> powerUp in ResourceManager.instance.powerUps)
		{
			this.availablePowerUps.Add(powerUp.Value);
		}
		
		for (int i = 0; i < 3; i++)
		{
			SetupBattleWeightForTier(i);
		}
		
		for (int i = 0; i < 2; i++)
		{
			SetupEventWeightForTier(i);
		}
	}

	public void NewRun(PlayerData playerData)
	{
		this.currentRun = new Run(playerData);
		
		AddPowerUp(playerData.starterPowerUp);
		this.currentRun.SetMapData(this.mapGenerator.GenerateNewMap());
		UIManager.instance.ChangeStateTo(UIManager.UIState.RUN);
	}

	public void ContinueRun(Dictionary data)
	{
		Dictionary playerData = (Dictionary)data["Player Data"];
		int playerId = (int) playerData["ID"];
		this.currentRun = new Run(ResourceManager.instance.characters[(CharacterData.CharacterID) playerId]);
		this.currentRun.LoadRun(data);
		
		UIManager.instance.ChangeStateTo(UIManager.UIState.RUN);
	}

	public void SelectRoom(RoomData roomData)
	{
		this.currentRun.ClimbFloor(roomData);
		
		RunModel runmodel = UIManager.instance.models[UIManager.UIState.RUN] as RunModel;
		runmodel.SetLastRoom(roomData);

		switch (roomData.type)
		{
			case RoomData.Type.COMBAT:
				if (roomData.battleData != null)
				{
					BattleModel battleModel = UIManager.instance.models[UIManager.UIState.BATTLE] as BattleModel;
					battleModel.battleData = new BattleData();
					battleModel.battleData.SetData(roomData.battleData);
				}

				UIManager.instance.ChangeStateTo(UIManager.UIState.BATTLE);
				break;
			
			case RoomData.Type.REST:
				UIManager.instance.ChangeStateTo(UIManager.UIState.REST);
				break;
			
			case RoomData.Type.SHOP:
				UIManager.instance.ChangeStateTo(UIManager.UIState.SHOP);
				break;
			
			case RoomData.Type.TREASURE:
				UIManager.instance.ChangeStateTo(UIManager.UIState.TREASURE);
				break;
			
			case RoomData.Type.EVENT:
				if (roomData.eventData != null)
				{
					//UIManager.instance.ChangeStateTo(UIManager.UIState.EVENT);
					
					switch (roomData.eventData.id)
					{
						case EventData.EventID.MOVE_REST:
							UIManager.instance.ChangeStateTo(UIManager.UIState.REST);
							break;
						
						case EventData.EventID.MOVE_SHOP:
							UIManager.instance.ChangeStateTo(UIManager.UIState.SHOP);
							break;
						
						case EventData.EventID.MOVE_TREASURE:
							UIManager.instance.ChangeStateTo(UIManager.UIState.TREASURE);
							break;
						
						case EventData.EventID.MOVE_COMBAT:
							BattleModel battleModel = UIManager.instance.models[UIManager.UIState.BATTLE] as BattleModel;
							battleModel.battleData = new BattleData();
							battleModel.battleData.SetData(RunManager.instance.GetRandomBattleforTier(0));
							UIManager.instance.ChangeStateTo(UIManager.UIState.BATTLE);
							break;
						
						default:
							EventModel eventModel = UIManager.instance.models[UIManager.UIState.EVENT] as EventModel;
							eventModel.SetData(roomData.eventData);
							UIManager.instance.ChangeStateTo(UIManager.UIState.EVENT);
							break;
					}
				}
				break;
		}
	}
	
	public List<BattleData> GetAllBattlesForTier(int tier)
	{
		List<BattleData> tierBattleData = new List<BattleData>();

		foreach (BattleData data in this.battleDataPool)
		{
			if (data.tier == tier)
			{
				tierBattleData.Add(data);
			}
		}
		
		return tierBattleData;
	}

	private void SetupBattleWeightForTier(int tier)
	{
		totalBattleWeightByTier[tier] = 0.0f;

		foreach (BattleData data in GetAllBattlesForTier(tier))
		{
			totalBattleWeightByTier[tier] += data.weight;
			data.accumilatedWeight = totalBattleWeightByTier[tier];
		}
	}

	public BattleData GetRandomBattleforTier(int tier)
	{
		float roll = this.currentRun.rng.RandfRange(0.0f, this.totalBattleWeightByTier[tier]);

		foreach (BattleData data in GetAllBattlesForTier(tier))
		{
			if (data.accumilatedWeight >= roll)
			{
				return data;
			}
		}
		
		GD.Print("No data found. Returning null.");
		return null;
	}
	
	public void SetupChances()
	{
		this.rarityTotalWeight = BASE_COMMON_WEIGHT + BASE_UNCOMMON_WEIGHT + BASE_RARE_WEIGHT;

		this.rarityWeight.Clear();
		this.rarityWeight.Add(1, BASE_COMMON_WEIGHT); //common weight
		this.rarityWeight.Add(2, BASE_COMMON_WEIGHT + BASE_UNCOMMON_WEIGHT); //uncommon
		this.rarityWeight.Add(3, this.rarityTotalWeight);//rare
	}
	
	public EventData[] GetAllEventsForTier(int tier)
	{
		List<EventData> tierBattleData = new List<EventData>();

		foreach (EventData data in this.eventDataPool)
		{
			if (data.tier == tier)
			{
				tierBattleData.Add(data);
			}
		}
		
		return tierBattleData.ToArray();
	}

	private void SetupEventWeightForTier(int tier)
	{
		totalEventWeightByTier[tier] = 0.0f;

		foreach (EventData data in GetAllEventsForTier(tier))
		{
			totalEventWeightByTier[tier] += data.weight;
			data.accumilatedWeight = totalEventWeightByTier[tier];
		}
	}

	public EventData GetRandomEventforTier(int tier)
	{
		float roll = this.currentRun.rng.RandfRange(0.0f, this.totalEventWeightByTier[tier]);

		foreach (EventData data in GetAllEventsForTier(tier))
		{
			if (data.accumilatedWeight >= roll)
			{
				return data;
			}
		}
		
		GD.Print("No data found. Returning null.");
		return null;
	}
	
	public void AddPowerUp(PowerUp powerUp)
	{
		if(!this.currentRun.ContainsPowerUp(powerUp.id) && this.availablePowerUps.Contains(powerUp))
		{
			this.availablePowerUps.Remove(powerUp);
			this.currentRun.powerUpHandler.AddPowerUp(powerUp);
			GD.Print("Adding " + powerUp.name + " to backpack!");
		}
		else
		{
			GD.Print("Cannot add " + powerUp.name);
		}
	}

	public void AddRandomPowerUp()
	{
		PowerUp powerUp = GetAvailablePowerUp();
		if(!this.currentRun.ContainsPowerUp(powerUp.id) && this.availablePowerUps.Contains(powerUp))
		{
			this.availablePowerUps.Remove(powerUp);
			this.currentRun.powerUpHandler.AddPowerUp(powerUp);
			//GD.Print("Adding " + powerUp.name + " to backpack!");
		}
	}
	
	public PowerUp GetAvailablePowerUp()
	{
		SetupChances();
		RandomNumberGenerator rng = new RandomNumberGenerator();
		float roll = rng.RandfRange(1.0f, this.rarityTotalWeight);
		
		PowerUp powerUp = GetRandomPowerUp(roll);
		if(powerUp == null)
		{
			powerUp = this.availablePowerUps[0].CreateInstance();
			GD.Print("Could not get powerUp");
		}

		return powerUp;
	}

	public List<PowerUp> GetAvailablePowerUps(int num)
	{
		List<PowerUp> powerUps = new List<PowerUp>();

		for (int i = 0; i < num; i++)
		{
			PowerUp powerUp = null;
			int count = 0;

			while (powerUps.Count < i+1 && count < 1000)
			{
				powerUp = GetAvailablePowerUp();
				if (!powerUps.Contains(powerUp))
				{
					powerUps.Add(powerUp);
				}

				count++;
			}
		}

		return powerUps;
	}

	private PowerUp GetRandomPowerUp(float roll)
	{
		Random rand = new Random();
		List<PowerUp> powerUps = this.availablePowerUps.OrderBy(x => rand.Next()).ToList();

		foreach (PowerUp powerUp in powerUps)
		{
			if(this.rarityWeight[(int)powerUp.rarity] > roll)
			{
				return powerUp;
			}
		}
		return null;
	}
	
	public void AddCard(CardData cardData)
	{
		this.currentRun.playerDeck.AddCard(cardData);
	}

	public void AddRandomCard()
	{
		CardData cardStats = GetAvailableCard();
		this.currentRun.playerDeck.AddCard(cardStats);
	}

	public void RemoveCard(CardData cardData)
	{
		if (this.currentRun.playerDeck.cards.Contains(cardData))
		{
			this.currentRun.playerDeck.RemoveCard(cardData);
		}
	}

	public virtual void RemoveRandomCard() { }
	
	public CardData GetAvailableCard()
	{
		SetupChances();
		float roll = RunManager.instance.currentRun.rng.RandfRange(0.0f, this.rarityTotalWeight);
		//GD.Print(roll);

		CardData cardData = GetRandomCard(roll);
		return cardData;
	}

	public List<CardData> GetAvailableCards(int num)
	{
		List<CardData> cards = new List<CardData>();

		for (int i = 0; i < num; i++)
		{
			CardData card = null;
			int count = 0;

			while (cards.Count < i+1 && count < 1000)
			{
				card = GetAvailableCard();
				if (!cards.Contains(card))
				{
					cards.Add(card);
				}

				count++;
			}
		}

		return cards;
	}

	private CardData GetRandomCard(float roll)
	{
		Random rand = new Random();
		List<CardData> cardStats = this.availableCards.OrderBy(x => rand.Next()).ToList();

		foreach (CardData card in cardStats)
		{
			if(this.rarityWeight[(int)card.rarity] > roll)
			{
				return card;
			}
		}

		GD.Print("Cannot get random card, returning null");
		return null;
	}

	public void Rest()
	{
		this.currentRun.Rest();
	}

	public void FullHeal()
	{
		this.currentRun.FullHeal();
	}
}
