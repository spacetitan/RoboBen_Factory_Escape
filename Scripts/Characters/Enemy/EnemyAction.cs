using Godot;
using System;

public partial class EnemyAction : Resource
{
    [Signal] public delegate void OnActionCompleteEventHandler();

    public enum ActionType { NONE, CONDITIONAL, CHANCE };
    
    //[Export] public EnemyIntent intent;
    [Export] public ActionType type;
    [Export] public int value = 0;
    [Export(PropertyHint.Range, "0,10")] public int chanceWeight = 0;
    [Export] public StringName intentKey = "";
    [Export] public AudioStream sound;
    
    public float accumulatedweight = 0;
    public bool isUsed = false;

    public Enemy owner = null;
    public Character target = null;
    
    public virtual void InitializeAction(Enemy enemy)
    {
        this.owner = enemy;
        return;
    }

    public virtual string GetIntent()
    {
        return this.value.ToString();
    }

    public virtual bool IsPerformable()
    {
        if(this.type == ActionType.CHANCE)
        {
            return true;
        }

        if(this.owner == null || this.target == null || isUsed)
        {
            return false;
        }

        return false;
    }

    public virtual void PerformAction(Action callback = null)
    {
        if(this.owner == null || this.target == null)
        {
            if(this.owner == null)
            {
                GD.Print("Enemy is null");
            }
            else if(target == null)
            {
                GD.Print("target is null");
            }
            else
            {
                GD.Print("this ability has already been used");
            }
            return;
        }

        if(this.type == ActionType.CONDITIONAL)
        {
            isUsed = true;
        }
        
        callback?.Invoke();
    }

    public virtual EnemyAction CreateInstance()
    {
        EnemyAction instance = (EnemyAction) this.Duplicate();

        //instance.intent = this.intent;
        instance.type = this.type;
        instance.chanceWeight = this.chanceWeight;
        instance.sound = this.sound;
        instance.accumulatedweight = this.accumulatedweight;

        return instance;
    }
}
