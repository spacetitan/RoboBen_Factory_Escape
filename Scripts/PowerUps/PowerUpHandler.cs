using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PowerUpHandler : Node
{
    private PackedScene powerUpUIScene = ResourceLoader.Load<PackedScene>("res://Scenes/HUDs/power_up_ui.tscn");
    
    public List<PowerUp> powerUps = new List<PowerUp>();
    private List<PowerUpUI> powerUpUI = new List<PowerUpUI>();
    
    private float applyInterval = 1f;
    
    public void AddPowerUp(PowerUp powerUp)
    {
        this.powerUps.Add(powerUp);
        powerUp.InitializePowerUp(this);
    }
    
    public void ActivatePowerUpsByType(PowerUp.ActivateType powerUpType, Action callback = null)
    {
        if(powerUpType == PowerUp.ActivateType.EVENT)
        {
            return;
        }

        List<PowerUpUI> powerUpQueue = new List<PowerUpUI>();

        foreach(PowerUpUI powerUpUI in this.powerUpUI)
        {
            if(powerUpUI.powerUp.activateType == powerUpType)
            {
                powerUpQueue.Add(powerUpUI);
            }
        }

        if(!powerUpQueue.Any())
        {
            //EmitSignal(SignalName.PowerUpsActivated, (int)powerUpType);
            callback?.Invoke();
            return;
        }

        Tween tween = CreateTween();
        foreach(PowerUpUI powerUpUI in powerUpQueue)
        {
            tween.TweenCallback(Callable.From(() => { powerUpUI.powerUp.ActivatePowerUp(); }));
            tween.TweenInterval(this.applyInterval);
        }
        tween.Finished += () => { callback?.Invoke(); };
    }

    public bool hasPowerUp(String id)
    {
        foreach(PowerUpUI powerUpUI in powerUpUI)
        {
            if(powerUpUI.powerUp.id == id)
            {
                return true;
            }
        }

        return false;
    }

    public void SpawnUI(GridContainer parentContainer)
    {
        bool inv = UIManager.instance.currentModel.state == UIManager.UIState.RUN;
        
        foreach (PowerUp powerUp in this.powerUps)
        {
            PowerUpUI powerUpUi = this.powerUpUIScene.Instantiate() as PowerUpUI;
            powerUpUi.GetSceneNodes();
            powerUpUi.SetData(powerUp, inv);
            parentContainer.AddChild(powerUpUi);
            powerUpUi.SetCustomMinimumSize(new Vector2(parentContainer.GetRect().Size.X / 2, parentContainer.GetRect().Size.X / 2));
            powerUpUi.PivotOffset = new Vector2(powerUpUi.GetRect().Size.X / 2, powerUpUi.GetRect().Size.Y / 2);
            powerUpUi.SetPosition(Vector2.Zero);
            this.powerUpUI.Add(powerUpUi);
        }
    }
    
    public void SpawnUI(HBoxContainer parentContainer)
    {
        bool inv = UIManager.instance.currentModel.state == UIManager.UIState.RUN;
        
        foreach (PowerUp powerUp in this.powerUps)
        {
            PowerUpUI powerUpUi = this.powerUpUIScene.Instantiate() as PowerUpUI;
            powerUpUi.GetSceneNodes();
            powerUpUi.SetData(powerUp, inv);
            parentContainer.AddChild(powerUpUi);
            //powerUpUi.SetCustomMinimumSize(new Vector2(parentContainer.GetRect().Size.Y / 2, parentContainer.GetRect().Size.Y / 2));
            powerUpUi.PivotOffset = new Vector2(powerUpUi.GetRect().Size.X / 2, powerUpUi.GetRect().Size.Y / 2);
            powerUpUi.SetPosition(Vector2.Zero);
            this.powerUpUI.Add(powerUpUi);
        }
    }

    public void ClearUI()
    {
        foreach (PowerUpUI powerUp in this.powerUpUI)
        {
            this.powerUpUI.Remove(powerUp);
            powerUp.QueueFree();
        }
    }

    public Godot.Collections.Dictionary<StringName, Variant> SavePowerUps()
    {
        Godot.Collections.Dictionary<StringName, Variant> data = new Godot.Collections.Dictionary<StringName, Variant>();
        for (int i = 0; i < powerUps.Count; i++)
        {
            data.Add("Power up " + i, powerUps[i].id);
        }
        return data;
    }
}
