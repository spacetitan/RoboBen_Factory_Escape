using Godot;
using System;

public partial class GenericPopUpView : UIView
{
    public struct GenericPopUpData 
    {
        public string title = "";
        public string body = "";
        public Texture2D texture;
        public string buttonText = "";
        public Action action;

        public GenericPopUpData()
        {
            this.title = "";
            this.body = "";
            this.buttonText = "";
        }
    }
    
    private RichTextLabel titleLabel = null;
    private RichTextLabel bodyLabel = null;
    private TextureRect popUpTexture = null;
    private Button closeButton = null;
    private Button confirmButton = null;
    
    private GenericPopUpData popUpData = new GenericPopUpData();
    
    public override void InitializeView(UIModel owner)
    {
        GetSceneNodes();
        base.InitializeView(owner);
    }
    
    private void GetSceneNodes()
    {
        this.titleLabel = GetNode<RichTextLabel>("%TitleLabel");

        HBoxContainer bodyContainer = GetNode<HBoxContainer>("%BodyContainer");
        this.bodyLabel = bodyContainer.GetNode<RichTextLabel>("%BodyLabel");
        this.popUpTexture = bodyContainer.GetNode<TextureRect>("%PopUpTexture");

        HBoxContainer buttonContainer = GetNode<HBoxContainer>("%ButtonContainer");
        this.closeButton = buttonContainer.GetNode<Button>("%CloseButton");
        this.closeButton.Pressed += () => UIManager.instance.popUpModel.ClosePopup(this.ID);
        this.confirmButton = buttonContainer.GetNode<Button>("%ConfrimButton");
    }
    
    public void OpenPopUp(GenericPopUpData data)
    {
        this.popUpData = data;
        
        this.titleLabel.Text = data.title;
        this.bodyLabel.Text = data.body;

        if(this.popUpData.texture != null)
        {
            this.popUpTexture.Texture = this.popUpData.texture;
            this.popUpTexture.Show();
        }

        if(this.popUpData.action != null)
        {
            this.confirmButton.Pressed += this.popUpData.action;
			
            if(this.popUpData.buttonText != "")
            {
                this.confirmButton.Text = this.popUpData.buttonText;
            }

            this.confirmButton.Show();
        }
        
        this.ShowView();
    }

    public override void Exit()
    {
        base.Exit();
        this.titleLabel.Text = "";
        this.bodyLabel.Text = "";
        this.popUpTexture.Texture = null;
        this.popUpTexture.Hide();

        if(this.popUpData.action != null)
        {
            this.confirmButton.Pressed -= this.popUpData.action;
        }
		
        this.confirmButton.Text = "Continue";
        this.confirmButton.Hide();

        this.popUpData = new GenericPopUpData();
    }
}

