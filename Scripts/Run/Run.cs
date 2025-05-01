using Godot;
using System;
using System.Collections.Generic;

public partial class Run
{
    public int gold { get; private set; } = 0;
    public RandomNumberGenerator rng { get; private set; } = new RandomNumberGenerator();
    public PlayerData playerData { get; private set; } = new PlayerData();
    public CardPile playerDeck { get; private set; } = new CardPile();
    public PowerUpHandler powerUpHandler { get; private set; } = new PowerUpHandler();

    public Run(PlayerData playerData)
    {
        this.gold = 10;
        this.rng = new RandomNumberGenerator();
        this.playerData = playerData.CreateInstance();
        this.playerData.startingDeck.LoadDataToDeck();
        
        this.playerDeck = new CardPile();
        this.playerDeck.SetDeck(this.playerData.startingDeck);
        
        this.powerUpHandler = new PowerUpHandler();
        this.powerUpHandler.powerUps.Add(this.playerData.starterPowerUp);
        
        //GD.Print("Starting new run!");
    }

    public Godot.Collections.Dictionary<StringName, Variant> saveRun()
    {
        Godot.Collections.Dictionary<StringName, Variant> data = new Godot.Collections.Dictionary<StringName, Variant>()
        {
            {"gold", gold},
            {"seed", rng.GetSeed()}
        };

        foreach (KeyValuePair<StringName, Variant> pair in playerData.savePlayerData())
        {
            data.Add(pair.Key, pair.Value);
        }
        
        foreach (KeyValuePair<StringName, Variant> pair in playerDeck.saveCardPile())
        {
            data.Add(pair.Key, pair.Value);
        }
        
        foreach (KeyValuePair<StringName, Variant> pair in powerUpHandler.SavePowerUps())
        {
            data.Add(pair.Key, pair.Value);
        }
        
        return data;
    }
}
