using Godot;
using System;
using System.Collections.Generic;

public partial class Run
{
    public int gold { get; private set; } = 0;
    public int reRolls { get; private set; } = 0;
    public int floorsClimbed { get; private set; } = 0;
    public RandomNumberGenerator rng { get; private set; } = new RandomNumberGenerator();
    public PlayerData playerData { get; private set; } = new PlayerData();
    public CardPile playerDeck { get; private set; } = new CardPile();
    public PowerUpHandler powerUpHandler { get; private set; } = new PowerUpHandler();
    public List<List<RoomData>> mapData { get; private set; } = new List<List<RoomData>>();

    public Run(PlayerData playerData)
    {
        this.gold = 10;
        this.reRolls = 2;
        this.rng = new RandomNumberGenerator();
        this.rng.Randomize();
        
        this.playerData = playerData.CreateInstance();
        this.playerData.startingDeck.LoadDataToDeck();
        
        this.playerDeck = new CardPile();
        this.playerDeck.SetDeck(this.playerData.startingDeck);
        
        this.powerUpHandler = new PowerUpHandler();
        this.powerUpHandler.AddPowerUp(this.playerData.starterPowerUp);

        //GD.Print("Starting new run!");
    }

    public void SetMapData(List<List<RoomData>> mapData)
    {
        this.mapData = mapData;
    }

    public void ClimbFloor()
    {
        this.floorsClimbed++;
    }

    public Godot.Collections.Dictionary<StringName, Variant> saveRun()
    {
        Godot.Collections.Dictionary<StringName, Variant> runData = new Godot.Collections.Dictionary<StringName, Variant>()
        {
            {"Gold", gold},
            {"ReRolls", reRolls},
            {"Floors Climbed", floorsClimbed},
            {"Seed", rng.GetSeed()}
        };
        
        Godot.Collections.Dictionary<StringName, Variant> roomData = new Godot.Collections.Dictionary<StringName, Variant>();
        for (int i = 0; i < this.mapData.Count; i++)
        {
            for (int j = 0; j < this.mapData[i].Count; j++)
            {
                roomData.Add(this.mapData[i][j].ToString() + " - " + this.mapData[i][j].column, this.mapData[i][j].saveRoomData());
            }
        }

        Godot.Collections.Dictionary<StringName, Variant> data = new Godot.Collections.Dictionary<StringName, Variant>();
        data.Add("Run Data", runData);
        data.Add("Player Data",playerData.savePlayerData());
        data.Add("Player Deck",playerDeck.saveCardPile());
        data.Add("Power ups",powerUpHandler.SavePowerUps());
        data.Add("Map Data", roomData);
        
        return data;
    }
}
