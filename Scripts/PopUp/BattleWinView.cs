using Godot;
using System;

public partial class BattleWinView : UIView
{
    public VBoxContainer buttonContainer = null;
    public UIButton continueButton = null;
    
    Vector2 buttonSize = Vector2.Zero;

    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.buttonContainer = this.GetNode<Panel>("%ButtonPanel").GetNode<VBoxContainer>("%ButtonContainer");
        this.continueButton = this.GetNode<UIButton>("%ContinueButton");
        this.continueButton.SetData("Continue");
        this.continueButton.button.Pressed += () =>
        {
            UIManager.instance.popUpModel.ClosePopup(this.viewID);
            UIManager.instance.ChangeStateTo(UIManager.UIState.RUN);
        };
        
        this.buttonSize = new Vector2(this.continueButton.Size.Y, this.continueButton.Size.Y);
    }

    public override void Exit()
    {
        foreach (Node node in this.buttonContainer.GetChildren())
        {
            node.QueueFree();
        }
    }

    public void OpenPopUp(int money)
    {
        UIButton moneyButton = ResourceManager.instance.uiButtonScene.Instantiate() as UIButton;
        this.buttonContainer.AddChild(moneyButton);
        moneyButton.GetSceneNodes();
        moneyButton.SetCustomMinimumSize(this.buttonSize);
        moneyButton.SetData("Money: " + money, ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.MONEY]);
        moneyButton.button.Pressed += () =>
        {
            RunManager.instance.currentRun.AddMoney(money);
            moneyButton.QueueFree();
            
            BattleHUDView view = UIManager.instance.hudModel.views["battleHUD"] as BattleHUDView;
            view.UpdateMoney();
        };
        
        UIButton rewardButton = ResourceManager.instance.uiButtonScene.Instantiate() as UIButton;
        this.buttonContainer.AddChild(rewardButton);
        rewardButton.GetSceneNodes();
        rewardButton.SetCustomMinimumSize(this.buttonSize);
        
        float roll = RunManager.instance.currentRun.rng.RandfRange(0.0f, 1.0f);

        if (roll < 0.3f)
        {
            rewardButton.SetData("Power-Up", ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.POWERUP]);
            rewardButton.button.Pressed += () =>
            {
                UIManager.instance.popUpModel.OpenRewardDraft(RewardDraftView.Type.POWERUP, true);
                rewardButton.QueueFree();
            };
        }
        else
        {
            rewardButton.SetData("Card", ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.CARD]);
            rewardButton.button.Pressed += () =>
            {
                UIManager.instance.popUpModel.OpenRewardDraft(RewardDraftView.Type.CARD, true);
                rewardButton.QueueFree();
            };
        }
        
        this.ShowView();
    }
}
