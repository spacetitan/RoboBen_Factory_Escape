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

    public override bool IsPerformable()
    {
        return base.IsPerformable();
    }

    public override void PerformAction(Character target)
    {
        //GD.Print(enemy.data.name + " attacks!");
        if(enemy == null || target == null || alreadyUsed)
        {
            if(enemy == null)
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

        if(this.type == EnemyActionType.CONDITIONAL)
        {
            alreadyUsed = true;
        }
        
        base.PerformAction();
    }
}
