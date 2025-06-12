using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot.Collections;

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
    public RoomData lastRoom { get; private set; } = null;
    public RunStats stats { get; private set; } = null;

    public Run(PlayerData playerData)
    {
        this.gold = 10;
        this.reRolls = 2;
        this.rng = new RandomNumberGenerator();
        this.rng.Randomize();
        
        this.playerData = playerData.CreateInstance();
        this.playerData.startingDeck.LoadDataToDeck();

        if (this.playerData.id == CharacterData.CharacterID.ROBODEV)
        {
            this.gold += 10;
            this.reRolls += 2;
        }

        this.playerDeck = new CardPile();
        this.playerDeck.SetDeck(this.playerData.startingDeck);
        
        this.powerUpHandler = new PowerUpHandler();
        
        this.stats = new RunStats();

        //GD.Print("Starting new run!");
    }

    public void SetMapData(List<List<RoomData>> mapData)
    {
        this.mapData = mapData;
    }

    public void ClimbFloor(RoomData roomData)
    {
        this.floorsClimbed++;
        this.lastRoom = roomData;
    }

    public void AddMoney(int amount)
    {
        this.gold += amount;
        AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.GET_COIN], true);
    }

    public void SpendReRoll()
    {
        this.reRolls--;

        if (this.reRolls < 0)
        {
            this.reRolls = 0;
        }

        this.stats.reRollsUsed++;
    }
    
    public void BuyReRoll()
    {
        this.reRolls++;
    }

    public void Rest()
    {
        float val = playerData.maxHealth * .3f;
        val = Mathf.RoundToInt(val);
        
        this.playerData.SetHealth(Math.Clamp(this.playerData.health + (int)val, 0, this.playerData.maxHealth));
    }

    public void FullHeal()
    {
        TakeMoney(10);

        this.playerData.SetHealth(this.playerData.maxHealth);
    }

    public void AddMaxHealth(int amount)
    {
        this.playerData.AddMaxHealth(amount);
    }

    public void TakeMoney(int amount)
    {
        this.gold -= amount;

        if (this.gold < 0)
        {
            this.gold = 0;
        }
        
        this.stats.moneySpent += amount;
    }

    public bool ContainsPowerUp(PowerUp.PowerUpID id)
    {
        foreach (PowerUp powerUp in this.powerUpHandler.powerUps)
        {
            if (powerUp.id == id)
            {
                return true;
            }
        }

        return false;
    }

    public Godot.Collections.Dictionary<StringName, Variant> saveRun()
    {
        Godot.Collections.Dictionary<StringName, Variant> runData = new Godot.Collections.Dictionary<StringName, Variant>()
        {
            {"Gold", gold},
            {"ReRolls", reRolls},
            {"Floors Climbed", floorsClimbed},
            {"Seed", rng.GetSeed()},
            {"Stats", this.stats.saveStats()}
        };

        if (this.lastRoom != null)
        {
            runData.Add("Last Room Row", this.lastRoom.row);
            runData.Add("Last Room Column", this.lastRoom.column);
        }
        else
        {
            runData.Add("Last Room Row", -1);
            runData.Add("Last Room Column", -1);
        }

        Godot.Collections.Dictionary<StringName, Variant> roomData = new Godot.Collections.Dictionary<StringName, Variant>();
        for (int i = 0; i < this.mapData.Count; i++)
        {
            for (int j = 0; j < this.mapData[i].Count; j++)
            {
                roomData.Add("Room " + this.mapData[i][j].row + " - " + this.mapData[i][j].column, this.mapData[i][j].saveRoomData());
            }
        }

        Godot.Collections.Dictionary<StringName, Variant> data = new Godot.Collections.Dictionary<StringName, Variant>();
        data.Add("Run Data", runData);
        data.Add("Player Data",playerData.savePlayerData());
        data.Add("Player Deck",playerDeck.saveCardPile());
        data.Add("Power Ups",powerUpHandler.SavePowerUps());
        data.Add("Map Data", roomData);
        
        return data;
    }

    public void LoadRun(Dictionary data)
    {
        Dictionary runData = (Dictionary)data["Run Data"];
        
        this.gold = (int) runData["Gold"];
        this.reRolls = (int) runData["ReRolls"];
        this.floorsClimbed = (int) runData["Floors Climbed"];
        this.rng.SetSeed((ulong)runData["Seed"]);
        this.stats.loadStats((Dictionary) runData["Stats"]);
        
        Dictionary playerData = (Dictionary)data["Player Data"];
        int id = (int) playerData["ID"];
        this.playerData = ResourceManager.instance.characters[(CharacterData.CharacterID) id].CreateInstance();
        this.playerData.LoadData(playerData);
        
        Dictionary deckData = (Dictionary)data["Player Deck"];
        this.playerDeck = new CardPile();
        this.playerDeck.LoadDeck(deckData);
        
        Dictionary powerUpData = (Dictionary)data["Power Ups"];
        this.powerUpHandler = new PowerUpHandler();
        this.powerUpHandler.LoadPowerUps(powerUpData);
        
        Dictionary mapData = (Dictionary)data["Map Data"];

        int count = 0;
        List<RoomData> floorData = new List<RoomData>();
        foreach (KeyValuePair<Variant, Variant> roomData in mapData)
        {
            RoomData newRoomData = new RoomData();
            newRoomData.LoadRoomData((Dictionary)roomData.Value);
            
            if (newRoomData.row > count)
            {
                this.mapData.Add(floorData);
                floorData = new List<RoomData>();
                count = newRoomData.row;
            }
            
            floorData.Add(newRoomData);
        }
        this.mapData.Add(floorData);

        if ((int) runData["Last Room Row"] != -1)
        {
            this.lastRoom = this.mapData[(int) runData["Last Room Row"]][(int) runData["Last Room Column"]];
        }
    }
}
