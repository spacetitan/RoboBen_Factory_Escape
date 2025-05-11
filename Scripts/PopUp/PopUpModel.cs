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

    public void ClosePopup(string popupName)
    {
        this.views[popupName].HideView();
        this.HideModel();
    }
}
