using Godot;
using System.Collections.Generic;
using System.Linq;
using System;

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
	
	[Export] public BattleData[] battleDataPool;
	public List<CardData> availableCards { get; private set; } = new List<CardData>();
	public List<PowerUp> availablePowerUps { get; private set; } = new List<PowerUp>();
	float[] totalWeightByTier = new float[3] {0.0f, 0.0f, 0.0f};
	
	const float BASE_COMMON_WEIGHT = 6.0F;
	const float BASE_UNCOMMON_WEIGHT = 3.0F;
	const float BASE_RARE_WEIGHT = 1.0F;
	private float rarityTotalWeight = 0;
	private Dictionary<int, float> rarityWeight = new Dictionary<int, float>();
	private float rarityWeightCommon = 0;
	private float rarityWeightUncommon = 0;
	private float rarityWeightRare = 0;

	public override void _Ready()
	{
		init();
		
		for (int i = 0; i < 3; i++)
		{
			SetupWeightForTier(i);
		}
	}

	public void NewRun(PlayerData playerData)
	{
		this.currentRun = new Run(playerData);

		foreach (KeyValuePair<CardData.CardID, CardData> card in ResourceManager.instance.cards)
		{
			this.availableCards.Add(card.Value);
		}
		
		foreach (KeyValuePair<PowerUp.PowerUpID, PowerUp> powerUp in ResourceManager.instance.powerUps)
		{
			this.availablePowerUps.Add(powerUp.Value);
		}
		
		AddPowerUp(playerData.starterPowerUp);
		this.currentRun.SetMapData(this.mapGenerator.GenerateNewMap());
		//GameManager.instance.SaveGame();
	}

	public void SelectRoom(RoomData roomData)
	{
		this.currentRun.ClimbFloor();
		
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
				break;
		}
	}
	
	public BattleData[] GetAllBattlesForTier(int tier)
	{
		List<BattleData> tierBattleData = new List<BattleData>();

		foreach (BattleData data in this.battleDataPool)
		{
			if (data.tier == tier)
			{
				tierBattleData.Add(data);
			}
		}
		
		return tierBattleData.ToArray();
	}

	private void SetupWeightForTier(int tier)
	{
		totalWeightByTier[tier] = 0.0f;

		foreach (BattleData data in GetAllBattlesForTier(tier))
		{
			totalWeightByTier[tier] += data.weight;
			data.accumilatedWeight = totalWeightByTier[tier];
		}
	}

	public BattleData GetRandomBattleforTier(int tier)
	{
		float roll = this.currentRun.rng.RandfRange(0.0f, this.totalWeightByTier[tier]);

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
