using Godot;
using System;
using System.Collections.Generic;

public partial class RewardDraftView : UIView
{
    public enum Type { NONE, POWERUP, CARD, MIXED };
    public HBoxContainer rewardContainer = null;
    public UIButton closeButton = null;
    public UIButton reRollButton = null;
    
    private bool isLayered = false;
    private Type rewardType = Type.CARD;
    
    private Vector2 rewardSize = Vector2.Zero;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.rewardContainer = this.GetNode<HBoxContainer>("%RewardContainer");
        this.closeButton = this.GetNode<UIButton>("%CloseButton");
        this.closeButton.SetData("Close");
        this.closeButton.button.Pressed += ClosePopUp;
        this.reRollButton = this.GetNode<UIButton>("%ReRollButton");
        this.reRollButton.button.Pressed += ReRoll;

        this.rewardSize = new Vector2(this.rewardContainer.Size.X / 4, this.rewardContainer.Size.Y * 0.7f);
    }
    
    public override void Exit()
    {
        foreach (Node node in this.rewardContainer.GetChildren())
        {
            node.QueueFree();
        }
    }

    public void OpenPopUp(Type type, bool isLayered)
    {
        this.reRollButton.SetData("Re-Rolls (" + RunManager.instance.currentRun.reRolls + ")", ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.REROLL]);
        SpawnRewards(type);
        this.isLayered = isLayered;
        this.rewardType = type;
        
        this.ShowView();
    }

    public void ClosePopUp()
    {
        UIManager.instance.popUpModel.ClosePopup(UIModel.ViewID.REWARD, this.isLayered);
    }

    public void SpawnRewards(Type type)
    {
        int count = 3;
        if (RunManager.instance.currentRun.ContainsPowerUp(PowerUp.PowerUpID.LUCKYPENNY))
        {
            float roll = RunManager.instance.currentRun.rng.Randf();
            //GD.Print("Roll: " + roll);
            if (roll >= .06f)
            {
                count++;
            }
        }


        List<PowerUp> powerUps = null;
        List<CardData> cards = null;
        if (type == Type.POWERUP)
        {
            powerUps = RunManager.instance.GetAvailablePowerUps(count);
        }
        else if (type == Type.CARD)
        {
            cards = RunManager.instance.GetAvailableCards(count);
        }

        for (int i = 0; i < count; i++)
        {
            CardDisplayUI displayUi = ResourceManager.instance.displayCard.Instantiate() as CardDisplayUI;
            this.rewardContainer.AddChild(displayUi);
            displayUi.GetSceneNodes();
            displayUi.SetCustomMinimumSize(this.rewardSize);
            
            switch (type)
            {
                case Type.POWERUP:
                    displayUi.SetData(powerUps[i], () =>
                    {
                        OnRewardButtonPressed(displayUi);
                    });
                    break;
            
                case Type.CARD:
                    displayUi.SetData(cards[i], () =>
                    {
                        OnRewardButtonPressed(displayUi);
                    });
                    break;
            
                case Type.MIXED:
                    break;
                
                case Type.NONE:
                default:
                    break;
            }
        }
    }

    public void OnRewardButtonPressed(CardDisplayUI displayUi)
    {
        if (displayUi.cardData != null)
        {
            RunManager.instance.AddCard(displayUi.cardData);

            if (UIManager.instance.currentModel is BattleModel)
            {
                BattleModel battleModel = UIManager.instance.currentModel as BattleModel;
                battleModel.player.deck.AddCard(displayUi.cardData);
            }
            else
            {
                RoomHUDView view = UIManager.instance.hudModel.views[UIModel.ViewID.ROOM_HUD] as RoomHUDView;
                view.UpdateData();
            }
            
        }
        else if(displayUi.powerUp != null)
        {
            RunManager.instance.AddPowerUp(displayUi.powerUp);
        }
        
        UIManager.instance.popUpModel.ClosePopup(UIModel.ViewID.REWARD, this.isLayered);
    }

    public void ReRoll()
    {
        if (RunManager.instance.currentRun.reRolls > 0)
        {
            RunManager.instance.currentRun.SpendReRoll();
            this.reRollButton.SetData("Re-Rolls (" + RunManager.instance.currentRun.reRolls + ")");
        
            foreach (Node node in this.rewardContainer.GetChildren())
            {
                node.QueueFree();
            }
        
            SpawnRewards(this.rewardType);
        }
    }
}
