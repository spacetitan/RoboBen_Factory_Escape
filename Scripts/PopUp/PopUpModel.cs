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

    public void OpenPopUp(ViewID ID)
    {
        this.ShowModel();
        this.views[ID].ShowView();
    }

    public void OpenGenericPopUp(GenericPopUpView.GenericPopUpData data)
    {
        this.ShowModel();
        GenericPopUpView popUpView = this.views[ViewID.POP_UP] as GenericPopUpView;
        popUpView.OpenPopUp(data);
    }

    public void OpenDeckPopUp(CardPile deck, string title)
    {
        this.ShowModel();
        DeckPopUpView popUpView = this.views[ViewID.DECK] as DeckPopUpView;
        popUpView.OpenPopUp(deck, title);
    }

    public void OpenBattleWinView(int money)
    {
        this.ShowModel();
        BattleWinView battleWinView = this.views[ViewID.WIN] as BattleWinView;
        battleWinView.OpenPopUp(money);
    }

    public void OpenRewardDraft(RewardDraftView.Type rewardType, bool isLayered = false)
    {
        this.ShowModel();
        RewardDraftView rewardDraftView = this.views[ViewID.REWARD] as RewardDraftView;
        rewardDraftView.OpenPopUp(rewardType, isLayered);
    }

    public void OpenMapPopUp()
    {
        this.ShowModel();
        MapView view = this.views[ViewID.MAP] as MapView;
        view.OpenPopUp();
    }

    public void ClosePopup(ViewID popupID, bool isLayered = false)
    {
        this.views[popupID].HideView();
        
        if (!isLayered)
        {
            this.HideModel();
        }
    }
    
    public void CloseRewardDraft(bool isLayered = false)
    {
        this.views[ViewID.REWARD].HideView();

        if (!isLayered)
        {
            this.HideModel();
        }
    }
}
