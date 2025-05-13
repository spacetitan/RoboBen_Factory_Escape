using Godot;
using System;

public partial class PopUpModel : UIModel
{
    public override void InitializeModel()
    {
        GetSceneNodes();
        base.InitializeModel();
    }
    
    private void GetSceneNodes()
    {
        
    }

    public void OpenPopUp(string viewID)
    {
        this.ShowModel();
        this.views[viewID].ShowView();
    }

    public void OpenGenericPopUp(GenericPopUpView.GenericPopUpData data)
    {
        this.ShowModel();
        GenericPopUpView popUpView = this.views["Generic"] as GenericPopUpView;
        popUpView.OpenPopUp(data);
    }

    public void OpenDeckPopUp(CardPile deck, string title)
    {
        this.ShowModel();
        DeckPopUpView popUpView = this.views["deck"] as DeckPopUpView;
        popUpView.OpenPopUp(deck, title);
    }

    public void OpenBattleWinView(int money)
    {
        this.ShowModel();
        BattleWinView battleWinView = this.views["battleWin"] as BattleWinView;
        battleWinView.OpenPopUp(money);
    }

    public void OpenRewardDraft(RewardDraftView.Type rewardType, bool isLayered = false)
    {
        this.ShowModel();
        RewardDraftView rewardDraftView = this.views["rewardDraft"] as RewardDraftView;
        rewardDraftView.OpenPopUp(rewardType, isLayered);
    }

    public void ClosePopup(string popupName, bool isLayered = false)
    {
        this.views[popupName].HideView();
        
        if (!isLayered)
        {
            this.HideModel();
        }
    }
    
    public void CloseRewardDraft(bool isLayered = false)
    {
        this.views["rewardDraft"].HideView();

        if (!isLayered)
        {
            this.HideModel();
        }
    }
}
