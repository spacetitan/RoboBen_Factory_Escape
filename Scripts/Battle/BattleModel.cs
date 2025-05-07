using Godot;
using System;

public partial class BattleModel : UIModel
{
    public BattleData battleData = null;

    public override void Enter()
    {
        BattleView view = this.views["battle"] as BattleView;
        view.SetData(battleData);
        view.ShowView();
    }
}
