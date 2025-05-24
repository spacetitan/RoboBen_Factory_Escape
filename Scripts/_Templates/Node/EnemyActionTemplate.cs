using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class _CLASS_ : EnemyAction
{
    public override void InitializeAction(Enemy enemy)
    {
        base.InitializeAction(enemy);
    }
    
    public override string GetIntent()
    {
        return this.value.ToString();
    }

    public override bool IsPerformable()
    {
        return base.IsPerformable();
    }

    public override void PerformAction(Action callback = null)
    {
        //GD.Print(enemy.data.name + " attacks!");
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
        base.PerformAction();
    }
}
