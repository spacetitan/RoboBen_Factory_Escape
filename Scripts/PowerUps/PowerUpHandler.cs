using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public partial class PowerUpHandler : Node
{
    private PackedScene powerUpUIScene = ResourceLoader.Load<PackedScene>("res://Scenes/HUDs/power_up_ui.tscn");

    public List<PowerUp> powerUps { get; private set; } = new List<PowerUp>();
    public List<PowerUpUI> powerUpUIs { get; private set; } = new List<PowerUpUI>();
    
    private float applyInterval = 1f;
    
    public Container container { get; private set; }
    
    public void AddPowerUp(PowerUp powerUp)
    {
        this.powerUps.Add(powerUp);
        powerUp.InitializePowerUp(this);

        if (this.container != null)
        {
            SpawnUI(powerUp);
        }
    }
    
    public void SetContainer(Container container)
    {
        ClearUI();
        
        this.container = container;
    }
    
    public void ActivatePowerUpsByType(PowerUp.ActivateType powerUpType, Action callback = null)
    {
        if(powerUpType == PowerUp.ActivateType.EVENT)
        {
            return;
        }

        List<PowerUpUI> powerUpQueue = new List<PowerUpUI>();

        foreach(PowerUpUI powerUpUI in this.powerUpUIs)
        {
            if(powerUpUI.powerUp.activateType == powerUpType)
            {
                powerUpQueue.Add(powerUpUI);
            }
        }

        if(!powerUpQueue.Any())
        {
            callback?.Invoke();
            return;
        }

        Tween tween = CreateTween();
        foreach(PowerUpUI powerUpUI in powerUpQueue)
        {
            tween.TweenCallback(Callable.From(() =>
            {
                powerUpUI.powerUp.ActivatePowerUp();
                powerUpUI.Flash();
            }));
            tween.TweenInterval(this.applyInterval);
        }
        tween.Finished += () => { callback?.Invoke(); };
    }

    public bool hasPowerUp(PowerUp.PowerUpID id)
    {
        foreach(PowerUpUI powerUpUI in powerUpUIs)
        {
            if(powerUpUI.powerUp.id == id)
            {
                return true;
            }
        }

        return false;
    }

    public PowerUpUI GetPowerUpUI(PowerUp.PowerUpID id)
    {
        foreach(PowerUpUI powerUpUI in powerUpUIs)
        {
            if(powerUpUI.powerUp.id == id)
            {
                return powerUpUI;
            }
        }
        
        GD.Print("No PowerUp Found, returning null");
        return null;
    }

    public void SpawnUI(PowerUp powerUp)
    {
        bool inv = UIManager.instance.currentModel.state == UIManager.UIState.RUN;
        
        PowerUpUI powerUpUi = this.powerUpUIScene.Instantiate() as PowerUpUI;
        Vector2 size = Vector2.Zero;
        if (inv)
        {
            size = new Vector2(this.container.GetRect().Size.X / 2, this.container.GetRect().Size.X / 2);
        }
        else
        {
            size = new Vector2(this.container.GetRect().Size.Y , this.container.GetRect().Size.Y);
        }
        powerUpUi.SetCustomMinimumSize(size);
        powerUpUi.GetSceneNodes();
        powerUpUi.SetData(powerUp, inv);
        this.container.AddChild(powerUpUi);
        powerUpUi.SetPosition(Vector2.Zero);
        this.powerUpUIs.Add(powerUpUi);
    }
    
    public void SpawnAllUI()
    {
        UIManager.UIState currentState = UIManager.instance.currentModel.state;
        Vector2 size = Vector2.Zero;
        
        foreach (PowerUp powerUp in this.powerUps)
        {
            PowerUpUI powerUpUi = this.powerUpUIScene.Instantiate() as PowerUpUI;
            powerUpUi.GetSceneNodes();
            this.container.AddChild(powerUpUi);

            switch (currentState)
            {
                case UIManager.UIState.RUN:
                    powerUpUi.SetData(powerUp, true);
                    size = new Vector2(this.container.GetRect().Size.X / 2, this.container.GetRect().Size.X / 2);
                    break;
                    
                case UIManager.UIState.GAMEOVER:
                    powerUpUi.SetData(powerUp, true);
                    size = new Vector2(this.container.GetRect().Size.X / 5 , this.container.GetRect().Size.X / 5);
                    break;
                
                default:
                    powerUpUi.SetData(powerUp, false);
                    size = new Vector2(this.container.GetRect().Size.Y , this.container.GetRect().Size.Y);
                    break;
            }
            GD.Print(size);
            powerUpUi.SetCustomMinimumSize(size);
            powerUpUi.SetPosition(Vector2.Zero);
            this.powerUpUIs.Add(powerUpUi);
        }
    }

    public void ClearUI()
    {
        foreach (PowerUpUI powerUp in this.powerUpUIs)
        {
            powerUp.QueueFree();
        }
        this.powerUpUIs.Clear();

        foreach (PowerUp powerUp in this.powerUps)
        {
            powerUp.ResetPowerUp();
        }
    }

    public Godot.Collections.Dictionary<StringName, Variant> SavePowerUps()
    {
        Godot.Collections.Dictionary<StringName, Variant> data = new Godot.Collections.Dictionary<StringName, Variant>();
        for (int i = 0; i < powerUps.Count; i++)
        {
            data.Add("Power up " + i, (int) powerUps[i].id);
        }
        return data;
    }

    public void LoadPowerUps(Dictionary data)
    {
        foreach (KeyValuePair<Variant, Variant> powerUpID in data)
        {
            int ID = (int)powerUpID.Value;
            RunManager.instance.AddPowerUp(ResourceManager.instance.powerUps[(PowerUp.PowerUpID) ID].CreateInstance());
        }
    }
}
