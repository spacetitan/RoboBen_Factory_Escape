using Godot;
using System.Collections.Generic;

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
	float[] totalWeightByTier = new float[3] {0.0f, 0.0f, 0.0f};

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
		this.currentRun.SetMapData(this.mapGenerator.GenerateNewMap());
		//GameManager.instance.SaveGame();
	}

	public void SelectRoom(RoomData roomData)
	{
		this.currentRun.ClimbFloor();

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
			
			case RoomData.Type.EVENT:
				break;
			
			case RoomData.Type.REST:
				break;
			
			case RoomData.Type.SHOP:
				break;
			
			case RoomData.Type.TREASURE:
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
}
